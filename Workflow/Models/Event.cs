using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Event", Schema = "app2000g11")]
    public class Event
    {
        public Event()
        {
            Notification = new HashSet<Notification>();
        }

        [Column(TypeName = "int(11)")]
        public int EventId { get; set; }
        public DateTime? EventDate { get; set; }
        [Column(TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [Column(TypeName = "int(11)")]
        public int CreatorId { get; set; }
        [Required]
        [StringLength(55)]
        public string Type { get; set; }
        [Column(TypeName = "int(11)")]
        public int? UserId { get; set; }
        [Column(TypeName = "int(11)")]
        public int? TaskId { get; set; }
        [Column(TypeName = "int(11)")]
        public int? TaskListId { get; set; }

        [ForeignKey("CreatorId")]
        [InverseProperty("EventCreator")]
        public virtual User Creator { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("Event")]
        public virtual Project Project { get; set; }
        [ForeignKey("TaskId")]
        [InverseProperty("Event")]
        public virtual Ptask Task { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EventUser")]
        public virtual User User { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<Notification> Notification { get; set; }

        public string GetMessage()
        {
            var list = new List<KeyValuePair<string, string>>();

            list.Add(new KeyValuePair<string, string>("new participant", Creator.GetName() + " added you to " + Project.ProjectName));
            list.Add(new KeyValuePair<string, string>("removed participant", "You are no longer a participant in " + Project.ProjectName));

            list.Add(new KeyValuePair<string, string>("new projectmanager", "You have been assigned project manager for " + Project.ProjectName));
            list.Add(new KeyValuePair<string, string>("remove projectmanager", "You are no longer project manager for " + Project.ProjectName));

            list.Add(new KeyValuePair<string, string>("project finished", Project.ProjectName + " is now finished."));
            

            //list.Add(new KeyValuePair<string, string>("new assigned task", "You have been assigned to the task " + Task.TaskName + " in + " + Project.ProjectName));
            //list.Add(new KeyValuePair<string, string>("remove assigned task", "You are no longer assigned to the task " + Task.TaskName + " in + " + Project.ProjectName));
            
            // .. and so on

            

            return list.Find(l => l.Key == Type).Value.ToString();
        }
    }
}
