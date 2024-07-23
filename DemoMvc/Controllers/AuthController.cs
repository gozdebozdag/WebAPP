using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiServices _apiService;

        public AuthController(ApiServices apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Login()
        //{
           
        //}

        //public async Task<IActionResult> Register(int id)
        //{
           
        //}
    }
}
