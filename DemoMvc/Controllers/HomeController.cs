using DemoMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DemoApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DemoMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiServices _apiService;

        public HomeController(ApiServices apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Urunler()
        {
            //List<Urunler> urunler = await _apiService.GetUrunlerAsync();
            return View(/*urunler*/);
        }

        public async Task<IActionResult> UrunlerWithMarka()
        {
            List<Urunler> urunlerWithMarka = await _apiService.GetUrunlerWithMarkaAsync();
            return View(urunlerWithMarka);
        }


        public async Task<IActionResult> UrunDetay(int id)
        {
            Urunler urun = await _apiService.GetProductByIdAsync(id);
            return View(urun);
        }

        public IActionResult YeniUrun()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniUrun(Urunler yeniUrun)
        {
            if (ModelState.IsValid)
            {
                await _apiService.AddProductAsync(yeniUrun);
                return RedirectToAction("Urunler");
            }
            return View(yeniUrun);
        }

        public async Task<IActionResult> UrunDuzenle(int id)
        {
            Urunler urun = await _apiService.GetProductByIdAsync(id);
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> UrunDuzenle(Urunler urun)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateProductAsync(urun);
                return RedirectToAction("Urunler");
            }
            return View(urun);
        }

        public async Task<IActionResult> UrunSil(int id)
        {
            await _apiService.DeleteProductAsync(id);
            return RedirectToAction("Urunler");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
