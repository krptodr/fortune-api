using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Exceptions
{
    public class DoesNotExistException : Exception
    {
        private const string DEFAULT_MSG = "Resource does not exist!";

        public DoesNotExistException()
            : base(DEFAULT_MSG)
        {
        }

        public DoesNotExistException(string msg)
            : base(msg)
        {
        }

        public DoesNotExistException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}