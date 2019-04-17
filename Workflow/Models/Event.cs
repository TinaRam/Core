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
        [ForeignKey("TaskListId")]
        [InverseProperty("Event")]
        public virtual TaskList TaskList { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EventUser")]
        public virtual User User { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<Notification> Notification { get; set; }

        public string GetMessage()
        {
            var list = new List<KeyValuePair<string, string>>();

            list.Add(new KeyValuePair<string, string>("new participant", Creator.GetName() + " added you to " + Project.ProjectName));
            list.Add(new KeyValuePair<string, string>("remove participant", "You are no longer a participant in " + Project.ProjectName));

            list.Add(new KeyValuePair<string, string>("new projectmanager", "You have been assigned project manager for " + Project.ProjectName));
            list.Add(new KeyValuePair<string, string>("remove projectmanager", "You are no longer project manager for " + Project.ProjectName));

            list.Add(new KeyValuePair<string, string>("project finished", Project.ProjectName + " is now finished."));


            //list.Add(new KeyValuePair<string, string>("new assigned task", "You have been assigned to the task " + Task.TaskName + " in + " + Project.ProjectName));
            //list.Add(new KeyValuePair<string, string>("remove assigned task", "You are no longer assigned to the task " + Task.TaskName + " in + " + Project.ProjectName));

            // .. and so on

            if (Type == "new participant") return Creator.GetName() + " added you to " + Project.ProjectName;
            if (Type == "remove participant") return "You are no longer a participant in " + Project.ProjectName;

            if (Type == "new projectmanager") return "You have been assigned project manager for " + Project.ProjectName;
            if (Type == "remove projectmanager") return "You are no longer project manager for " + Project.ProjectName;

            if (Type == "new assigned task") return "You have been assigned to " + Task.TaskName + " in " + Project.ProjectName;
            if (Type == "remove assigned task") return "You are no longer assigned to " + Task.TaskName + " in " + Project.ProjectName;
            if (Type == "finished task") return Creator.GetName() + " finished " + Task.TaskName + " in " + Project.ProjectName + ".";

            return "";
        }

        public string GetEvent()
        {
            if (Type == "new participant") return User.GetName() + " is now participating in the project.";
            if (Type == "remove participant") return User.GetName() + " is no longer participating in the project.";

            if (Type == "new tasklist") return Creator.GetName() + " added " + TaskList.ListName + " to the project.";
            if (Type == "remove tasklist") return Creator.GetName() + " removed " + TaskList.ListName + " from the project.";

            if (Type == "new task") return Creator.GetName() + " added " + Task.TaskName + " to " + Task.TaskList.ListName + ".";
            if (Type == "finished task") return Creator.GetName() + " finished " + Task.TaskName + ".";
            if (Type == "remove task") return Creator.GetName() + " deleted " + Task.TaskName + " from " + Task.TaskList.ListName + ".";

            return "";
        }

        public string GetIcon()
        {
            var list = new List<KeyValuePair<string, string>>();

            list.Add(new KeyValuePair<string, string>("new participant", "fas fa-user-plus"));
            list.Add(new KeyValuePair<string, string>("remove participant", "fas fa-user-slash"));

            list.Add(new KeyValuePair<string, string>("new task", "fas fa-sticky-note"));
            list.Add(new KeyValuePair<string, string>("finished task", "far fa-sticky-note"));
            list.Add(new KeyValuePair<string, string>("remove task", "far fa-sticky-note"));

            list.Add(new KeyValuePair<string, string>("new tasklist", "fas fa-clipboard"));
            list.Add(new KeyValuePair<string, string>("remove tasklist", "fa fa-clipboard"));

            return list.Find(l => l.Key == Type).Value.ToString();
        }
    }
}
