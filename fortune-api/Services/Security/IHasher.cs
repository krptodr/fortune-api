using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fortune_api.Services.Security
{
    public interface IHasher
    {
        /// <summary>
        /// Hashes the secret
        /// </summary>
        /// <param name="secret">String to be hashed</param>
        /// <returns>Hashed secret</returns>
        string Hash(string secret);

        /// <summary>
        /// Compares the secret with the correct hash
        /// </summary>
        /// <param name="secret">String to be hashed</param>
        /// <param name="correctHash">Correct hash</param>
        /// <returns>Boolean indicating whether the hashed secret is the same as the correctHash</returns>
        bool CompareWithHash(string secret, string correctHash);
    }
}
