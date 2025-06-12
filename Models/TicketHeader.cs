// Models/TicketHeader.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models // IMPORTANT: Replace with your actual project namespace
{
    // Assuming this table will have a single record or records managed by a key
    // Added a Key property for EF Core mapping, assuming it's configuration data
    public class TicketHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate ID if it's a new table with a key
        public int ID { get; set; } // Added an ID for EF Core to map

        [StringLength(255)]
        public string ClinicName { get; set; }

        [StringLength(255)]
        public string HeaderLine1 { get; set; }

        [StringLength(255)]
        public string HeaderLine2 { get; set; }

        [StringLength(255)]
        public string HeaderLine3 { get; set; }

        [StringLength(255)]
        public string LabOPD { get; set; }
    }
}
