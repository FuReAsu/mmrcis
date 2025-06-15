// Models/Patient.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic; // Add this using directive for ICollection

namespace mmrcis.Models // Use your actual project namespace here
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        // Foreign Key to the Person table (This is the one-to-one link)
        [Required]
        [Display(Name = "Person")]
        public int PersonID { get; set; }
        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!; // Navigation property back to Person

        // Status for patient (e.g., active, deceased, transferred) - useful for filtering
        [Display(Name = "Patient Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Default status

        // When the patient record was created (distinct from Person.RegisteredSince if Person was created earlier)
        public DateTime PatientSince { get; set; } = DateTime.Now;

        // --- Collections related to THIS Patient ---
        // These collections represent entities that belong to or are associated with this specific patient record.
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<IncomeBill>? IncomeBills { get; set; } // Income bills FOR this patient
        public ICollection<ExpenseBill>? ExpenseBills { get; set; } // Expense bills FOR this patient
        public ICollection<PatientDocument>? PatientDocuments { get; set; } // Documents FOR this patient
        public ICollection<PatientLabRecord>? PatientLabRecords { get; set; } // Lab records FOR this patient
        public ICollection<PatientVital>? PatientVitals { get; set; } // Vitals FOR this patient
        public ICollection<PatientCheckinOut>? PatientCheckinOuts { get; set; } // Check-in/out records FOR this patient
    }
}
