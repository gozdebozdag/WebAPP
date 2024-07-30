using DemoMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;

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
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = username;
            return View();
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
                try
                {
                    var user = await _apiService.RegisterAsync(userDto);
                    if (user != null)
                    {
                        return Redirect("/");
                    }

                    ModelState.AddModelError(string.Empty, "Kayıt başarısız.");
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu.");
                }
            }
            return View(userDto);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:7271/api/Auth/Login?Username=" + userDto.Username + "&Password=" + userDto.Password, new StringContent(string.Empty));

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { error = $"Error: {response.StatusCode} - {errorContent}" });
            }

            var responseData = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (responseData != null)
            {
                HttpContext.Session.SetString("Username", responseData.Username);
                return Json(new { success = true });
            }

            return Json(new { success = false, error = "Invalid login attempt." });
        }
    }
}

