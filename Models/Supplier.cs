// Models/Supplier.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace mmrcis.Models // Use your actual project namespace here
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; } // Nullable

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; } // Nullable

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } // Nullable

        [StringLength(255)]
        public string? Address { get; set; } // Nullable

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true; // Default to active

        public DateTime RegisteredSince { get; set; } = DateTime.Now;
    }
}
