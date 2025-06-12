// Models/PatientVital.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class PatientVital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? BP { get; set; } // Blood Pressure, nullable if not always recorded

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Oximeter { get; set; } // Nullable

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; } // Nullable

        [Column(TypeName = "decimal(5,2)")]
        public decimal? RespRate { get; set; } // Respiratory Rate, nullable

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; } // Nullable

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int? OperatorID { get; set; } // Nullable based on your script
        [ForeignKey("OperatorID")]
        public Person? Operator { get; set; } // Navigation property to Person (as Operator)

        public ICollection<PatientCheckinOut>? PatientCheckinOuts { get; set; }
    }
}
