// Models/Patient.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace mmrcis.Models // Use your actual project namespace here
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        // Foreign Key to the Person table
        [Required]
        [Display(Name = "Person")]
        public int PersonID { get; set; }
        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!; // Navigation property

        // You can add more patient-specific properties here as needed, e.g.:
        // [StringLength(50)]
        // public string? BloodGroup { get; set; }

        // [StringLength(500)]
        // public string? KnownAllergies { get; set; }

        // Status for patient (e.g., active, deceased, transferred) - useful for filtering
        [Display(Name = "Patient Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Default status

        // When the patient record was created (distinct from Person.RegisteredSince if Person was created earlier)
        public DateTime PatientSince { get; set; } = DateTime.Now;
    }
}
