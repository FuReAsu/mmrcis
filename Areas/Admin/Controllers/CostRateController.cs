using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mmrcis.Data;
using mmrcis.Models;
using mmrcis.Services;

namespace mmrcis.Areas.Admin.Controllers
{
		[Area("Admin")]
		[Authorize(Policy = "RequireAdminRole")]
		public class CostRateController : Controller
		{
				private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CisDbContext _context;
        private readonly ILogger<CostRateController> _logger; 
        private readonly IAuditService _auditService;

        public CostRateController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               CisDbContext context,
                               ILogger<CostRateController> logger,
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
            string currentController = "CostRate";
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
						var costRates = await _context.CostRates.ToListAsync();
            return View(costRates);
				}
        
        public IActionResult Create()
        {
           var model = new CostRate();
           return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CostRate model)
        {
            if (ModelState.IsValid)
            {
                var costrate = new CostRate
                {
                    CostType = model.CostType,
                    UnitCost = model.UnitCost,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    RegisteredSince = DateTime.Now 
                };
                _context.CostRates.Add(costrate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"CostRate for '{model.CostType}' added successfully!";
                _logger.LogInformation($"Admin registered new CostRate: {model.CostType}");
                string logParameters = $"Created CostRate = {model.CostType}";
                await GenerateAuditLog("Create",logParameters);
                
                return RedirectToAction(nameof(Index));

            }
            
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costrate = await _context.CostRates.FindAsync(id);
            if (costrate == null)
            {
                return NotFound();
            }
            return View(costrate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CostRate model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var costrate = await _context.CostRates.FindAsync(id);
                string oldCostRate = costrate.CostType;

                if (costrate == null)
                {
                    return NotFound();
                }
                costrate.CostType = model.CostType;
                costrate.Description = model.Description;
                costrate.UnitCost = model.UnitCost;
                costrate.IsActive = model.IsActive;

                _context.Update(costrate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"CostRate '{model.CostType}' updated successfully!";
                _logger.LogInformation($"Admin edited CostRate: {oldCostRate}");
                string logParameters = $"Edited CostRate = {oldCostRate}";
                await GenerateAuditLog("Edit", logParameters);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costrate = await _context.CostRates.FindAsync(id);
       
            if (costrate == null)
            {
                return NotFound();
            }
            
            return View(costrate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var costrate = await _context.CostRates.FindAsync(id);
            
            if (costrate == null)
            {
                return NotFound();
            }

            return View(costrate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CostRate model)
        {

            if (id != model.ID)
            {
                return NotFound();
            }

            var costrate = await _context.CostRates.FindAsync(id);
            string deletedCostRate = costrate.CostType;

            if (costrate == null)
            {
                return NotFound();
            }
            
            try
            {
                _context.CostRates.Remove(costrate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to delete CostRate due to related IncomeBillItem Entries");
                ModelState.AddModelError("", "Cannot delete this patient because there are related IncomeBillItem Entries");
                return View(costrate);
            } 
            
            TempData["SuccessMessage"] = $"CostRate '{deletedCostRate}' deleted successfully!";
            _logger.LogInformation($"Admin deleted CostRate: {deletedCostRate}");

            string logParameters = $"Deleted CostRate = {deletedCostRate}";
            await GenerateAuditLog("Delete", logParameters);

            return RedirectToAction(nameof(Index));

        }
		}
}
