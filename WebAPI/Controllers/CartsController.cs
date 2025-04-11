using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : Controller
    {
        private ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        private int GetTheUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            return int.Parse(userId);
        }

        [HttpGet("getcart")]
        public IActionResult GetCart()
        {
            int userId = GetTheUserId();

            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null)
            {
                return BadRequest("Cart not found");
            }
            return Ok(cart);
        }

        [HttpGet("getcartbyuserid")]
        public IActionResult GetCartByUserId()
        {
            int userId = GetTheUserId();
            var cart = _cartService.GetCartDtoByUserId(userId);

            if (cart == null)
            {
                return BadRequest("Cart not found");
            }
            return Ok(cart);
        }

        [HttpPost("createcart")]
        public IActionResult CreateCart()
        {
            int userId = GetTheUserId();

            var result = _cartService.CreateCart(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("additemtocart")]
        public IActionResult AddItemToCart(int productId, int quantity)
        {
            int userId = GetTheUserId();
            var result = _cartService.AddItemToCart(userId, productId, quantity);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("removeitemfromcart/{productId}")]
        public IActionResult RemoveItemFromCart(int productId)
        {
            int userId = GetTheUserId();
            var result = _cartService.RemoveItemFromCart(userId, productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("clearcart")]
        public IActionResult ClearCart()
        {
            int userId = GetTheUserId();
            var result = _cartService.ClearCart(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
