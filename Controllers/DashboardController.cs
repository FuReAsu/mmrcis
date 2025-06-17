using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mmrcis.Controllers
{
		[Authorize]
		public class DashboardController: Controller
		{
				public IActionResult Index()
				{
						ViewData["Title"] = "Clinic Dashboard";
						return View();
				}
		}
}
