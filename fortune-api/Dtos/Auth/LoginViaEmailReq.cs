using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.Auth
{
    public class LoginViaEmailReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}