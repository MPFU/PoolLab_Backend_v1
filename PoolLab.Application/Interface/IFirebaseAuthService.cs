using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IFirebaseAuthService
    {
       // Task<string> SendPhoneOTPAsync(string phoneNumber);
        Task<bool> VerifyPhoneOTPAsync(string idToken);
        Task<string> SendEmailVerificationAsync(string email);
        Task<bool> VerifyEmailAsync(string email);
        Task<bool> CreateAccFirebase(UserRecordArgs userRecordArgs);
    }
}
