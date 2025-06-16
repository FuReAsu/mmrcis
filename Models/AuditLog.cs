using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models
{
    public class AuditLog
    {
        public int ID { get; set; }

        [Display(Name = "Person")]
        public int PersonID { get; set; }
        [ForeignKey("PersonID")]
        public Person Person { get; set; } = null!;

        public string Action { get; set; }
        
        public string ControllerName { get; set; }

        public string Parameters { get; set; }

        public DateTime Timestamp { get; set; }

        public string IpAddress { get; set; }

        public string UserAgent { get; set; }
        
    }
}
