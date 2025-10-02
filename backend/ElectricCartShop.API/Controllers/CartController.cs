using ElectricCartShop.API.DTOs;
using ElectricCartShop.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectricCartShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<CartDto>> GetCart(string sessionId)
        {
            var cart = await _cartService.GetCartAsync(sessionId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartItemDto>> AddToCart(AddToCartDto addToCartDto)
        {
            try
            {
                var cartItem = await _cartService.AddToCartAsync(addToCartDto);
                return Ok(cartItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int id, UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                var cartItem = await _cartService.UpdateCartItemAsync(id, updateCartItemDto);
                return Ok(cartItem);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var result = await _cartService.RemoveFromCartAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("clear/{sessionId}")]
        public async Task<ActionResult> ClearCart(string sessionId)
        {
            var result = await _cartService.ClearCartAsync(sessionId);
            // Siempre devolver NoContent, incluso si no había items que eliminar
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutResponseDto>> Checkout(CheckoutDto checkoutDto)
        {
            try
            {
                var result = await _cartService.CheckoutAsync(checkoutDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


