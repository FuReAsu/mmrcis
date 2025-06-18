using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class CostRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "CostType")]
        [StringLength(100)]
        public string CostType { get; set; } = null!;
        
        [Required]
        [Display(Name = "UnitCost")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; } = "";
       
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Registered Since")]
        public DateTime RegisteredSince { get; set; }
        
        public ICollection<IncomeBillItem> IncomeBillItems { get; set; } = new List<IncomeBillItem>();
    }
}
