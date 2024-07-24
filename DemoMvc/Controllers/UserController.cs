using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
