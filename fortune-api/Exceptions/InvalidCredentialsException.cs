using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        private const string DEFAULT_MSG = "Invalid credentials!";

        public InvalidCredentialsException()
            : base(DEFAULT_MSG)
        {
        }

        public InvalidCredentialsException(string msg)
            : base(msg)
        {
        }

        public InvalidCredentialsException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}