using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Firebase
{
    public static class FirebaseConfiguration
    {
        public static void InitializeFirebase()
        {

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("poollab-phucnvm-firebase-adminsdk-mlm8g-3163e43e22.json")
            });

        }
    }
}
