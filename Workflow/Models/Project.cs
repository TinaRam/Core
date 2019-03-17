using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Project", Schema = "Workflow")]
    public class Project
    {
        public Project()
        {
            ProjectParticipant = new HashSet<ProjectParticipant>();
            PTask = new HashSet<PTask>();
        }

        [Column("ProjectID", TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(255)]
        public string ProjectName { get; set; }
        [StringLength(255)]
        public string ProjectDescription { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProjectStart { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProjectDeadline { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CompletionDate { get; set; }
        [Column(TypeName = "int(11)")]
        public int? ProjectManager { get; set; }

        [ForeignKey("ProjectManager")]
        [InverseProperty("Project")]
        public virtual Employee ProjectManagerNavigation { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<ProjectParticipant> ProjectParticipant { get; set; }
        [InverseProperty("TaskProject")]
        public virtual ICollection<PTask> PTask { get; set; }
    }
}
