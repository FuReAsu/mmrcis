
using System;
using System.ComponentModel.DataAnnotations;

namespace mmrcis.Models 
{
    public class Service
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(128)]
        public string ServiceName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime RegisteredSince { get; set; } = DateTime.Now;
    }
}
