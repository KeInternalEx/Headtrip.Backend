using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Secrets
{
    public static class InternalEncryptionKeys
    {
        public static string EmailConfirmationKey
        {
            get
            {
                var result = ConfigurationManager.AppSettings["EmailConfirmationKey"];
                if (result == null)
                {
                    throw new Exception("MISSING EmailConfirmationKey");
                }

                if (result.Length < 16)
                    throw new Exception("EmailConfirmationKey NOT LONG ENOUGH");

                return result;
            }
        }
    }
}
