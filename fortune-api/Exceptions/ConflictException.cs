using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Exceptions
{
    public class ConflictException : Exception
    {
        private const string DEFAULT_MSG = "A data conflict occurred!";

        public ConflictException()
            : base(DEFAULT_MSG)
        {
        }

        public ConflictException(string msg)
            : base(msg)
        {
        }

        public ConflictException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}