// Data/DbInitializer.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mmrcis.Models; // Ensure this matches your project's Models namespace
using System.Linq;
using System.Threading.Tasks;
using System;

namespace mmrcis.Data // Ensure this matches your project's Data namespace
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            CisDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure the database is created and all pending migrations are applied.
            // This is crucial if you're running the app for the first time or after new migrations.
            await context.Database.MigrateAsync();

            // --- 1. Seed Roles ---
            // Define the roles required in your application
            string[] roleNames = { "Admin", "Doctor", "Nurse", "Operator" };

            foreach (var roleName in roleNames)
            {
                // Check if the role already exists. If not, create it.
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. Seed an Admin User ---
            // Define the details for your initial admin user
            string adminEmail = "admin@cisapp.com";
            string adminPassword = "AdminPassword123!"; // **IMPORTANT: CHANGE THIS IN PRODUCTION!**
                                                       // Use a strong, unique password for production
                                                       // or use environment variables/user secrets.
            string adminRole = "Admin";

            // Try to find the admin user by email
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // If the admin user does not exist, create them
            if (adminUser == null)
            {
                // Create a Person record for the Admin.
                // This links the system login account to a real-world individual.
                var adminPerson = new Person
                {
                    FullName = "System Administrator",
                    PersonType = "Admin", // Matches the PersonType for Admin roles
                    Address = "123 Admin Lane, CIS HQ",
                    Qualification = "Certified Administrator",
                    RegisteredSince = DateTime.Now
                };
                context.Persons.Add(adminPerson);
                await context.SaveChangesAsync(); // Save Person to generate its ID before linking

                // Create the ApplicationUser (the actual login account)
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Set to true to allow immediate login without email confirmation
                    PersonID = adminPerson.ID // Link the ApplicationUser to the newly created Person
                };

                // Create the user with the specified password
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign the "Admin" role to the newly created user
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine($"Admin user '{adminEmail}' created and assigned '{adminRole}' role.");
                }
                else
                {
                    // Log any errors if user creation failed
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                // If admin user already exists, ensure they have the "Admin" role
                if (!await userManager.IsInRoleAsync(adminUser, adminRole))
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine($"Admin user '{adminEmail}' already exists, ensuring '{adminRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Admin user '{adminEmail}' already exists and has '{adminRole}' role.");
                }
            }

            // --- 3. Seed other initial data (e.g., Services, CostRates) ---
            // Example: Seed initial Services if none exist
            if (!context.Services.Any())
            {
                context.Services.AddRange(
                    new Service { ServiceName = "General Consultation", Description = "Initial patient consultation for diagnosis." },
                    new Service { ServiceName = "Specialist Referral", Description = "Referral to a specialist doctor." },
                    new Service { ServiceName = "Routine Blood Test", Description = "Basic blood work for general health check." },
                    new Service { ServiceName = "X-Ray Imaging", Description = "Radiography service for diagnostic imaging." },
                    new Service { ServiceName = "Minor Procedure", Description = "Small medical procedure, e.g., wound dressing." }
                );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Services seeded.");
            }

            // Example: Seed initial CostRates if none exist
            if (!context.CostRates.Any())
            {
                context.CostRates.AddRange(
                    new CostRate { CostCode = 101, CostAmount = 50.00m, CostType = "Medical", IorE = "INC", AccountCode = 1001 },
                    new CostRate { CostCode = 102, CostAmount = 25.00m, CostType = "Lab", IorE = "INC", AccountCode = 1002 },
                    new CostRate { CostCode = 201, CostAmount = 150.00m, CostType = "Equipment", IorE = "EXP", AccountCode = 2001 },
                    new CostRate { CostCode = 202, CostAmount = 75.00m, CostType = "Supplies", IorE = "EXP", AccountCode = 2002 }
                );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Cost Rates seeded.");
            }

            // Add more seeding logic here for other master data or default entries
            // (e.g., initial Patient records, if desired, or sample transactions for testing)

            Console.WriteLine("Database seeding complete.");
        }
    }
}
