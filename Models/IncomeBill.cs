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

        public ICollection<IncomeBillItem> IncomeBillItems { get; set; } = new List<IncomeBillItem>();
    }
}
