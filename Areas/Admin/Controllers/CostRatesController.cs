// Areas/Admin/Controllers/CostRatesController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering; // Keep this, as SelectListItem is still useful for IorE
using System.Collections.Generic; // Add this for List<SelectListItem>

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CostRatesController : Controller
    {
        private readonly CisDbContext _context;

        public CostRatesController(CisDbContext context)
        {
            _context = context;
        }

        // Modified helper method to populate dropdowns and dynamic CostTypes
        private async Task PopulateDropdowns()
        {
            // For CostType: Get all distinct existing CostTypes from the database
            // Order by for consistent display
            ViewBag.ExistingCostTypes = await _context.CostRates
                                                    .Where(cr => !string.IsNullOrEmpty(cr.CostType)) // Exclude null/empty
                                                    .Select(cr => cr.CostType)
                                                    .Distinct()
                                                    .OrderBy(ct => ct)
                                                    .ToListAsync();

            // For IorE (Income or Expense) - remains a fixed list unless you also make this dynamic
            ViewBag.IorE_Options = new List<SelectListItem>
            {
                new SelectListItem { Value = "In", Text = "Income" },
                new SelectListItem { Value = "Exp", Text = "Expense" }
            };
        }

        // GET: Admin/CostRates
        public async Task<IActionResult> Index()
        {
            return View(await _context.CostRates.OrderBy(cr => cr.CostCode).ToListAsync());
        }

        // GET: Admin/CostRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costRate = await _context.CostRates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (costRate == null)
            {
                return NotFound();
            }

            return View(costRate);
        }

        // GET: Admin/CostRates/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns(); // Call async version
            return View();
        }

        // POST: Admin/CostRates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CostCode,CostAmount,CostType,IorE,AccountCode")] CostRate costRate)
        {
            // Trim whitespace from CostType
            if (!string.IsNullOrEmpty(costRate.CostType))
            {
                costRate.CostType = costRate.CostType.Trim();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    costRate.RegisteredSince = DateTime.Now;
                    _context.Add(costRate);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Cost Rate '{costRate.CostCode}' created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Check for unique constraint violation (CostCode or AccountCode)
                    if (ex.InnerException?.Message.Contains("Cannot insert duplicate key row") == true ||
                        ex.InnerException?.Message.Contains("Violation of UNIQUE KEY constraint") == true)
                    {
                        if (costRate.AccountCode.HasValue && _context.CostRates.Any(c => c.AccountCode == costRate.AccountCode.Value && c.ID != costRate.ID))
                        {
                            ModelState.AddModelError("AccountCode", "This Account Code already exists. Please enter a unique Account Code.");
                        }
                        else // Assume it's CostCode if not AccountCode
                        {
                            ModelState.AddModelError("CostCode", "This Cost Code already exists. Please enter a unique Cost Code.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the cost rate. Please try again.");
                    }
                    await PopulateDropdowns(); // Re-populate dropdowns if returning to view
                    return View(costRate);
                }
            }
            await PopulateDropdowns(); // Re-populate dropdowns if model state is invalid
            return View(costRate);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costRate = await _context.CostRates.FindAsync(id);
            if (costRate == null)
            {
                return NotFound();
            }
            await PopulateDropdowns(); // Call async version
            return View(costRate);
        }

        // POST: Admin/CostRates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CostCode,CostAmount,CostType,IorE,AccountCode,IsActive,RegisteredSince")] CostRate costRate) 
        {
            if (id != costRate.ID)
            {
                return NotFound();
            }

            // Trim whitespace from CostType
            if (!string.IsNullOrEmpty(costRate.CostType))
            {
                costRate.CostType = costRate.CostType.Trim();
            }

            if (ModelState.IsValid)
            {
                // Retrieve the original CostRate from the database
                // AsNoTracking() means EF won't track this entity, allowing us to attach the 'costRate' later.
                var originalCostRate = await _context.CostRates.AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

                if (originalCostRate == null)
                {
                    return NotFound(); // This case implies the item was deleted between GET and POST
                }

                try
                {
                    // --- IMPORTANT: Enforce immutability of CostCode and AccountCode ---
                    // Re-assign the original CostCode and AccountCode from the database
                    // This prevents any changes to these fields, even if the readonly attribute is bypassed.
                    costRate.CostCode = originalCostRate.CostCode;
                    costRate.AccountCode = originalCostRate.AccountCode;
                    // Ensure RegisteredSince is also preserved if not editable in form
                    costRate.RegisteredSince = originalCostRate.RegisteredSince;
                    // ------------------------------------------------------------------

                    _context.Update(costRate); // Now 'costRate' contains the original codes and updated other fields
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Cost Rate '{costRate.CostCode}' updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostRateExists(costRate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle unique constraint violation during edit if CostCode or AccountCode were somehow attempted to be changed
                    // Although the above logic prevents changes, this catch remains for other potential DB errors or unexpected scenarios.
                    if (ex.InnerException?.Message.Contains("Cannot insert duplicate key row") == true ||
                        ex.InnerException?.Message.Contains("Violation of UNIQUE KEY constraint") == true)
                    {
                        // If it's an account code conflict with another entry, or cost code (though should be prevented)
                        if (costRate.AccountCode.HasValue && _context.CostRates.Any(c => c.AccountCode == costRate.AccountCode.Value && c.ID != costRate.ID))
                        {
                            ModelState.AddModelError("AccountCode", "This Account Code already exists for another cost rate. Only non-conflicting updates are allowed.");
                        }
                        else
                        {
                            ModelState.AddModelError("CostCode", "This Cost Code cannot be updated to a duplicate value. Please ensure the original code remains unique.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the cost rate. Please try again.");
                    }
                    await PopulateDropdowns(); // Re-populate dropdowns if returning to view
                    return View(costRate);
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(); // Re-populate dropdowns if model state is invalid
            return View(costRate);
        }

        // GET: Admin/CostRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costRate = await _context.CostRates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (costRate == null)
            {
                return NotFound();
            }

            return View(costRate);
        }

        // POST: Admin/CostRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var costRate = await _context.CostRates.FindAsync(id);
            if (costRate != null)
            {
                _context.CostRates.Remove(costRate);
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Cost Rate '{costRate?.CostCode}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // This is the method definition that was likely misplaced or affected by a syntax error
        private bool CostRateExists(int id)
        {
            return _context.CostRates.Any(e => e.ID == id);
        }
    }
}
