using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("AssignedTask", Schema = "Workflow")]
    public partial class AssignedTask
    {
        [Column(TypeName = "int(11)")]
        public int AssignedTaskId { get; set; }
        [Column(TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [Column(TypeName = "int(11)")]
        public int UserId { get; set; }
        [Column(TypeName = "int(11)")]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("AssignedTask")]
        public virtual Ptask Task { get; set; }
    }
}
