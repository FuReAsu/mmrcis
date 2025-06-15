
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

namespace mmrcis.Areas.Operator.Controllers
{
    [Area("Operator")] 
    [Authorize(Roles = "Operator,Admin")] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
