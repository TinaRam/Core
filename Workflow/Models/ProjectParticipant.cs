using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("ProjectParticipant", Schema = "Workflow")]
    public class ProjectParticipant
    {
        [Column(TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [Column(TypeName = "int(11)")]
        public int UserId { get; set; }

        [ForeignKey("ProjectId")]
        [InverseProperty("ProjectParticipant")]
        public virtual Project Project { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ProjectParticipant")]
        public virtual User User { get; set; }
    }
}
