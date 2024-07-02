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
            List<Markalar> markalar = await _apiService.GetBrand();
            return View(markalar);
        }

        public async Task<IActionResult> MarkaDetay(int id)
        {
            Markalar marka = await _apiService.GetBrandByIdAsync(id);
            return View(marka);
        }

        public IActionResult YeniMarka()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniMarka(Markalar yeniMarka)
        {
            if (ModelState.IsValid)
            {
                await _apiService.AddBrandAsync(yeniMarka);
                return RedirectToAction("Markalar");
            }
            return View(yeniMarka);
        }

        public async Task<IActionResult> MarkaDuzenle(int id)
        {
            Markalar marka = await _apiService.GetBrandByIdAsync(id);
            if (marka == null)
            {
                return NotFound();
            }
            return View(marka);
        }

        [HttpPost]
        public async Task<IActionResult> MarkaDuzenle(Markalar marka)
        {
            if (ModelState.IsValid)
            {
                var success = await _apiService.UpdateBrandAsync(marka);
                if (success)
                {
                    return RedirectToAction("Markalar");
                }
                ModelState.AddModelError(string.Empty, "Ürün güncelleme işlemi başarısız");
            }
            return View(marka);
        }

        public async Task<IActionResult> MarkaSil(int id)
        {
            await _apiService.DeleteProductAsync(id);
            return RedirectToAction("Markalar");
        }
    }
}
