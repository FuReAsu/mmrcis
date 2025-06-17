using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmrcis.Areas.Admin.Models;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdminRole")] 
    public class StaffController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<StaffController> _logger; 
        private readonly IAuditService _auditService;

        public StaffController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<StaffController> logger,
                               IAuditService auditService) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger; 
            _auditService = auditService;
        }
        
        private async Task GenerateAuditLog(string action, string parameters)
        {
            var currentUser = await _userManager.Users
                                    .Include(u => u.Person)
                                    .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            string currentUserName = currentUser.Person.FullName;
            string currentAction = action;
            string currentController = "Staff";
            string currentParameters = parameters;
            string currentIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            string currentUserAgent = Request.Headers["User-Agent"].ToString();
            await _auditService.LogActionAsync(
                    currentUserName,
                    currentAction,
                    currentController,
                    parameters,
                    currentIpAddress,
                    currentUserAgent
                    );
        }

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
                    RegisteredSince = user.Person?.RegisteredSince ?? DateTime.MinValue 
                });
            }
            return View(staffList);
        }

        
        public async Task<IActionResult> Register()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var model = new RegisterStaffViewModel
            {
                AvailableRoles = roles
            };
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterStaffViewModel model)
        {
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            if (ModelState.IsValid)
            {
                
                var person = new Person
                {
                    FullName = model.FullName,
                    PersonType = model.SelectedRole, 
                    Qualification = model.Qualification,
                    Specialization = model.Specialization,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,


                    RegisteredSince = DateTime.Now
                };

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                
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
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    _logger.LogInformation($"Admin registered new user: {model.Email} with role: {model.SelectedRole}");
                    TempData["SuccessMessage"] = $"Staff member '{model.FullName}' registered successfully.";
                    
                    var logParameters = $"FullName={model.FullName}, " +
                                     $"PersonType={model.SelectedRole}, " +
                                     $"Qualification={model.Qualification}, " +
                                     $"Specialization={model.Specialization}, " +
                                     $"Address={model.Address}, " +
                                     $"PhoneNumber={model.PhoneNumber}, " +
                                     $"UserName={model.Email}, " +
                                     $"Email={model.Email}, " +
                                     $"PersonID={person.ID}"; 
                    await GenerateAuditLog("Create",  logParameters);

                    return RedirectToAction(nameof(Index)); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogWarning($"User creation failed for {model.Email}: {error.Description}");
                }
            }
            return View(model);
        }

        
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

            var model = new EditStaffViewModel 
            {
                UserId = user.Id,
                Email = user.Email,
                PersonId = user.Person?.ID ?? 0, 
                FullName = user.Person?.FullName,
                SelectedRole = roles.FirstOrDefault() ?? "No Role", 
                Qualification = user.Person?.Qualification,
                Specialization = user.Person?.Specialization,
                Address = user.Person?.Address,
                PhoneNumber = user.Person?.PhoneNumber,
                CurrentRoles = roles.ToList() 
            };

            return View(model);
        }


        
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
                SelectedRole = userRoles.FirstOrDefault() ?? string.Empty, 
                Qualification = user.Person?.Qualification,
                Specialization = user.Person?.Specialization,
                Address = user.Person?.Address,
                PhoneNumber = user.Person?.PhoneNumber,
                AvailableRoles = roles,
                CurrentRoles = userRoles.ToList()
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStaffViewModel model)
        {
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            model.CurrentRoles = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(model.UserId))).ToList(); 

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }
                
                string changedAccountFullName = user.Person.FullName;
                string changedUserName = user.UserName;

                user.Email = model.Email;
                user.UserName = model.Email; 

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "The new password and confirmation password do not match.");
                        return View(model);
                    }
                    
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

                
                if (user.Person != null)
                {
                    user.Person.FullName = model.FullName;
                    user.Person.PersonType = model.SelectedRole; 
                    user.Person.Qualification = model.Qualification;
                    user.Person.Specialization = model.Specialization;
                    user.Person.Address = model.Address;
                    user.Person.PhoneNumber = model.PhoneNumber;
                    
                    _context.Update(user.Person); 
                    await _context.SaveChangesAsync();
                }

                
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(model.SelectedRole))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }
                else if (currentRoles.Count > 1) 
                {
                    var rolesToRemove = currentRoles.Except(new[] { model.SelectedRole }).ToList();
                    if (rolesToRemove.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    }
                }


                _logger.LogInformation($"Admin updated user: {model.Email}");
                TempData["SuccessMessage"] = $"Staff member '{model.FullName}' updated successfully.";

                string logParameters = $"Edited UserName = {changedUserName}" + $"Edited UserFullName = {changedAccountFullName}";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        
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

            var model = new StaffListViewModel 
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id);
            var deletedUserFullName = user.Person?.FullName;
            if (user == null)
            {
                return NotFound();
            }

            
            if (user.Person != null)
            {
                _context.Persons.Remove(user.Person);
            }

            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync(); 
                _logger.LogInformation($"Admin deleted user: {user.Email}");
                TempData["SuccessMessage"] = $"Staff member '{user.Email}' and associated data deleted successfully.";

                string logParameters = $"UserFullName = {deletedUserFullName}";
                await GenerateAuditLog("Delete", logParameters);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            _logger.LogError($"Error deleting user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            return View(await _userManager.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == id)); 
        }
    }
}
