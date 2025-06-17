using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace mmrcis.Areas.Operator.Controllers
{
		[Area("Operator")]
		[Authorize(Policy = "RequireOperatorRole")]
		public class HomeController : Controller
		{
				public IActionResult Index()
				{
							ViewData["Title"] = "Operator Portal";
							return View();
				}
		}
}
