using ElectricCartShop.API.DTOs;
using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseLogRepository _purchaseLogRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IPurchaseLogRepository purchaseLogRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _purchaseLogRepository = purchaseLogRepository;
        }

        public async Task<CartDto> GetCartAsync(string sessionId)
        {
            var cartItems = await _cartRepository.GetBySessionIdAsync(sessionId);
            var cartItemDtos = new List<CartItemDto>();

            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    cartItemDtos.Add(new CartItemDto
                    {
                        Id = item.Id,
                        Product = MapProductToDto(product),
                        Quantity = item.Quantity,
                        Size = item.Size
                    });
                }
            }

            return new CartDto
            {
                Items = cartItemDtos,
                TotalAmount = cartItemDtos.Sum(item => item.Product.Price * item.Quantity),
                TotalItems = cartItemDtos.Sum(item => item.Quantity)
            };
        }

        public async Task<CartItemDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            var product = await _productRepository.GetByIdAsync(addToCartDto.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {addToCartDto.ProductId} not found.");

            var existingItem = await _cartRepository.GetBySessionIdAsync(addToCartDto.SessionId);
            var existingCartItem = existingItem.FirstOrDefault(c => 
                c.ProductId == addToCartDto.ProductId && c.Size == addToCartDto.Size);

            CartItem cartItem;
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += addToCartDto.Quantity;
                cartItem = await _cartRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    Size = addToCartDto.Size,
                    SessionId = addToCartDto.SessionId
                };
                cartItem = await _cartRepository.CreateAsync(cartItem);
            }

            return new CartItemDto
            {
                Id = cartItem.Id,
                Product = MapProductToDto(product),
                Quantity = cartItem.Quantity,
                Size = cartItem.Size
            };
        }

        public async Task<CartItemDto> UpdateCartItemAsync(int id, UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _cartRepository.GetByIdAsync(id);
            if (cartItem == null)
                throw new ArgumentException($"Cart item with ID {id} not found.");

            cartItem.Quantity = updateCartItemDto.Quantity;
            var updatedItem = await _cartRepository.UpdateAsync(cartItem);
            var product = await _productRepository.GetByIdAsync(updatedItem.ProductId);

            return new CartItemDto
            {
                Id = updatedItem.Id,
                Product = MapProductToDto(product!),
                Quantity = updatedItem.Quantity,
                Size = updatedItem.Size
            };
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            return await _cartRepository.DeleteAsync(id);
        }

        public async Task<bool> ClearCartAsync(string sessionId)
        {
            try
            {
                return await _cartRepository.DeleteBySessionIdAsync(sessionId);
            }
            catch (Exception)
            {
                // Si hay un error, retornar true para indicar que el carrito está "limpio"
                return true;
            }
        }

        public async Task<CheckoutResponseDto> CheckoutAsync(CheckoutDto checkoutDto)
        {
            // Validate customer information
            if (string.IsNullOrWhiteSpace(checkoutDto.CustomerName) || 
                string.IsNullOrWhiteSpace(checkoutDto.CustomerEmail))
            {
                throw new ArgumentException("Customer name and email are required.");
            }

            // Get cart items
            var cart = await GetCartAsync(checkoutDto.SessionId);
            if (!cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            // Generate order number
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            // Create purchase log with simplified product data
            var simplifiedItems = cart.Items.Select(i => new 
            { 
                ProductId = i.Product.Id, 
                ProductName = i.Product.Name, 
                Quantity = i.Quantity, 
                UnitPrice = i.Product.Price,
                TotalPrice = i.Product.Price * i.Quantity,
                Size = i.Size
            }).ToList();

            var purchaseLog = new PurchaseLog
            {
                OrderNumber = orderNumber,
                CustomerEmail = checkoutDto.CustomerEmail,
                CustomerName = checkoutDto.CustomerName,
                TotalAmount = cart.TotalAmount,
                Status = "Completed",
                LogMessage = $"Purchase completed for {cart.Items.Count} items. Payment method: {checkoutDto.PaymentMethod}",
                AdditionalData = System.Text.Json.JsonSerializer.Serialize(new 
                { 
                    ShippingAddress = checkoutDto.ShippingAddress,
                    PaymentMethod = checkoutDto.PaymentMethod,
                    ItemCount = cart.Items.Count,
                    TotalQuantity = cart.TotalItems,
                    Items = simplifiedItems
                })
            };

            await _purchaseLogRepository.CreateAsync(purchaseLog);

            // Clear the cart
            await ClearCartAsync(checkoutDto.SessionId);

            // Return checkout response
            return new CheckoutResponseDto
            {
                OrderNumber = orderNumber,
                TotalAmount = cart.TotalAmount,
                Status = "Completed",
                PurchaseDate = DateTime.UtcNow,
                Items = cart.Items
            };
        }

        private static ProductDto MapProductToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
                Category = product.Category,
                Sizes = product.Sizes
            };
        }
    }
}


