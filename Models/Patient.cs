using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public DateTime PatientSince { get; set; }

        public ICollection<IncomeBill> IncomeBills { get; set; } = new List<IncomeBill>();
    }
}
