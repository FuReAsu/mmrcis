using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmrcis.Areas.Admin.Controllers
{
		[Area("Admin")]
		[Authorize(Policy = "RequireAdminRole")]
		public class AuditLogController : Controller
		{
        private readonly CisDbContext _context;

        public AuditLogController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context) 
        {
            _context = context;
        }
    

        public IActionResult Index()
        {
            var auditlogs = _context.AuditLogs
                                                .OrderByDescending(al => al.Timestamp)
                                                .ToList();
            return View(auditlogs);
        }
    }
}
