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

        public async Task<IActionResult> GrupDetay(int id)
        {
            UrunGruplari grup = await _apiService.GetGrupByIdAsync(id);
            return View(grup);
        }

        public IActionResult YeniGrup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniGrup(UrunGruplari yenigrup)
        {
            if (ModelState.IsValid)
            {
                await _apiService.AddGrupAsync(yenigrup);
                return RedirectToAction("Gruplar");
            }
            return View(yenigrup);
        }

        public async Task<IActionResult> GrupDuzenle(int id)
        {
            UrunGruplari grup = await _apiService.GetGrupByIdAsync(id);
            if (grup == null)
            {
                return NotFound();
            }
            return View(grup);
        }

        [HttpPost]
        [Route("Grup/GrupDuzenle/{id}")]
        public async Task<IActionResult> GrupDuzenle([FromBody] UrunGruplari grup, int id)
        {
            if (ModelState.IsValid)
            {
                grup.GrupId = id;

                var success = await _apiService.UpdateGrupAsync(grup);
                if (success)
                {
                    return RedirectToAction("Gruplar");
                }
                ModelState.AddModelError(string.Empty, "Ürün güncelleme işlemi başarısız");
            }
            return View(grup);
        }



        public async Task<IActionResult> GrupSil(int id)
        {
            await _apiService.DeleteGrupAsync(id);
            return RedirectToAction("Gruplar");
        }
    }
}
