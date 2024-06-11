using DemoApi.Models;
using DemoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly IProductService _productService;

        public UrunController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public ActionResult GetProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("GetProduct/{id}")]
        public ActionResult GetProduct(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("AddProduct")]
        public ActionResult AddProduct(Urunler product)
        {
            _productService.AddProduct(product);
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("UpdateProduct")]
        public ActionResult UpdateProduct(Urunler product)
        {
            _productService.UpdateProduct(product);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("DeleteProduct/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("GetUrunlerWithMarka")]
        public ActionResult GetUrunlerWithMarka()
        {
            var urunlerWithMarka = _productService.GetUrunlerWithMarka();
            return Ok(urunlerWithMarka);
        }
    }
}
