using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("ProjectParticipant", Schema = "Workflow")]
    public class ProjectParticipant
    {
        [Column("ProjectID", TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [Column("EmployeeID", TypeName = "int(11)")]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("ProjectParticipant")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("ProjectParticipant")]
        public virtual Project Project { get; set; }
    }
}
