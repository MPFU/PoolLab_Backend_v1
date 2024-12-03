using Microsoft.AspNetCore.Http;
using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IVNPayService
    {
        Task<string> CreatePaymentUrl(CreateVNPayDTO vNPayDTO, HttpContext context);

        public Task<VnPaymentResponseModel> PaymentExecute(VNPayReturnQuery query);
    }
}
