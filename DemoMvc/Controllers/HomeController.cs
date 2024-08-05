using DemoMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DemoApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using Markalar = DemoApi.Models.Markalar;

namespace DemoMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiServices _apiService;
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration,ApiServices apiService)
        {
            _apiService = apiService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IActionResult> Urunler()
        {
            IEnumerable<Urunler> urunler;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"
                SELECT u.UrunId, u.UrunAdi, u.MarkaId, m.Marka,ug.Grup,uu.Uretici,u.UrunKodu
                FROM Urunler u
                INNER JOIN Markalar m ON u.MarkaId = m.MarkaId
                INNER JOIN UrunGruplari ug on u.GrupId=ug.GrupId
                INNER JOIN UrunUretici uu on u.UreticiId=uu.UreticiId;";
                urunler = await db.QueryAsync<Urunler>(query);
            }

            return View(urunler);
        }
        public async Task<IActionResult> UrunDetay(int id)
        {
            Urunler urun;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"
                SELECT u.UrunId, u.UrunAdi, u.MarkaId, m.Marka, ug.Grup, uu.Uretici, u.UrunKodu
                FROM Urunler u
                INNER JOIN Markalar m ON u.MarkaId = m.MarkaId
                INNER JOIN UrunGruplari ug ON u.GrupId = ug.GrupId
                INNER JOIN UrunUretici uu ON u.UreticiId = uu.UreticiId
                WHERE u.UrunId = @UrunId;";

                urun = await db.QueryFirstOrDefaultAsync<Urunler>(query, new { UrunId = id });
            }

            if (urun == null)
            {
                return NotFound();
            }

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
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
           }

        [HttpPost]
        [Route("Home/UrunDuzenle/{id}")]
        public async Task<IActionResult> UrunDuzenle([FromBody]Urunler urun,int id)
        {
            if (ModelState.IsValid)
            {
                urun.UrunId = id;
                var success = await _apiService.UpdateProductAsync(urun);
                if (success)
                {
                    return RedirectToAction("Urunler");
                }
                ModelState.AddModelError(string.Empty, "�r�n g�ncelleme i�lemi ba�ar�s�z");
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
