using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Notification", Schema = "app2000g11")]
    public class Notification
    {
        [Key]
        [Column("NId", TypeName = "int(11)")]
        public int Nid { get; set; }
        [Column(TypeName = "int(11)")]
        public int EventId { get; set; }
        [Column(TypeName = "tinyint(1)")]
        public byte? Viewed { get; set; }
        [Column(TypeName = "tinyint(1)")]
        public byte? Email { get; set; }
        [Column(TypeName = "tinyint(1)")]
        public byte? InApp { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("Notification")]
        public virtual Event Event { get; set; }

        public string getMessage()
        {
            string message = "";
            

            return message;
        }
    }
}
