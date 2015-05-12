using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.Auth
{
    public class LoginRes
    {
        public string JWT { get; set; }
        public UserDto User { get; set; }
    }
}