using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class MarkaController : Controller
    {
        private readonly ApiServices _apiService;

        public MarkaController(ApiServices apiService)
        {
            _apiService = apiService;
        }

       

        public async Task<IActionResult> Markalar()
        {
            List<Markalar> markalar = await _apiService.GetMarkalar();
            return View(markalar);
        }
    }
}
