using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("User", Schema = "app2000g11")]
    public class User
    {
        public User()
        {
            EmployeeLeave = new HashSet<EmployeeLeave>();
            EventCreator = new HashSet<Event>();
            EventUser = new HashSet<Event>();
            Notification = new HashSet<Notification>();
            Project = new HashSet<Project>();
            ProjectParticipant = new HashSet<ProjectParticipant>();
        }

        [Column(TypeName = "int(11)")]
        public int UserId { get; set; }
        [Required]
        [StringLength(55)]
        public string Username { get; set; }
        [Required]
        [StringLength(55)]
        public string Password { get; set; }
        [Required]
        [StringLength(55)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(55)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "int(11)")]
        public int? Role { get; set; }
        public string About { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<EmployeeLeave> EmployeeLeave { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Event> EventCreator { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Event> EventUser { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Notification> Notification { get; set; }
        [InverseProperty("ProjectManagerNavigation")]
        public virtual ICollection<Project> Project { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ProjectParticipant> ProjectParticipant { get; set; }

        public string GetName()
        {
            return FirstName + " " + LastName;
        }
    }
}