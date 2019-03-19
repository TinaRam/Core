using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("TaskList", Schema = "Workflow")]
    public partial class TaskList
    {
        public TaskList()
        {
            Ptask = new HashSet<Ptask>();
        }

        [Column(TypeName = "int(11)")]
        public int TaskListId { get; set; }
        [Column(TypeName = "int(11)")]
        public int ProjectId { get; set; }
        [StringLength(55)]
        public string ListName { get; set; }

        [ForeignKey("ProjectId")]
        [InverseProperty("TaskList")]
        public virtual Project Project { get; set; }
        [InverseProperty("TaskList")]
        public virtual ICollection<Ptask> Ptask { get; set; }
    }
}
