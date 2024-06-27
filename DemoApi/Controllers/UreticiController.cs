using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UreticiController : Controller
    {
        private readonly string SqlStr = "Data Source=192.168.1.17;Persist Security Info=True;Initial Catalog=Gozde;User ID=sefa;Password=sefa123;Connection Timeout=300";
        private readonly ILogger<UreticiController> _logger;

        public UreticiController(ILogger<UreticiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetUreticiler")]
        public ActionResult GetUreticiler()
        {
            List<UrunUretici> ureticiler;
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                ureticiler = con.Query<UrunUretici>("SELECT * FROM UrunUretici").ToList();
            }
            string json = JsonConvert.SerializeObject(ureticiler, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost("AddUretici")]
        public ActionResult AddUretici(UrunUretici uretici)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"INSERT INTO UrunUretici (Uretici) VALUES (@Uretici)";
                var result = con.Execute(query, uretici);
                if (result > 0)
                {
                    return Ok(new { message = "Üretici ekleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Üretici ekleme işlemi başarısız" });
                }
            }
        }

        [HttpPut("UpdateUretici")] 
        public ActionResult UpdateUretici(UrunUretici uretici)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"UPDATE UrunUretici SET Uretici = @Uretici WHERE UreticiId = @UreticiId";
                var result = con.Execute(query, uretici);
                if (result > 0)
                {
                    return Ok(new { message = "Üretici güncelleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Üretici güncelleme işlemi başarısız" });
                }
            }
        }

        [HttpDelete("DeleteUretici/{id}")]
        public ActionResult DeleteUretici(int id)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = "DELETE FROM UrunUretici WHERE UreticiId = @UreticiId";
                var result = con.Execute(query, new { UreticiId = id });
                if (result > 0)
                {
                    return Ok(new { message = "Üretici silme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Üretici silme işlemi başarısız" });
                }
            }
        }

    }
}
