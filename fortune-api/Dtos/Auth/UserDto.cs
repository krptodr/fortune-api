using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fortune_api.Dtos.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PermissionDto> Permissions { get; set; }
        public bool Deleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}