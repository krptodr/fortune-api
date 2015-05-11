using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace fortune_api.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DefaultValue(false)]
        public bool Deleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}