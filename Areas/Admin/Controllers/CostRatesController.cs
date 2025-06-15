
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmrcis.Data;
using mmrcis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Collections.Generic; 

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

        
        private async Task PopulateDropdowns()
        {
            
            
            ViewBag.ExistingCostTypes = await _context.CostRates
                                                    .Where(cr => !string.IsNullOrEmpty(cr.CostType)) 
                                                    .Select(cr => cr.CostType)
                                                    .Distinct()
                                                    .OrderBy(ct => ct)
                                                    .ToListAsync();

            
            ViewBag.IorE_Options = new List<SelectListItem>
            {
                new SelectListItem { Value = "In", Text = "Income" },
                new SelectListItem { Value = "Exp", Text = "Expense" }
            };
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.CostRates.OrderBy(cr => cr.CostCode).ToListAsync());
        }

        
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

        
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns(); 
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CostCode,CostAmount,CostType,IorE,AccountCode")] CostRate costRate)
        {
            
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
                    
                    if (ex.InnerException?.Message.Contains("Cannot insert duplicate key row") == true ||
                        ex.InnerException?.Message.Contains("Violation of UNIQUE KEY constraint") == true)
                    {
                        if (costRate.AccountCode.HasValue && _context.CostRates.Any(c => c.AccountCode == costRate.AccountCode.Value && c.ID != costRate.ID))
                        {
                            ModelState.AddModelError("AccountCode", "This Account Code already exists. Please enter a unique Account Code.");
                        }
                        else 
                        {
                            ModelState.AddModelError("CostCode", "This Cost Code already exists. Please enter a unique Cost Code.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the cost rate. Please try again.");
                    }
                    await PopulateDropdowns(); 
                    return View(costRate);
                }
            }
            await PopulateDropdowns(); 
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
            await PopulateDropdowns(); 
            return View(costRate);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CostCode,CostAmount,CostType,IorE,AccountCode,IsActive,RegisteredSince")] CostRate costRate) 
        {
            if (id != costRate.ID)
            {
                return NotFound();
            }

            
            if (!string.IsNullOrEmpty(costRate.CostType))
            {
                costRate.CostType = costRate.CostType.Trim();
            }

            if (ModelState.IsValid)
            {
                
                
                var originalCostRate = await _context.CostRates.AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

                if (originalCostRate == null)
                {
                    return NotFound(); 
                }

                try
                {
                    
                    
                    
                    costRate.CostCode = originalCostRate.CostCode;
                    costRate.AccountCode = originalCostRate.AccountCode;
                    
                    costRate.RegisteredSince = originalCostRate.RegisteredSince;
                    

                    _context.Update(costRate); 
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
                    
                    
                    if (ex.InnerException?.Message.Contains("Cannot insert duplicate key row") == true ||
                        ex.InnerException?.Message.Contains("Violation of UNIQUE KEY constraint") == true)
                    {
                        
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
                    await PopulateDropdowns(); 
                    return View(costRate);
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns(); 
            return View(costRate);
        }

        
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

        
        private bool CostRateExists(int id)
        {
            return _context.CostRates.Any(e => e.ID == id);
        }
    }
}
