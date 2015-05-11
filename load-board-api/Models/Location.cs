using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace load_board_api.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DefaultValue(false)]
        public bool Deleted { get; set; }
        [Required, Timestamp, Column(TypeName = "datetime2")]
        public byte[] RowVersion { get; set; }
    }
}