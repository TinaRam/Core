using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("EmployeeLeave", Schema = "Workflow")]
    public partial class EmployeeLeave
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int LeaveId { get; set; }
        [Column(TypeName = "int(11)")]
        public int UserId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("EmployeeLeave")]
        public virtual User User { get; set; }
    }
}
