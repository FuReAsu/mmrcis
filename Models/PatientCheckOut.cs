// Models/PatientCheckinOut.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    public class PatientCheckinOut
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Person Patient { get; set; } // Navigation property to Person (as Patient)

        public int? DoctorID { get; set; } // Nullable based on your script
        [ForeignKey("DoctorID")]
        public Person? Doctor { get; set; } // Navigation property to Person (as Doctor)

        public int? PatientVitalID { get; set; } // Nullable based on your script
        [ForeignKey("PatientVitalID")]
        public PatientVital? PatientVital { get; set; } // Navigation property

        public DateTime? CIN_TIME { get; set; } // Check-in time
        public DateTime? COUT_TIME { get; set; } // Check-out time

        public bool? IsServed { get; set; } // Nullable BIT
        public bool IsBilled { get; set; } = false; // Corrected default to false (0)
    }
}
