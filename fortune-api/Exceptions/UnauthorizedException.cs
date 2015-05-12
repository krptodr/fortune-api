using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Exceptions
{
    public class UnauthorizedException : Exception
    {
        private const string DEFAULT_MSG = "Unauthorized!";

        public UnauthorizedException()
            : base(DEFAULT_MSG)
        {
        }

        public UnauthorizedException(string msg)
            : base(msg)
        {
        }

        public UnauthorizedException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}