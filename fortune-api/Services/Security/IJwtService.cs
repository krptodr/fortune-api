using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services.Security
{
    public interface IJwtService
    {
        /// <summary>
        /// Creates a JWT with the provided parameters
        /// </summary>
        /// <param name="sub">Subject - The authenticated user</param>
        /// <param name="iss">Issuer - The issuer of the JWT</param>
        /// <param name="aud">Audience - The intended recipient of the JWT</param>
        /// <param name="nbf">Not Before - The token cannot be used before this time</param>
        /// <param name="exp">Expiration - The token cannot be used after this time</param>
        /// <param name="other">Other - Any other items that should be added to the payload</param>
        /// <returns>The generated JWT</returns>
        string CreateToken(string sub, string iss, string aud, DateTime nbf, DateTime exp, Dictionary<string, string> other);

        /// <summary>
        /// Ensures the token has not been tampered with and returns the token's contents
        /// </summary>
        /// <param name="token">The JWT to be parsed</param>
        /// <returns>The data contained within the token</returns>
        /// <exception cref="InvalidCredentialsException">Throws an exception if the token has been tampered with</exception>
        Dictionary<string, string> ParseToken(string token);
    }
}
