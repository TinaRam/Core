﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("PTask", Schema = "Workflow")]
    public class PTask
    {
        public PTask()
        {
            AssignedTask = new HashSet<AssignedTask>();
        }

        [Key]
        [Column("TaskID", TypeName = "int(11)")]
        public int TaskId { get; set; }
        [Required]
        [StringLength(255)]
        public string TaskName { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "enum('low','normal','high')")]
        public string Priority { get; set; }
        public DateTime? TaskCreationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TaskDeadline { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CompletionDate { get; set; }
        [Column("TaskProjectID", TypeName = "int(11)")]
        public int? TaskProjectId { get; set; }

        [ForeignKey("TaskProjectId")]
        [InverseProperty("PTask")]
        public virtual Project TaskProject { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<AssignedTask> AssignedTask { get; set; }
    }
}
