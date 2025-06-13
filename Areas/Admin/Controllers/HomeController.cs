// Areas/Admin/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mmrcis.Areas.Admin.Controllers
{
    [Area("Admin")] // This attribute is essential for MVC to recognize this as an Area controller
    [Authorize(Policy = "RequireAdminRole")] // Only users with the "Admin" role can access this controller
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }
    }
}
