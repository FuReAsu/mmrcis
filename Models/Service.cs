// Models/Service.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace mmrcis.Models // Use your actual project namespace here
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
