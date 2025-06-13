// Areas/Admin/Controllers/StaffController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmrcis.Areas.Admin.Models;
using mmrcis.Data;
using mmrcis.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks; // Required for async methods

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")] // Only Admins can manage staff
    public class StaffController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<StaffController> _logger; // Add logger for error messages

        public StaffController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<StaffController> logger) // Inject logger
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger; // Assign logger
        }

        // GET: Admin/Staff (Lists all staff/users)
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Include(u => u.Person).ToListAsync();
            var staffList = new List<StaffListViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                staffList.Add(new StaffListViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.Person?.FullName,
                    PersonType = user.Person?.PersonType,
                    Roles = string.Join(", ", roles),
                    RegisteredSince = user.Person?.RegisteredSince ?? DateTime.MinValue // Handle null date if Person is null
                });
            }
            return View(staffList);
        }

        // GET: Admin/Staff/Register (Display form to register new staff)
        public async Task<IActionResult> Register()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var model = new RegisterStaffViewModel
            {
                AvailableRoles = roles
            };
            return View(model);
        }

        // POST: Admin/Staff/Register (Handle submission of new staff registration)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterStaffViewModel model)
        {
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            if (ModelState.IsValid)
            {
                // 1. Create the Person record
                var person = new Person
                {
                    FullName = model.FullName,
                    PersonType = model.SelectedRole, // Use the selected role as PersonType
                    Qualification = model.Qualification,
                    Specialization = model.Specialization,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
//                    Allergy = "", // Provide an empty string as default, or ensure nullable if changed in DB
//                    BloodGroup = "",
                    RegisteredSince = DateTime.Now
                };

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                // 2. Create the ApplicationUser, linking to the Person
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    PersonID = person.ID
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 3. Assign the selected role to the user
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    _logger.LogInformation($"Admin registered new user: {model.Email} with role: {model.SelectedRole}");
                    TempData["SuccessMessage"] = $"Staff member '{model.FullName}' registered successfully.";
                    return RedirectToAction(nameof(Index)); // Redirect to staff list after registration
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogWarning($"User creation failed for {model.Email}: {error.Description}");
                }
            }
            return View(model);
        }

        // GET: Admin/Staff/Details/{id} (Display details of a specific staff member)
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditStaffViewModel // Reusing EditStaffViewModel for display
            {
                UserId = user.Id,
                Email = user.Email,
                PersonId = user.Person?.ID ?? 0, // Handle null Person
                FullName = user.Person?.FullName,
                SelectedRole = roles.FirstOrDefault() ?? "No Role", // Display first role, or adjust as needed
                Qualification = user.Person?.Qualification,
                Specialization = user.Person?.Specialization,
                Address = user.Person?.Address,
                PhoneNumber = user.Person?.PhoneNumber,
                CurrentRoles = roles.ToList() // Pass all roles for display
            };

            return View(model);
        }


        // GET: Admin/Staff/Edit/{id} (Display form to edit existing staff)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditStaffViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                PersonId = user.Person?.ID ?? 0,
                FullName = user.Person?.FullName,
                SelectedRole = userRoles.FirstOrDefault() ?? string.Empty, // Pre-select current primary role
                Qualification = user.Person?.Qualification,
                Specialization = user.Person?.Specialization,
                Address = user.Person?.Address,
                PhoneNumber = user.Person?.PhoneNumber,
                AvailableRoles = roles,
                CurrentRoles = userRoles.ToList()
            };

            return View(model);
        }

        // POST: Admin/Staff/Edit (Handle submission of staff edits)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStaffViewModel model)
        {
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            model.CurrentRoles = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(model.UserId))).ToList(); // Keep current roles for display if validation fails

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                // Update ApplicationUser details
                user.Email = model.Email;
                user.UserName = model.Email; // Keep UserName and Email in sync

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "The new password and confirmation password do not match.");
                        return View(model);
                    }
                    // Remove current password hash and set new one (this is how Identity handles password changes)
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        foreach (var error in passwordChangeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    foreach (var error in userUpdateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Update Person details
                if (user.Person != null)
                {
                    user.Person.FullName = model.FullName;
                    user.Person.PersonType = model.SelectedRole; // Keep PersonType consistent with primary role
                    user.Person.Qualification = model.Qualification;
                    user.Person.Specialization = model.Specialization;
                    user.Person.Address = model.Address;
                    user.Person.PhoneNumber = model.PhoneNumber;
                    // Allergy field is handled as per your previous fix (nullable or default)
                    _context.Update(user.Person); // Mark Person entity as modified
                    await _context.SaveChangesAsync();
                }

                // Update Roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(model.SelectedRole))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }
                else if (currentRoles.Count > 1) // If user has multiple roles, and only one is selected, remove others
                {
                    var rolesToRemove = currentRoles.Except(new[] { model.SelectedRole }).ToList();
                    if (rolesToRemove.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    }
                }


                _logger.LogInformation($"Admin updated user: {model.Email}");
                TempData["SuccessMessage"] = $"Staff member '{model.FullName}' updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Admin/Staff/Delete/{id} (Display confirmation for deleting staff)
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new StaffListViewModel // Reusing StaffListViewModel for display
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.Person?.FullName,
                PersonType = user.Person?.PersonType,
                Roles = string.Join(", ", roles),
                RegisteredSince = user.Person?.RegisteredSince ?? DateTime.MinValue
            };

            return View(model);
        }

        // POST: Admin/Staff/Delete/{id} (Handle actual deletion of staff)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Important: Handle associated Person record
            if (user.Person != null)
            {
                _context.Persons.Remove(user.Person);
            }

            // Delete the ApplicationUser
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync(); // Save changes for Person deletion if applicable
                _logger.LogInformation($"Admin deleted user: {user.Email}");
                TempData["SuccessMessage"] = $"Staff member '{user.Email}' and associated data deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            _logger.LogError($"Error deleting user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            return View(await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id)); // Re-display delete view with errors
        }
    }
}
