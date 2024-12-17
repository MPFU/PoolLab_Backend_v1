using FirebaseAdmin.Auth;
using PoolLab.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace PoolLab.Application.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        //public async Task<string> SendPhoneOTPAsync(string phoneNumber)
        //{
        //    try
        //    {
        //        TimeSpan timeSpan = TimeSpan.FromMinutes(10);
        //        var sessionInfo = await FirebaseAuth.DefaultInstance.CreateSessionCookieAsync(phoneNumber, timeSpan);
        //        return sessionInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error sending OTP", ex);
        //    }
        //}

        public async Task<bool> VerifyPhoneOTPAsync(string idToken)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying OTP", ex);
            }
        }

        public async Task<string> SendEmailVerificationAsync(string email)
        {
            try
            {
                var link = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(email);
                return link;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email verification", ex);
            }
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                return user.EmailVerified;
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying email", ex);
            }
        }

        public async Task<bool> CreateAccFirebase(UserRecordArgs userRecordArgs)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
                return user.EmailVerified;
            }
            catch (Exception ex)
            {
                throw new Exception("Error create acc firebase", ex);
            }
        }
    }
}
