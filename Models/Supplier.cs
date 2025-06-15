
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace mmrcis.Models 
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
        public string? ContactPerson { get; set; } 

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; } 

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } 

        [StringLength(255)]
        public string? Address { get; set; } 

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true; 

        public DateTime RegisteredSince { get; set; } = DateTime.Now;
    }
}
