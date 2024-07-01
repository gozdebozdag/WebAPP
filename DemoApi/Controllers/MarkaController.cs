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
        private readonly IMarkaService _markaService;

        public MarkaController(IMarkaService markaService)
        {
            _markaService = markaService;
        }

        [HttpGet("GetBrands")]
        public ActionResult GetBrand()
        {
            var brands = _markaService.GetAllBrands();
            return Ok(brands);
        }

        [HttpGet("GetBrand/{id}")]
        public ActionResult GetProduct(int id)
        {
            var brand = _markaService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpPost("AddBrand")]
        public ActionResult AddBrand(Markalar marka)
        {
            _markaService.AddBrand(marka);
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("UpdateBrand")]
        public ActionResult UpdateBrand(Markalar marka)
        {
            _markaService.AddBrand(marka);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("DeleteBrand/{id}")]
        public ActionResult DeleteBrand(int id)
        {
            _markaService.DeleteBrand(id);
            return Ok(new { message = "Product deleted successfully" });
        }

    }

}

