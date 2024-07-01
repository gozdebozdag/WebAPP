using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using DemoApi.Models;
using DemoApi.Services;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkaController : Controller
    {
        private readonly string SqlStr = "Data Source=192.168.1.17;Persist Security Info=True;Initial Catalog=Gozde;User ID=sefa;Password=sefa123;Connection Timeout=300";
        private readonly ILogger<MarkaController> _logger;
        private readonly IMarkaService _markaService;

        public MarkaController(IMarkaService markaService)
        {
            _markaService = markaService;
        }
        public MarkaController(ILogger<MarkaController> logger)
        {
            _logger = logger;
        }
        [HttpGet("GetBrand/{id}")]
        public ActionResult GetBrandById(int id)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var brand = con.Query<Urunler>("SELECT * FROM Markalar WHERE Id = @Id", new { Id = id }).FirstOrDefault();
                if (brand == null)
                {
                    throw new KeyNotFoundException("Brands not found.");
                }
                return Ok(brand);
            }
        }

        [HttpGet("GetBrands")]
        public ActionResult GetBrands()
        {
            List<Markalar> markalar;
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                markalar = con.Query<Markalar>("SELECT * FROM Markalar").ToList();
            }
            return Json(markalar);
        }

        [HttpPost("AddBrand")]
        public ActionResult AddBrand(Markalar marka)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"INSERT INTO Markalar (Marka) 
                              VALUES (@Marka)";
                var result = con.Execute(query, marka);
                if (result > 0)
                {
                    return Ok(new { message = "Marka ekleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Marka ekleme işlemi başarısız" });
                }
            }
        }

        [HttpPut("UpdateBrand")]
        public ActionResult UpdateBrand(Markalar marka)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"UPDATE Markalar 
                              SET Marka = @Marka
                              WHERE MarkaId = @MarkaId";
                var result = con.Execute(query, marka);
                if (result > 0)
                {
                    return Ok(new { message = "Marka güncelleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Marka güncelleme işlemi başarısız" });
                }
            }
        }

        [HttpDelete("DeleteBrand/{id}")]
        public ActionResult DeleteBrand(int id)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = "DELETE FROM Markalar WHERE MarkaId = @MarkaId";
                var result = con.Execute(query, new { MarkaId = id });
                if (result > 0)
                {
                    return Ok(new { message = "Marka silme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Marka silme işlemi başarısız" });
                }
            }
        }


    }

}

