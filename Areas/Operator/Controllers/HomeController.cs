// Areas/Operator/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")] // --- CRITICAL: This links the controller to the "Operator" area ---
    [Authorize(Roles = "Operator,Admin")] // Ensure only authorized roles can access this area
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // You can add data to pass to the view if needed, e.g., dashboard stats for the operator
            return View();
        }
    }
}
