using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoMvc.Controllers
{
    public class GrupController : Controller
    {
        private readonly ApiServices _apiService;

        public GrupController(ApiServices apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Gruplar()
        {
            List<UrunGruplari> gruplar = await _apiService.GetGrups();
            return View(gruplar);
        }
    }
}
