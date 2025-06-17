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
            
            string adminEmail = "admin@test.local";
            string adminPassword = "P@ssw0rd";
            string adminRole = "Admin";
            string adminPhoneNumber = "111-222-3333";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var adminPerson = new Person
                {
                    FullName = "Admin",
                    PersonType = "Admin",
                    Address = "123 Admin Lane, MMRCIS",
                    PhoneNumber = adminPhoneNumber,
                    RegisteredSince = DateTime.Now,
                    Email = adminEmail
                };
                context.Persons.Add(adminPerson);
                await context.SaveChangesAsync();
                
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PersonID = adminPerson.ID,
                    PhoneNumber = adminPhoneNumber,
                    PhoneNumberConfirmed = true
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
            string operatorPhoneNumber = "444-555-6666";

            var operatorUser = await userManager.FindByEmailAsync(operatorEmail);

            if (operatorUser == null)
            {

                var operatorPerson = new Person
                {
                    FullName = "Clinic Operator",
                    PersonType = "Operator",
                    Address = "456 Operator Street, Clinic Office",
                    RegisteredSince = DateTime.Now,
                    PhoneNumber = operatorPhoneNumber,
                    Email = operatorEmail
                };
                context.Persons.Add(operatorPerson);
                await context.SaveChangesAsync();

                operatorUser = new ApplicationUser
                {
                    UserName = operatorEmail,
                    Email = operatorEmail,
                    EmailConfirmed = true,
                    PersonID = operatorPerson.ID,
                    PhoneNumber = operatorPhoneNumber,
                    PhoneNumberConfirmed = true
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
            string doctorEmail = "doctor@test.local";
            string doctorPassword = "P@ssw0rd";
            string doctorRole = "Doctor";
            string doctorPhoneNumber = "444-555-6666";

            var doctorUser = await userManager.FindByEmailAsync(doctorEmail);

            if (doctorUser == null)
            {

                var doctorPerson = new Person
                {
                    FullName = "Clinic Doctor",
                    PersonType = "Doctor",
                    Address = "456 Doctor Street, Clinic Office",
                    RegisteredSince = DateTime.Now,
                    PhoneNumber = doctorPhoneNumber,
                    Email = doctorEmail
                };
                context.Persons.Add(doctorPerson);
                await context.SaveChangesAsync();

                doctorUser = new ApplicationUser
                {
                    UserName = doctorEmail,
                    Email = doctorEmail,
                    EmailConfirmed = true,
                    PersonID = doctorPerson.ID,
                    PhoneNumber = doctorPhoneNumber,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(doctorUser, doctorPassword);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(doctorUser, doctorRole);
                    Console.WriteLine($"Doctor user '{doctorEmail}' created and assigned '{doctorRole}' role.");
                }
                else
                {
                    Console.WriteLine($"Error creating doctor user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {

                if (!await userManager.IsInRoleAsync(doctorUser, doctorRole))
                {
                    await userManager.AddToRoleAsync(doctorUser, doctorRole);
                    Console.WriteLine($"Doctor user '{doctorEmail}' already exists, ensuring '{doctorRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Doctor user '{doctorEmail}' already exists and has '{doctorRole}' role.");
                }


                if (doctorUser.PersonID == null)
                {
                    var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == doctorEmail && p.PersonType == "Doctor");
                    if (existingPerson != null)
                    {
                        doctorUser.PersonID = existingPerson.ID;
                        await userManager.UpdateAsync(doctorUser);
                        Console.WriteLine($"Doctor user '{doctorEmail}' linked to existing Person ID: {existingPerson.ID}.");
                    }
                }
            }
            string nurseEmail = "nurse@test.local";
            string nursePassword = "P@ssw0rd";
            string nurseRole = "Nurse";
            string nursePhoneNumber = "444-555-6666";

            var nurseUser = await userManager.FindByEmailAsync(nurseEmail);

            if (nurseUser == null)
            {

                var nursePerson = new Person
                {
                    FullName = "Clinic Nurse",
                    PersonType = "Nurse",
                    Address = "456 Nurse Street, Clinic Office",
                    RegisteredSince = DateTime.Now,
                    PhoneNumber = nursePhoneNumber,
                    Email = nurseEmail
                };
                context.Persons.Add(nursePerson);
                await context.SaveChangesAsync();

                nurseUser = new ApplicationUser
                {
                    UserName = nurseEmail,
                    Email = nurseEmail,
                    EmailConfirmed = true,
                    PersonID = nursePerson.ID,
                    PhoneNumber = nursePhoneNumber,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(nurseUser, nursePassword);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(nurseUser, nurseRole);
                    Console.WriteLine($"Nurse user '{nurseEmail}' created and assigned '{nurseRole}' role.");
                }
                else
                {
                    Console.WriteLine($"Error creating nurse user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {

                if (!await userManager.IsInRoleAsync(nurseUser, nurseRole))
                {
                    await userManager.AddToRoleAsync(nurseUser, nurseRole);
                    Console.WriteLine($"Nurse user '{nurseEmail}' already exists, ensuring '{nurseRole}' role is assigned.");
                }
                else
                {
                    Console.WriteLine($"Nurse user '{nurseEmail}' already exists and has '{nurseRole}' role.");
                }


                if (nurseUser.PersonID == null)
                {
                    var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == nurseEmail && p.PersonType == "Nurse");
                    if (existingPerson != null)
                    {
                        nurseUser.PersonID = existingPerson.ID;
                        await userManager.UpdateAsync(nurseUser);
                        Console.WriteLine($"Nurse user '{nurseEmail}' linked to existing Person ID: {existingPerson.ID}.");
                    }
                }
            }


        }
    }
}
