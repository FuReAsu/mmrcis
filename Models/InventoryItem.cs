
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace mmrcis.Models 
{
    public class InventoryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Unit of Measure")]
        public string? UnitOfMeasure { get; set; } 

        [Column(TypeName = "decimal(18,2)")] 
        [Display(Name = "Current Stock")]
        public decimal CurrentStock { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Min Stock Level")]
        public decimal MinStockLevel { get; set; } = 0; 

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Purchase Price (Per Unit)")]
        public decimal PurchasePrice { get; set; }

        [StringLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        
        [Display(Name = "Supplier")]
        public int? SupplierID { get; set; } 
        [ForeignKey("SupplierID")]
        public Supplier? Supplier { get; set; } 

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true; 

        public DateTime RegisteredSince { get; set; } = DateTime.Now;
    }
}
