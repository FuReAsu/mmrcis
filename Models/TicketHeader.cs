
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmrcis.Models 
{
    
    
    public class TicketHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int ID { get; set; } 

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
