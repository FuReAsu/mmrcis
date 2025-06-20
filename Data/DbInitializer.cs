using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using mmrcis.Models;

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
            
            //add Admin User
            await InsertStaffUserRecords(context, userManager, roleManager,"admin@test.local","Administrator","Admin","111-2222-333","P@ssw0rd","No 27, Admin Steet,MMRCIS");
            
            //add Operator User
            await InsertStaffUserRecords(context,userManager,roleManager,"operator@test.local","Clinic Operator","Operator","222-111-44","P@ssw0rd","No 65, Operator Road,MMRCIS");
            
            //add Nurse User
            await InsertStaffUserRecords(context,userManager,roleManager,"nurse@test.local","Miss Nurse","Nurse","345-234-55","P@ssw0rd","No 5, Nurse Road,MMRCIS");

            //add Doctor User
            await InsertStaffUserRecords(context,userManager,roleManager,"doctor@test.local","Mister Doctor","Doctor","213-541-66","P@ssw0rd","No 25, Doctor Road,MMRCIS");

            await InsertStaffUserRecords(context,userManager,roleManager,"doctor2@test.local","John Watkins","Doctor","345-561-77","P@ssw0rd","No 47, Hello Road,MMRCIS");

            //add Patient Records
            var jdDOB = new DateTime(1996, 5, 30);
            var msDOB = new DateTime(1980, 10, 11);
            await InsertPatientRecords(context, "Jane Doe", "No 78 NEPS Lumina Square","235-9832-51" , "jd@neps.com", jdDOB, "Female", "B" ); 
            await InsertPatientRecords(context, "Michael Seyers", "No 34, Manila Street", "903-587-4144", "ms@indie.net", msDOB, "Male", "AB" ); 

            //Insert CostRates
            if (!context.CostRates.Any())
            {
                context.CostRates.AddRange(
                    new CostRate { CostType = "GeneralConsultaion", UnitCost = 50.00m, Description = "General Consultation Fee", IsActive = true, RegisteredSince = DateTime.Now  },
                    new CostRate { CostType = "SpecialistConsultation", UnitCost = 75.00m, Description = "Specialized Consultation Fee", IsActive = true, RegisteredSince = DateTime.Now },
                    new CostRate { CostType = "OPD Visit", UnitCost = 25.00m, Description = "OPD Visit Fee", IsActive = true, RegisteredSince = DateTime.Now  },
                    new CostRate { CostType = "X-Ray", UnitCost = 90.00m, Description = "X-Ray Scanning Fee", IsActive = true,RegisteredSince = DateTime.Now }
                    );
                await context.SaveChangesAsync();
                Console.WriteLine("Initial Cost Rates seeded.");
            }
        }

        private static async Task InsertStaffUserRecords(
        CisDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        string email,
        string fullName, 
        string role, 
        string phoneNumber, 
        string password,
        string address)
        {
            var newUser = await userManager.FindByEmailAsync(email);

            if (newUser == null)
            {
                var newPerson = new Person
                {
                    FullName = fullName,
                    PersonType = role,
                    Address = address,
                    PhoneNumber = phoneNumber,
                    RegisteredSince = DateTime.Now,
                    Email = email 
                };
                context.Persons.Add(newPerson);
                await context.SaveChangesAsync();
                
                newUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    PersonID = newPerson.ID, 
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, password);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                    Console.WriteLine($"New {role} user '{fullName}' created and assigned '{role}' role.");
                }
                else
                {
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(newUser, role))
                {
                    await userManager.AddToRoleAsync(newUser, role);
                    Console.WriteLine($"New {role} user '{fullName}' already exists, ensuring '{role}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"New {role} user '{fullName}' already exists and has '{role}' role.");
                }

                if (newUser.PersonID == null)
                {
                    var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == email && p.PersonType == role);
                    if (existingPerson != null)
                    {
                        newUser.PersonID = existingPerson.ID;
                        await userManager.UpdateAsync(newUser);
                        Console.WriteLine($"New {role} user '{fullName}' linked to existing Person ID: {existingPerson.ID}.");
                    }
                }
            }
        }

        private static async Task InsertPatientRecords
        (
            CisDbContext context,    
            string fullName,
            string address,
            string phoneNumber,
            string email,
            DateTime dob,
            string sex,
            string bloodGroup
        )
        {   var checkPerson = await context.Patients
                                    .Include(p => p.Person)
                                    .FirstOrDefaultAsync(p => p.Person.FullName == fullName);
            
            if (checkPerson == null)
            {
                var newPerson = new Person
                {
                    FullName = fullName,
                    Address = address,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    DOB = dob,
                    Sex = sex,
                    BloodGroup = bloodGroup,
                    RegisteredSince = DateTime.Now,
                    PersonType = "Patient"
                };

                context.Persons.Add(newPerson);
                await context.SaveChangesAsync();

                var newPatient = new Patient
                {
                    PersonID = newPerson.ID,
                    Status = "Active",
                    PatientSince = DateTime.Now
                };

                context.Patients.Add(newPatient);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Patient {fullName} already  exists, skipping seeding");
            }
        }
    }
}
