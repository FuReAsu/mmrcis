// Models/PatientVital.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Collections.Generic; // Not needed here, collection moved

namespace mmrcis.Models
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

        // ADD THESE LINES: Link to the Patient model
        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!; // Navigation property to Patient

        // REMOVED: PatientCheckinOuts - This collection belongs on the Patient model, or PatientCheckinOut itself
        // if PatientVital can have multiple checkouts (less likely).
    }
}
