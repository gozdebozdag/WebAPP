using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
