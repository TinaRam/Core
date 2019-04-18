using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Project", Schema = "app2000g11")]
    public class Project
    {
        public Project()
        {
            Event = new HashSet<Event>();
            ProjectParticipant = new HashSet<ProjectParticipant>();
            Ptask = new HashSet<Ptask>();
            Report = new HashSet<Report>();
            TaskList = new HashSet<TaskList>();
        }

        [Column(TypeName = "int(11)")]
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
        [Column(TypeName = "tinyint(1)")]
        public byte? MarkedAsFinished { get; set; }

        [ForeignKey("ProjectManager")]
        [InverseProperty("Project")]
        public virtual User ProjectManagerNavigation { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<Event> Event { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<ProjectParticipant> ProjectParticipant { get; set; }
        [InverseProperty("TaskProject")]
        public virtual ICollection<Ptask> Ptask { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<Report> Report { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<TaskList> TaskList { get; set; }

        public bool isManager(int userId)
        {
            return ProjectManager == userId;
        }

        public bool isFinished()
        {
            return MarkedAsFinished != 0;
        }

        public bool isArchived()
        {
            return CompletionDate != null;
        }

        public int getTotalTasks()
        {
            int c = 0;
            foreach (Ptask t in Ptask)
            {
                if (t.Deleted == 0)
                {
                    c++;
                }
            }
            return c;
        }

        public int getFinishedTasks()
        {
            int c = 0;
            foreach (Ptask t in Ptask)
            {
                if (t.Deleted == 0 && t.CompletionDate != null)
                {
                    c++;
                }
            }
            return c;
        }
    }
}
