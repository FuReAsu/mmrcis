using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mmrcis.Areas.Medical.Controllers
{
		[Area("Medical")]
		[Authorize(Policy = "RequireMedicalRole")]
		public class HomeController: Controller
		{
				public IActionResult Index()
				{
						ViewData["Title"] = "Medical Portal";
						return View();
				}
		}
}
