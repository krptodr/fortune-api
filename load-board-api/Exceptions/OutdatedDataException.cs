using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace load_board_api.Exceptions
{
    public class OutdatedDataException : Exception
    {
        private const string DEFAULT_MSG = "Data is outdated!";

        public OutdatedDataException()
            : base(DEFAULT_MSG)
        {
        }

        public OutdatedDataException(string msg)
            : base(msg)
        {
        }

        public OutdatedDataException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}