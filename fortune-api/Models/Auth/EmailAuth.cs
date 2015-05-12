using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Models.Auth
{
    public class EmailAuth
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}