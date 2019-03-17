using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("AssignedTask", Schema = "Workflow")]
    public class AssignedTask
    {
        [Column("EmployeeID", TypeName = "int(11)")]
        public int EmployeeId { get; set; }
        [Column("TaskID", TypeName = "int(11)")]
        public int TaskId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("AssignedTask")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("AssignedTask")]
        public virtual PTask Task { get; set; }
    }
}
