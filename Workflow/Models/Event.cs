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

            if (Type == "new participant") return Creator.GetName() + " added you to " + Project.ProjectName;
            if (Type == "remove participant") return "You are no longer a participant in " + Project.ProjectName;

            if (Type == "new projectmanager") return "You have been assigned project manager for " + Project.ProjectName;
            if (Type == "remove projectmanager") return "You are no longer project manager for " + Project.ProjectName;

            if (Type == "assigned task") return "You have been assigned to " + Task.TaskName + " in " + Project.ProjectName;
            if (Type == "remove assigned task") return "You are no longer assigned to " + Task.TaskName + " in " + Project.ProjectName;
            if (Type == "finished task") return Creator.GetName() + " finished " + Task.TaskName + " in " + Project.ProjectName + ".";

            return "unknown notification messasge - update event.cs";
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
            if (Type == "assigned task") return Creator.GetName() + " assigned " + User.GetName() + " to the task " + Task.TaskName + ".";
            if (Type == "restore task") return Creator.GetName() + " restored deleted task " + Task.TaskName + ".";

            return "unknown event message - update events.cs";
        }

        public string GetIcon()
        {

            if (Type == "new participant") return "fas fa-user-plus";
            if (Type == "remove participant") return "fas fa-user-slash";

            if (Type == "new task") return "fas fa-clipboard-list";
            if (Type == "finished task") return "fas fa-clipboard-check";
            if (Type == "remove task") return "fas fa-sticky-note";
            if (Type == "assigned task") return "far fa-sticky-note";
            if (Type == "restore task") return "far fa-sticky-note";

            if (Type == "new tasklist") return "fas fa-folder-plus";
            if (Type == "remove tasklist") return "fa fa-folder-minus";


            return "unknown icon - update events.cs";
        }
    }
}
