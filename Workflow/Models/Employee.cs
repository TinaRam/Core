using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Employee", Schema = "Workflow")]
    public class Employee
    {
        public Employee()
        {
            AssignedTask = new HashSet<AssignedTask>();
            Project = new HashSet<Project>();
            ProjectParticipant = new HashSet<ProjectParticipant>();
        }

        [Column("EmployeeID", TypeName = "int(11)")]
        public int EmployeeId { get; set; }
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
        [Column("UserID", TypeName = "int(11)")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Employee")]
        public virtual User User { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<AssignedTask> AssignedTask { get; set; }
        [InverseProperty("ProjectManagerNavigation")]
        public virtual ICollection<Project> Project { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<ProjectParticipant> ProjectParticipant { get; set; }
    }
}
