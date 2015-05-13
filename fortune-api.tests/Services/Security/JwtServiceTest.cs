using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using fortune_api.Services.Security;
using fortune_api.Exceptions;

namespace fortune_api.Tests.Services.Security
{
    [TestClass]
    public class JwtServiceTest
    {
        IJwtService Service;

        public JwtServiceTest()
        {
            this.Service = new JwtService();
        }

        [TestMethod]
        public void ValidToken()
        {
            string sub = "1",
                   iss = JwtService.DEFAULT_ISSUER,
                   aud = JwtService.DEFAULT_AUDIENCE;
            DateTime nbf = DateTime.Now,
                     exp = nbf.AddHours(2);
            string token = this.Service.CreateToken(sub, iss, aud, nbf, exp, new Dictionary<string, string>());
            Dictionary<string, string> contents = this.Service.ParseToken(token);
            Assert.AreEqual(sub, contents["sub"]);
            Assert.AreEqual(iss, contents["iss"]);
            Assert.AreEqual(aud, contents["aud"]);
            Assert.AreEqual(nbf.ToString(), contents["nbf"]);
            Assert.AreEqual(exp.ToString(), contents["exp"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void TamperedToken()
        {
            string sub = "1",
                   iss = JwtService.DEFAULT_ISSUER,
                   aud = JwtService.DEFAULT_AUDIENCE;
            DateTime nbf = DateTime.Now,
                     exp = nbf.AddHours(2);
            string token = this.Service.CreateToken(sub, iss, aud, nbf, exp, new Dictionary<string, string>());
            token = token.Substring(1);
            Dictionary<string, string> contents = this.Service.ParseToken(token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void ExpiredToken()
        {
            string sub = "1",
                   iss = JwtService.DEFAULT_ISSUER,
                   aud = JwtService.DEFAULT_AUDIENCE;
            DateTime nbf = DateTime.Now.AddHours(-3),
                     exp = nbf.AddHours(2);
            string token = this.Service.CreateToken(sub, iss, aud, nbf, exp, new Dictionary<string, string>());
            token = token.Substring(1);
            Dictionary<string, string> contents = this.Service.ParseToken(token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void FutureToken()
        {
            string sub = "1",
                   iss = JwtService.DEFAULT_ISSUER,
                   aud = JwtService.DEFAULT_AUDIENCE;
            DateTime nbf = DateTime.Now.AddHours(1),
                     exp = nbf.AddHours(2);
            string token = this.Service.CreateToken(sub, iss, aud, nbf, exp, new Dictionary<string, string>());
            token = token.Substring(1);
            Dictionary<string, string> contents = this.Service.ParseToken(token);
        }
    }
}