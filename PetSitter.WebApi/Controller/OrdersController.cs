using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models.DTO;
using PetSitter.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetSitter.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderService orderService, IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto checkoutRequest)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            try
            {
                var paymentLinkInfo = await _orderService.CreateOrderAndInitiatePayment(checkoutRequest, userId);
                if (paymentLinkInfo == null || string.IsNullOrEmpty(paymentLinkInfo.checkoutUrl))
                {
                    return BadRequest(new { message = "Could not create payment link." });
                }

                // Trả về toàn bộ object hoặc chỉ checkoutUrl tùy theo nhu cầu của frontend
                return Ok(paymentLinkInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            try
            {
                var order = await _orderRepository.FindByIdAsync(orderId);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                if (order.UserId != userId)
                {
                    return Forbid();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpGet("getAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            //var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (!Guid.TryParse(userIdString, out var userId))
            //{
            //    return Unauthorized(new { message = "Invalid user token." });
            //}

            try
            {
                var orders = await _orderRepository.GetAllOrderAsync();
                var ordersDto = orders.Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    ShopId = o.OrderItems.FirstOrDefault().Product.Shop.ShopId,
                    ShopName = o.OrderItems.FirstOrDefault().Product.Shop.ShopName,
                    OrderCode = o.OrderCode,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ShippingAddress = o.ShippingAddress,
                    CreatedAt = o.CreatedAt,
                });
                
                
                if (orders == null)
                {
                    return NotFound(new { message = "Orders are empty" });
                }


                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}