using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.Auth
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}