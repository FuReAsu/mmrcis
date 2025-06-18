using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class IncomeBillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "CostRate")]
        public int CostRateID { get; set; }

        [ForeignKey("CostRateID")]
        public CostRate CostRate { get; set; } = null!;
            
        [Required]
        [Display(Name = "IncomeBill")]
        public int IncomeBillID { get; set; }

        [ForeignKey("IncomeBillID")]
        public IncomeBill IncomeBill { get; set; } = null!;

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "UnitCost")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }

        [NotMapped]
        [Display(Name = "Total")]
        public decimal Total => Quantity * UnitCost;
    }
}
