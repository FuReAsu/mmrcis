// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema; // For Foreign Key attribute

namespace mmrcis.Models // Use your actual project namespace here
{
    // Inherit from IdentityUser to get all standard Identity properties (username, password hash, email, etc.)
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties specific to your user, e.g., a link to the Person table
        public int? PersonID { get; set; } // Nullable, as a user might be created before a full Person record
        [ForeignKey("PersonID")]
        public Person? Person { get; set; } // Navigation property

        // You could add other properties here if needed, but linking to Person is generally better
    }
}
