using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class IncomeBill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!;

        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }

        [Required]
        [Display(Name = "Operator")]
        public int PersonID { get; set; }

        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!;

        [Display(Name = "Patient Check In Out Record")]
        public int? PatientCheckInOutID { get; set; }

        [ForeignKey("PatientCheckInOutID")]
        public PatientCheckInOut? PatientCheckInOut { get; set; }
       
        [Display(Name = "BillTotal")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BillTotal { get; set; }

        public ICollection<IncomeBillItem> IncomeBillItems { get; set; } = new List<IncomeBillItem>();
    }
}
