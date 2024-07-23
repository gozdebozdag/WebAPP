using DemoMvc.Models;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _apiService.RegisterAsync(userDto);
                if (user != null)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Kayıt başarısız.");
            }
            return View(userDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var token = await _apiService.LoginAsync(userDto);
                if (!string.IsNullOrEmpty(token))
                {
                    // Giriş başarılı, token'ı saklayın
                    HttpContext.Session.SetString("JWTToken", token);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            }
            return View(userDto);
        }
    }
}
