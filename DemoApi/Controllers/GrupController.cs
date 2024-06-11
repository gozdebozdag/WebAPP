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
    public class GrupController : Controller
    {
        private readonly string SqlStr = "Data Source=192.168.1.17;Persist Security Info=True;Initial Catalog=Gozde;User ID=sefa;Password=sefa123;Connection Timeout=300";
        private readonly ILogger<GrupController> _logger;

        public GrupController(ILogger<GrupController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetGrups")]
        public ActionResult GetGrups()
        {
            List<UrunGruplari> gruplar;
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                gruplar = con.Query<UrunGruplari>("SELECT * FROM UrunGruplari").ToList();
            }
            string json = JsonConvert.SerializeObject(gruplar, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost("AddGrup")]
        public ActionResult AddGrup(UrunGruplari grup)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"INSERT INTO UrunGruplari (Grup) VALUES (@Grup)";
                var result = con.Execute(query, new { Grup = grup.Grup });
                if (result > 0)
                {
                    return Ok(new { message = "Grup ekleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Grup ekleme işlemi başarısız" });
                }
            }
        }

        [HttpPut("UpdateGrup")]
        public ActionResult UpdateGrup(UrunGruplari grup)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = @"UPDATE UrunGruplari SET Grup = @Grup WHERE GrupId = @GrupId";
                var result = con.Execute(query, new { Grup = grup.Grup, GrupId = grup.GrupId });
                if (result > 0)
                {
                    return Ok(new { message = "Grup güncelleme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Grup güncelleme işlemi başarısız" });
                }
            }
        }

        [HttpDelete("DeleteGrup/{id}")]
        public ActionResult DeleteGrup(int id)
        {
            using (IDbConnection con = new SqlConnection(SqlStr))
            {
                var query = "DELETE FROM UrunGruplari WHERE GrupId = @GrupId";
                var result = con.Execute(query, new { GrupId = id });
                if (result > 0)
                {
                    return Ok(new { message = "Grup silme işlemi başarılı" });
                }
                else
                {
                    return StatusCode(500, new { message = "Grup silme işlemi başarısız" });
                }
            }
        }
    }
}
