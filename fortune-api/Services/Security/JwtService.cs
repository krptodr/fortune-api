using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JWT;
using System.Configuration;
using Newtonsoft.Json;
using fortune_api.Exceptions;

namespace fortune_api.Services.Security
{
    public class JwtService : IJwtService
    {
        public const string DEFAULT_ISSUER = "fortune-api";
        public const string DEFAULT_AUDIENCE = "fortune-api";

        private string secretKey;

        public JwtService()
        {
            this.secretKey = ConfigurationManager.AppSettings["JWT_KEY"];
        }

        /// <summary>
        /// Creates a JWT with the provided parameters
        /// </summary>
        /// <param name="sub">Subject - The authenticated user</param>
        /// <param name="iss">Issuer - The issuer of the JWT</param>
        /// <param name="aud">Audience - The intended recipient of the JWT</param>
        /// <param name="nbf">Not Before - The token cannot be used before this time</param>
        /// <param name="exp">Expiration - The token cannot be used after this time</param>
        /// <param name="payload">Other - Any other items that should be added to the payload</param>
        /// <returns>The generated JWT</returns>
        public string CreateToken(string sub, string iss, string aud, DateTime nbf, DateTime exp, Dictionary<string, string> payload)
        {
            payload.Add("sub", sub);
            payload.Add("iss", iss);
            payload.Add("aud", aud);
            payload.Add("nbf", nbf.ToString());
            payload.Add("exp", exp.ToString());

            string token = JWT.JsonWebToken.Encode(payload, this.secretKey, JWT.JwtHashAlgorithm.HS256);

            return token;
        }

        /// <summary>
        /// Ensures the token has not been tampered with and returns the token's contents
        /// </summary>
        /// <param name="token">The JWT to be parsed</param>
        /// <returns>The data contained within the token</returns>
        /// <exception cref="InvalidCredentialsException">Throws an exception if the token has been tampered with</exception>
        public Dictionary<string, string> ParseToken(string token)
        {
            Dictionary<string, string> payload;
            try
            {
                string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);
                payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);
            }
            catch (Exception)
            {
                throw new InvalidCredentialsException();
            }
            if (!payload.ContainsKey("sub") ||
               !payload.ContainsKey("iss") ||
               !payload.ContainsKey("aud") ||
               !payload.ContainsKey("aud") ||
               !payload.ContainsKey("nbf") ||
               !payload.ContainsKey("exp"))
            {
                throw new InvalidCredentialsException();
            }
            return payload;
        }
    }
}