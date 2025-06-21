using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Medical.Controllers
{
		[Area("Medical")]
		[Authorize(Policy = "RequireMedicalRole")]
		public class AvailableAppointmentController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;

        public AvailableAppointmentController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
    

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.Users
                                    .Include(u => u.Person)
                                    .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var availableAppointments = await _context.Appointments
                                            .Where(a => a.PersonID == currentUser.Person.ID)
                                            .Include(a => a.Patient)
                                                .ThenInclude(p => p.Person)
                                            .Include(a => a.PatientCheckInOut)
                                                .ThenInclude(pcio => pcio.PatientVisitRecord)
                                            .OrderByDescending(a => a.AppointmentDateTime)
                                            .ToListAsync();

            return View(availableAppointments);
        }
    }
}
