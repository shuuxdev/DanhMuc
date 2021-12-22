using System.Threading.Tasks;
using Backend.Models.Request;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/product")]
    public class ProductController : Controller
    {
        private ProductService _productService { get; set; }

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public IActionResult GetProducts([FromQuery] Page setting)
        {
            var products = _productService.GetProducts(setting);
            return Json(products);
        }
        [HttpGet("async")]
        public async Task<IActionResult> GetProductsAsync([FromQuery] Page setting)
        {
            var products = await _productService.GetProductsAsync(setting);
            return Json(products);
        }
        [HttpGet("item")]
        public IActionResult GetProduct(int itemid)
        {
            var product = _productService.GetProduct(itemid);
            return Json(product);
        }
        [HttpGet("search")]
        public IActionResult SearchProduct([FromQuery] string keyword)
        {
            var products = _productService.SearchProduct(keyword);
            return Json(products);
        }
    }
}