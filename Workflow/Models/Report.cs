using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Models
{
    [Table("Report", Schema = "app2000g11")]
    public class Report
    {
        [Column(TypeName = "int(11)")]
        public int ReportId { get; set; }
        [Column(TypeName = "int(11)")]
        public int? ProjectId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Comment { get; set; }

        [ForeignKey("ProjectId")]
        [InverseProperty("Report")]
        public virtual Project Project { get; set; }
    }
}
