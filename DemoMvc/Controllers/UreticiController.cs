using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class UreticiController : Controller
    {
        private readonly ApiServices _apiService;

        public UreticiController(ApiServices apiService)
        {
            _apiService = apiService;
        }



        public async Task<IActionResult> Ureticiler()
        {
            List<UrunUretici> markalar = await _apiService.GetUreticiler();
            return View(markalar);
        }
    }
}

