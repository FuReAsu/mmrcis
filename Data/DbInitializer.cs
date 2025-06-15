
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using mmrcis.Models; 
using System.Linq;
using System.Threading.Tasks;
using System; 

namespace mmrcis.Data 
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            CisDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            
            
            await context.Database.MigrateAsync();

            
            
            string[] roleNames = { "Admin", "Doctor", "Nurse", "Operator" };

            foreach (var roleName in roleNames)
            {
                
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            
            string adminEmail = "admin@test.local";
            string adminPassword = "P@ssw0rd"; 
            string adminRole = "Admin";

            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            
            if (adminUser == null)
            {
                
                
                var adminPerson = new Person
                {
                    FullName = "System Administrator",
                    PersonType = "Admin", 
                    Address = "123 Admin Lane, CIS HQ",
                    Qualification = "Certified Administrator",
                    RegisteredSince = DateTime.Now,
                    BloodGroup = "B",
                    Sex = "Male", 
                    PhoneNumber = "111-222-3333", 
                    Email = adminEmail 
                };
                context.Persons.Add(adminPerson);
                await context.SaveChangesAsync(); 

                
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, 
                    PersonID = adminPerson.ID 
                };

                
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine($"Admin user '{adminEmail}' created and assigned '{adminRole}' role.");
                }
                else
                {
                    
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    
                    
                }
            }
            else
            {
                
                if (!await userManager.IsInRoleAsync(adminUser, adminRole))
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine($"Admin user '{adminEmail}' already exists, ensuring '{adminRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Admin user '{adminEmail}' already exists and has '{adminRole}' role.");
                }

                
                
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

            
            string operatorEmail = "operator@test.local";
            string operatorPassword = "P@ssw0rd";
            string operatorRole = "Operator";

            var operatorUser = await userManager.FindByEmailAsync(operatorEmail);

            if (operatorUser == null)
            {
                
                var operatorPerson = new Person
                {
                    FullName = "Clinic Operator",
                    PersonType = "Operator", 
                    Address = "456 Operator Street, Clinic Office",
                    Qualification = "Clinic Staff",
                    RegisteredSince = DateTime.Now,
                    BloodGroup = "A",
                    Sex = "Female", 
                    PhoneNumber = "444-555-6666", 
                    Email = operatorEmail 
                };
                context.Persons.Add(operatorPerson);
                await context.SaveChangesAsync(); 

                
                operatorUser = new ApplicationUser
                {
                    UserName = operatorEmail,
                    Email = operatorEmail,
                    EmailConfirmed = true,
                    PersonID = operatorPerson.ID 
                };

                
                var result = await userManager.CreateAsync(operatorUser, operatorPassword);
                if (result.Succeeded)
                {
                    
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
                
                if (!await userManager.IsInRoleAsync(operatorUser, operatorRole))
                {
                    await userManager.AddToRoleAsync(operatorUser, operatorRole);
                    Console.WriteLine($"Operator user '{operatorEmail}' already exists, ensuring '{operatorRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Operator user '{operatorEmail}' already exists and has '{operatorRole}' role.");
                }

                
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
