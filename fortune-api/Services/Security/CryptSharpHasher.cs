using CryptSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace fortune_api.Services.Security
{
    public class CryptSharpHasher : IHasher
    {
        private static readonly int SALT_LENGTH = Convert.ToInt32(ConfigurationManager.AppSettings["SALT_LENGTH"]);

        private string GetRandomSalt()
        {
            return Crypter.Blowfish.GenerateSalt(SALT_LENGTH);
        }

        public string Hash(string secret)
        {
            return Crypter.Blowfish.Crypt(secret, GetRandomSalt());
        }

        public bool CompareWithHash(string secret, string correctHash)
        {
            return Crypter.CheckPassword(secret, correctHash);
        }
    }
}