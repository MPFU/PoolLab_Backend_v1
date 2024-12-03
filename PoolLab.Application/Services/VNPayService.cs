using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PoolLab.Application.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;

        public VNPayService(IConfiguration configuration, IUnitOfWork unitOfWork, IPaymentService paymentService, IAccountService accountService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _accountService = accountService;
        }


        public async Task<string> CreatePaymentUrl(CreateVNPayDTO vNPayDTO, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            if (vNPayDTO.Amount <= 0 && vNPayDTO.Amount > 2000000)
            {
                return "Số tiền nạp không hợp lệ.\n Bạn chỉ có thể nạp lớn hơn 0 và bé hơn 2.000.000đ!";
            }

            var cus = await _unitOfWork.AccountRepo.GetByIdAsync(vNPayDTO.CustomerId);

            if (cus == null)
            {
                return "Không tìm thấy tài khoản này!";
            }

            PaymentDepositDTO paymentDepositDTO = new PaymentDepositDTO();
            paymentDepositDTO.Amount = vNPayDTO.Amount;
            paymentDepositDTO.AccountId = cus.Id;
            paymentDepositDTO.PaymentMethod = "VNPay";
            paymentDepositDTO.PaymentInfo = "Nạp Tiền";
            paymentDepositDTO.Status = "Đang Thanh Toán";
            paymentDepositDTO.TypeCode = 1;
            var pay = await _paymentService.CreateTransactionDeposit(paymentDepositDTO);
            if (!Guid.TryParse(pay, out _))
            {
                return pay;
            }

            var tick = DateTime.Now.Ticks.ToString();
            
            var vnpay = new VNPayLibrary();
            vnpay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (vNPayDTO.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", vnpay.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan nap tien:" + tick);
            vnpay.AddRequestData("vnp_BankCode", _configuration["Vnpay:BankCode"]);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.UtcNow.AddDays(1).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_TxnRef", pay);

            string paymentUrl = vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return paymentUrl;
        }

        public async Task<VnPaymentResponseModel> PaymentExecute(VNPayReturnQuery querry)
        {
            try
            {
                var vnpay = new VNPayLibrary();

                foreach (PropertyInfo prop in querry.GetType().GetProperties())
                {
                    var value = prop.GetValue(querry)?.ToString() ?? string.Empty;
                    vnpay.AddResponseData(prop.Name, value.ToString());
                }


                Guid orderId = Guid.Parse(querry.vnp_TxnRef);
                long vnpayTranId = Convert.ToInt64(querry.vnp_TransactionNo);
                string vnp_ResponseCode = querry.vnp_ResponseCode;
                var vnp_SecureHash = querry.vnp_SecureHash;
                decimal vnp_Amount = Convert.ToDecimal(querry.vnp_Amount) / 100;
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["Vnpay:HashSecret"]);
                if (!checkSignature)
                {
                    return new VnPaymentResponseModel()
                    {
                        Success = false
                    };
                }

                var trans = await _unitOfWork.PaymentRepo.GetByIdAsync(orderId);
                if (trans == null)
                {
                    return new VnPaymentResponseModel()
                    {
                        Success = false,
                        VnPayResponseCode = vnp_ResponseCode
                    };
                }

                var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)trans.AccountId);
                if (cus == null)
                {
                    return new VnPaymentResponseModel()
                    {
                        Success = false,
                        VnPayResponseCode = vnp_ResponseCode
                    };
                }

                await _unitOfWork.BeginTransactionAsync();

                trans.Status = vnp_ResponseCode == "00" ? "Hoàn Tất" : (vnp_ResponseCode == "24") ? "Đã Huỷ" : "Thất Bại";

                _unitOfWork.PaymentRepo.Update(trans);
                var result = await _unitOfWork.SaveAsync() > 0;

                if (vnp_ResponseCode == "00")
                {
                    var upBalance = await _accountService.DepositBalance(cus.Id, vnp_Amount);
                    if (upBalance != null)
                    {
                        return new VnPaymentResponseModel()
                        {
                            Success = false,
                            VnPayResponseCode = vnp_ResponseCode
                        };
                    }

                    await _unitOfWork.CommitTransactionAsync();

                    return new VnPaymentResponseModel
                    {
                        Success = true,
                        PaymentMethod = "VnPay",
                        OrderId = orderId.ToString(),
                        TransactionId = vnpayTranId.ToString(),
                        Token = vnp_SecureHash,
                        VnPayResponseCode = vnp_ResponseCode,
                    };
                }
               
                await _unitOfWork.CommitTransactionAsync();

                return new VnPaymentResponseModel
                {
                    Success = false,
                    VnPayResponseCode = vnp_ResponseCode,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.ToString());
            }

        }

    }
}
