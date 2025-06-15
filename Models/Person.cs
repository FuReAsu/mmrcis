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
        public string PersonType { get; set; } = null!; // Added default to avoid null warnings

        [Required]
        [StringLength(255)]
        public string FullName { get; set; } = null!; // Added default

        [StringLength(255)]
        public string? Qualification { get; set; } // For staff (Doctor, Nurse, Operator)

        [StringLength(255)]
        public string? Specialization { get; set; } // For Doctors

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = null!; // Added default

        public DateTime RegisteredSince { get; set; } = DateTime.Now; // Default value set in model

        // --- Patient-specific fields on Person (if they are general demographics) ---
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; } // Date of Birth
        public int? Age { get; set; }      // Age (can be calculated, or stored if needed)
        [StringLength(16)]
        public string? Sex { get; set; }
        [StringLength(8)]
        public string? BloodGroup { get; set; }
        [StringLength(255)]
        public string? Allergy { get; set; }
        [StringLength(255)]
        public string? FatherName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        [StringLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; }

        // --- Navigation properties for relationships where Person plays various roles ---

        // For Person as a Patient (One-to-one relationship with the Patient model)
        // This is the link FROM Person TO its corresponding Patient profile.
        public Patient? PatientProfile { get; set; }

        // For Person as an Operator (who creates income bills, receives payments, etc.)
        public ICollection<IncomeBill>? IncomeBillsAsCreatedByOperator { get; set; }
        public ICollection<Payment>? PaymentsAsReceivedByOperator { get; set; }
        public ICollection<ExpenseBill>? ExpenseBillsAsOperator { get; set; }
        public ICollection<PatientVital>? PatientVitalsAsOperator { get; set; } // Vitals taken/recorded by this operator
        public ICollection<PostingTransaction>? PostingTransactionsAsOperator { get; set; }

        // For Person as a Doctor
        public ICollection<PatientLabRecord>? PatientLabRecordsAsDoctor { get; set; } // Lab records requested/reviewed by this doctor
        public ICollection<PatientCheckinOut>? PatientCheckinOutsAsDoctor { get; set; } // Check-ins where this person is the doctor

        // For Person as a Checked Person in Posting Transactions
        public ICollection<PostingTransaction>? PostingTransactionsAsCheckedPerson { get; set; }

        // Navigation property for linking to an ApplicationUser (if this Person is a system user)
        // This is the 'one' side of the ApplicationUser's PersonID relationship
        // public ApplicationUser? ApplicationUser { get; set; } // Often managed from the ApplicationUser side
    }
}
