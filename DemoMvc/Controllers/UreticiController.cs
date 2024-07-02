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
            List<UrunUretici> uretici = await _apiService.GetUreticiler();
            return View(uretici);
        }

        public async Task<IActionResult> UreticiDetay(int id)
        {
            UrunUretici uretici = await _apiService.GetUreticiByIdAsync(id);
            return View(uretici);
        }

        public IActionResult YeniUretici()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniUretici(UrunUretici yeniuretici)
        {
            if (ModelState.IsValid)
            {
                await _apiService.AddUreticiAsync(yeniuretici);
                return RedirectToAction("Ureticiler");
            }
            return View(yeniuretici);
        }

        public async Task<IActionResult> UreticiDuzenle(int id)
        {
            UrunUretici uretici = await _apiService.GetUreticiByIdAsync(id);
            if (uretici == null)
            {
                return NotFound();
            }
            return View(uretici);
        }

        [HttpPost]
        public async Task<IActionResult> UreticiDuzenle(UrunUretici uretici)
        {
            if (ModelState.IsValid)
            {
                var success = await _apiService.UpdateUreticiAsync(uretici);
                if (success)
                {
                    return RedirectToAction("Urunler");
                }
                ModelState.AddModelError(string.Empty, "Üretici güncelleme işlemi başarısız");
            }
            return View(uretici);
        }

        public async Task<IActionResult> UreticiSil(int id)
        {
            await _apiService.DeleteUreticiAsync(id);
            return RedirectToAction("Ureticiler");
        }
    }
}

