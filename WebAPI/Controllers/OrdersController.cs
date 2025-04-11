using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;



namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
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

        [HttpGet("getordersbyuserid")]
        public ActionResult GetOrdersByUserId()
        {
            var result = _orderService.GetOrdersByUserId(GetTheUserId());
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("getusersrestaurantsorders")]
        public ActionResult GetUsersRestaurantsOrders()
        {
            var result = _orderService.GetUsersRestaurantsOrders(GetTheUserId());
            if (result != null)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPost("createorder")]
        public ActionResult CreateOrder()
        {
            var result = _orderService.CreateOrder(GetTheUserId());
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("changeorderstatus")]
        public ActionResult ChangeOrderStatus(int orderId, int statusId)
        {
            _orderService.ChangeOrderStatus(orderId, statusId);
            return Ok();
        }
        [HttpGet("getorderhistory")]
        public ActionResult GetOrderHistory(int orderId)
        {
            var result = _orderService.GetOrderHistory(orderId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


    }
}
