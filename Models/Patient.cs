
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic; 

namespace mmrcis.Models 
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        
        [Required]
        [Display(Name = "Person")]
        public int PersonID { get; set; }
        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!; 

        
        [Display(Name = "Patient Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; 

        
        public DateTime PatientSince { get; set; } = DateTime.Now;

        
        
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<IncomeBill>? IncomeBills { get; set; } 
        public ICollection<ExpenseBill>? ExpenseBills { get; set; } 
        public ICollection<PatientDocument>? PatientDocuments { get; set; } 
        public ICollection<PatientLabRecord>? PatientLabRecords { get; set; } 
        public ICollection<PatientVital>? PatientVitals { get; set; } 
        public ICollection<PatientCheckinOut>? PatientCheckinOuts { get; set; } 
    }
}
