
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; 

namespace mmrcis.Models 
{
    
    public class ApplicationUser : IdentityUser
    {
        
        public int? PersonID { get; set; } 
        [ForeignKey("PersonID")]
        public Person? Person { get; set; } 

        
    }
}
