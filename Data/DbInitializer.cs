// Data/DbInitializer.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Needed for context.Database.MigrateAsync()
using mmrcis.Models; // Ensure this matches your project's Models namespace
using System.Linq;
using System.Threading.Tasks;
using System; // Needed for DateTime.Now

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

            // --- 2. Seed an Admin User and associated Person ---
            string adminEmail = "admin@test.local";
            string adminPassword = "P@ssw0rd"; // **IMPORTANT: CHANGE THIS IN PRODUCTION!**
            string adminRole = "Admin";

            // Try to find the admin user by email
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // If the admin user does not exist, create them
            if (adminUser == null)
            {
                // 1. Create a Person record for the Admin.
                // This links the system login account to a real-world individual.
                var adminPerson = new Person
                {
                    FullName = "System Administrator",
                    PersonType = "Admin", // Matches the PersonType for Admin roles
                    Address = "123 Admin Lane, CIS HQ",
                    Qualification = "Certified Administrator",
                    RegisteredSince = DateTime.Now,
                    BloodGroup = "B",
                    Sex = "Male", // Corrected typo: "male" -> "Male" for consistency
                    PhoneNumber = "111-222-3333", // Added contact number for completeness
                    Email = adminEmail // Associate email with the person record
                };
                context.Persons.Add(adminPerson);
                await context.SaveChangesAsync(); // <--- CRITICAL: Save Person to get its ID before linking!

                // 2. Create the ApplicationUser (the actual login account)
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Set to true to allow immediate login without email confirmation
                    PersonID = adminPerson.ID // <--- CRITICAL: Link the ApplicationUser to the newly created Person ID
                };

                // 3. Create the user with the specified password
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // 4. Assign the "Admin" role to the newly created user
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine($"Admin user '{adminEmail}' created and assigned '{adminRole}' role.");
                }
                else
                {
                    // Log any errors if user creation failed
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    // Optional: If user creation fails, consider removing the person record if it was added.
                    // Or ensure the process is robust enough to handle partial creations.
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

                // Optional: For existing users, ensure their PersonID is set if it's null
                // This is useful for dev environments where you might add PersonId later.
                if (adminUser.PersonID == null)
                {
                    var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == adminEmail && p.PersonType == "Admin");
                    if (existingPerson != null)
                    {
                        adminUser.PersonID = existingPerson.ID;
                        await userManager.UpdateAsync(adminUser);
                        Console.WriteLine($"Admin user '{adminEmail}' linked to existing Person ID: {existingPerson.ID}.");
                    }
                }
            }

            // --- 3. Seed an Operator User and associated Person ---
            string operatorEmail = "operator@test.local";
            string operatorPassword = "P@ssw0rd";
            string operatorRole = "Operator";

            var operatorUser = await userManager.FindByEmailAsync(operatorEmail);

            if (operatorUser == null)
            {
                // 1. Create a Person record for the Operator
                var operatorPerson = new Person
                {
                    FullName = "Clinic Operator",
                    PersonType = "Operator", // Matches the PersonType for Operator roles
                    Address = "456 Operator Street, Clinic Office",
                    Qualification = "Clinic Staff",
                    RegisteredSince = DateTime.Now,
                    BloodGroup = "A",
                    Sex = "Female", // Corrected typo: "femaile" -> "Female"
                    PhoneNumber = "444-555-6666", // Added contact number
                    Email = operatorEmail // Associate email with the person record
                };
                context.Persons.Add(operatorPerson);
                await context.SaveChangesAsync(); // <--- CRITICAL: Save Person to get its ID before linking!

                // 2. Create the ApplicationUser for Operator
                operatorUser = new ApplicationUser
                {
                    UserName = operatorEmail,
                    Email = operatorEmail,
                    EmailConfirmed = true,
                    PersonID = operatorPerson.ID // <--- CRITICAL: Link ApplicationUser to the new Person ID
                };

                // 3. Create the user with the specified password
                var result = await userManager.CreateAsync(operatorUser, operatorPassword);
                if (result.Succeeded)
                {
                    // 4. Assign the "Operator" role
                    await userManager.AddToRoleAsync(operatorUser, operatorRole);
                    Console.WriteLine($"Operator user '{operatorEmail}' created and assigned '{operatorRole}' role.");
                }
                else
                {
                    Console.WriteLine($"Error creating operator user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                // If operator user already exists, ensure they have the "Operator" role
                if (!await userManager.IsInRoleAsync(operatorUser, operatorRole))
                {
                    await userManager.AddToRoleAsync(operatorUser, operatorRole);
                    Console.WriteLine($"Operator user '{operatorEmail}' already exists, ensuring '{operatorRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Operator user '{operatorEmail}' already exists and has '{operatorRole}' role.");
                }

                // Optional: For existing users, ensure their PersonID is set if it's null
                if (operatorUser.PersonID == null)
                {
                    var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == operatorEmail && p.PersonType == "Operator");
                    if (existingPerson != null)
                    {
                        operatorUser.PersonID = existingPerson.ID;
                        await userManager.UpdateAsync(operatorUser);
                        Console.WriteLine($"Operator user '{operatorEmail}' linked to existing Person ID: {existingPerson.ID}.");
                    }
                }
            }

            // --- 4. Seed other initial data (e.g., Services, CostRates, Suppliers, InventoryItems) ---
            // Seed initial Services if none exist
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

            // Seed initial CostRates if none exist
            if (!context.CostRates.Any())
            {
                context.CostRates.AddRange(
                    new CostRate { CostCode = 101, CostAmount = 50.00m, CostType = "Medical", IorE = "INC", AccountCode = 1001, RegisteredSince = DateTime.Now },
                    new CostRate { CostCode = 102, CostAmount = 25.00m, CostType = "Lab", IorE = "INC", AccountCode = 1002, RegisteredSince = DateTime.Now },
                    new CostRate { CostCode = 201, CostAmount = 150.00m, CostType = "Equipment", IorE = "EXP", AccountCode = 2001, RegisteredSince = DateTime.Now },
                    new CostRate { CostCode = 202, CostAmount = 75.00m, CostType = "Supplies", IorE = "EXP", AccountCode = 2002, RegisteredSince = DateTime.Now }
                );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Cost Rates seeded.");
            }
            
            // Seed initial Suppliers if none exist
            if (!context.Suppliers.Any())
            {
                context.Suppliers.AddRange(
                    new Supplier { Name = "Global Pharma Inc.", ContactPerson = "Dr. Emily White", Email = "emily@globalpharma.com", Phone = "555-1001", Address = "123 Pharma Blvd", IsActive = true, RegisteredSince = DateTime.Now },
                    new Supplier { Name = "MediEquip Solutions", ContactPerson = "Mr. John Davis", Email = "john@mediequip.com", Phone = "555-1002", Address = "456 Device Dr", IsActive = true, RegisteredSince = DateTime.Now },
                    new Supplier { Name = "Lab Supplies Co.", ContactPerson = "Ms. Sarah Chen", Email = "sarah@labsupplies.com", Phone = "555-1003", Address = "789 Test Lane", IsActive = true, RegisteredSince = DateTime.Now }
                );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Suppliers seeded.");
            }

            // Seed initial Inventory Items if none exist (requires Suppliers to be seeded first)
            if (!context.InventoryItems.Any() && context.Suppliers.Any())
            {
                var globalPharma = await context.Suppliers.FirstAsync(s => s.Name == "Global Pharma Inc.");
                var mediequip = await context.Suppliers.FirstAsync(s => s.Name == "MediEquip Solutions");

                context.InventoryItems.AddRange(
                    new InventoryItem { Name = "Pain Reliever Tablets", UnitOfMeasure = "Bottle", CurrentStock = 100, MinStockLevel = 20, PurchasePrice = 15.50m, Description = "Common pain relief medication", SupplierID = globalPharma.ID, IsActive = true, RegisteredSince = DateTime.Now },
                    new InventoryItem { Name = "Sterile Gloves", UnitOfMeasure = "Box", CurrentStock = 50, MinStockLevel = 10, PurchasePrice = 8.75m, Description = "Disposable examination gloves", SupplierID = globalPharma.ID, IsActive = true, RegisteredSince = DateTime.Now },
                    new InventoryItem { Name = "Blood Pressure Cuff", UnitOfMeasure = "Each", CurrentStock = 10, MinStockLevel = 2, PurchasePrice = 120.00m, Description = "Manual blood pressure measurement device", SupplierID = mediequip.ID, IsActive = true, RegisteredSince = DateTime.Now }
                );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Inventory Items seeded.");
            }

            Console.WriteLine("Database seeding complete.");
        }
    }
}
