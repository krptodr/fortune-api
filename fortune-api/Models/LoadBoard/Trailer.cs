using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace fortune_api.LoadBoard.Models
{
    public class Trailer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public Guid LocationId { get; set; }
        [Required, DefaultValue(false)]
        public bool Deleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}