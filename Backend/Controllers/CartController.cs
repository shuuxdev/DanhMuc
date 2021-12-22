
using System;
using System.Threading.Tasks;
using Backend.Attributes;
using Backend.Entities;
using Backend.Models.Request;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api")]
    public class CartController : Controller
    {
        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public CartService _cartService { get; }



        [Authorize]
        [HttpGet("cart")]
        public IActionResult GetCart()
        {
            string userId = HttpContext.Items["User"].ToString();
            return Json(_cartService.GetCart(userId));
        }

        [Authorize]
        [HttpPost("cart/checkout")]
        public IActionResult GetCartItemsById([FromBody] int[] list)
        {
            string userId = HttpContext.Items["User"].ToString();

            return Json(_cartService.GetItems(list));
        }
        [Authorize]
        [HttpPost("cart")]
        public IActionResult AddItemToCart([FromQuery] int productId)
        {
            CartItem cart = new CartItem();
            cart.product_id = productId;
            string userId = HttpContext.Items["User"].ToString();
            var result = _cartService.Add(cart, userId);

            if (result != "ok")
            {
                return BadRequest($"{result} row added");
            }
            return Ok($"{result} rows added");
        }
        [Authorize]
        [HttpPost("cart/cartwithtype")]
        public IActionResult AddItemToCart([FromBody] CartItem item)
        {
            string userId = HttpContext.Items["User"].ToString();
            var result = _cartService.Add(item, userId);
            if (result != "ok")
            {
                return BadRequest($"{result} row added");
            }
            return Ok($"{result} rows added");
        }

        [HttpDelete("cart")]
        [Authorize]
        public IActionResult RemoveItemsFromCart([FromBody] int[] list)
        {
            string userId = HttpContext.Items["User"].ToString();
            var result = _cartService.RemoveList(list, userId);
            if (result == 0)
            {
                return BadRequest($"{result} rows deleted");
            }
            return Ok($"{result} rows deleted");
        }
        [Authorize]
        [HttpPost("cart/quantity")]
        public IActionResult UpdateQuantity([FromBody] Quantity quantity)
        {
            var userId = HttpContext.Items["User"].ToString();
            var result = _cartService.UpdateQuantity(quantity);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }

}