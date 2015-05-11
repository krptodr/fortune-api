using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        private const string DEFAULT_MSG = "Resource already exists!";

        public AlreadyExistsException()
            : base(DEFAULT_MSG)
        {
        }

        public AlreadyExistsException(string msg)
            : base(msg)
        {
        }

        public AlreadyExistsException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}