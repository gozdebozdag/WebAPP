using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
