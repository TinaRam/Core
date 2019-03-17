using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("User", Schema = "Workflow")]
    public class User
    {
        public User()
        {
            Employee = new HashSet<Employee>();
        }

        [Column("UserID", TypeName = "int(11)")]
        public int UserId { get; set; }
        [Required]
        [StringLength(55)]
        public string Username { get; set; }
        [Required]
        [StringLength(55)]
        public string Password { get; set; }
        [Column(TypeName = "tinyint(1)")]
        public byte IsAdmin { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
