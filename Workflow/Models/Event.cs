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
    }
}
