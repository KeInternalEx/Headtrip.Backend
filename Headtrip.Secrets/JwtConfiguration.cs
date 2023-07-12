using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Secrets
{
    public class JwtConfiguration
    {
        private static NameValueCollection _JwtSection { 
            get
            {
                return (NameValueCollection)ConfigurationManager.GetSection("GameServerJwtConfiguration");
            } 
        }

        public static string JwtSubject
        {
            get
            {
                var result = _JwtSection["Jwt:Subject"];
                if (result == null)
                {
                    throw new Exception("JWT CONFIGURATION MISSING SUBJECT");
                }

                return result;
            }
        }

        public static string JwtIssuer
        {
            get
            {
                var result = _JwtSection["Jwt:Issuer"];
                if (result == null)
                    throw new Exception("JWT CONFIGURATION MISSING ISSUER");

                return result;
            }
        }

        public static string JwtAudience
        {
            get
            {
                var result = _JwtSection["Jwt:Audience"];
                if (result == null)
                    throw new Exception("JWT CONFIGURATION MISSING AUDIENCE");

                return result;
            }


        }

        public static string JwtSigningKey
        {
            get
            {
                var result = _JwtSection["Jwt:SigningKey"];
                if (result == null)
                    throw new Exception("JWT CONFIGURATION MISSING SIGNING KEY");

                return result;
            }
        }

        public static string JwtEncryptionKey
        {
            get
            {
                var result = _JwtSection["Jwt:EncryptionKey"];
                if (result == null)
                    throw new Exception("JWT CONFIGURATION MISSING ENCRYPTION KEY");

                return result;
            }
        }

        public static string JwtHeaderName
        {
            get
            {
                var result = _JwtSection["Jwt:HeaderName"];
                if (result == null)
                    throw new Exception("JWT CONFIGURATION MISSING HEADER NAME");

                return result;
            }
        }

    }
}
