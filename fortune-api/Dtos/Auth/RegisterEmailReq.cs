using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.Auth
{
    public class RegisterEmailReq
    {
        public bool NewUser { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserDto User { get; set; }
    }
}