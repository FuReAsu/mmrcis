using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Areas.Medical.Models;
using mmrcis.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Medical.Controllers
{
		[Area("Medical")]
		[Authorize(Policy = "RequireMedicalRole")]
		public class PatientInfoController : Controller
		{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;

        public PatientInfoController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                                                .Include(p => p.Person)
                                                .ToListAsync();
            return View(patients);
        }
    }

}
