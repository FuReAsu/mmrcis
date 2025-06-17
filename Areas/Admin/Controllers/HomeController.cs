using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mmrcis.Areas.Admin.Controllers
{
		[Area("Admin")]
		[Authorize(Policy = "RequireAdminRole")]
		public class HomeController: Controller
		{
				public IActionResult Index()
				{
						ViewData["Title"] = "Admin Portal";
						return View();
				}
		}
}
