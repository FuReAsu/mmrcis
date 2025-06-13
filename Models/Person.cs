// Models/Person.cs
using System;
using System.Collections.Generic; // Important: Add this for ICollection
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Ensure this matches your actual project namespace
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure ID is auto-incremented
        public int ID { get; set; }

        [Required]
        [StringLength(16)] // e.g., "Admin", "Doctor", "Nurse", "Operator", "Patient"
        public string PersonType { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [StringLength(255)]
        public string? Qualification { get; set; } // For staff (Doctor, Nurse, Operator)

        [StringLength(255)]
        public string? Specialization { get; set; } // For Doctors

        [StringLength(255)]
        public string Address { get; set; }

        public DateTime RegisteredSince { get; set; } = DateTime.Now; // Default value set in model

        // --- Patient-specific fields ---
        public DateTime? DOB { get; set; } // Date of Birth
        public int? Age { get; set; }     // Age (can be calculated, or stored if needed)
        [StringLength(16)]
        public string? Sex { get; set; }
        [StringLength(8)]
        public string? BloodGroup { get; set; }
        [StringLength(255)]
        public string? Allergy { get; set; } // Corrected typo from Alergy
        [StringLength(255)]
        public string? FatherName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; } // Added this nullable property based on a previous example

        // --- Navigation properties for relationships where Person plays various roles ---
        // These are the ICollection properties that were missing and causing the errors.

        // For Person as a Patient
        public ICollection<IncomeBill>? IncomeBillsAsPatient { get; set; }
        public ICollection<ExpenseBill>? ExpenseBillsAsPatient { get; set; }
        public ICollection<PatientDocument>? PatientDocuments { get; set; }
        public ICollection<PatientCheckinOut>? PatientCheckinOutsAsPatient { get; set; }


        // For Person as an Operator
        public ICollection<IncomeBill>? IncomeBillsAsOperator { get; set; }
        public ICollection<ExpenseBill>? ExpenseBillsAsOperator { get; set; }
        public ICollection<PatientVital>? PatientVitalsAsOperator { get; set; }
        public ICollection<PostingTransaction>? PostingTransactionsAsOperator { get; set; }


        // For Person as a Doctor
        public ICollection<PatientLabRecord>? PatientLabRecordsAsDoctor { get; set; }
        public ICollection<PatientCheckinOut>? PatientCheckinOutsAsDoctor { get; set; }


        // For Person as a Checked Person in Posting Transactions
        public ICollection<PostingTransaction>? PostingTransactionsAsCheckedPerson { get; set; }

        // Navigation property for linking to an ApplicationUser (if this Person is a system user)
        // This is the 'one' side of the ApplicationUser's PersonID relationship
        // public ApplicationUser? ApplicationUser { get; set; } // Often managed from the ApplicationUser side
    }
}
