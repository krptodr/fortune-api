using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fortune_api.Models.Auth
{
    public class UserProfile
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual List<Permission> Permissions { get; set; }
        [Required]
        public bool Deleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}