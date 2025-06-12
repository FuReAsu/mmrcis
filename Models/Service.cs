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

        // Add any other relevant service details, e.g., description, default cost
        [StringLength(255)]
        public string Description { get; set; }

        public DateTime RegisteredSince { get; set; } = DateTime.Now;
    }
}
