using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;
        private ICartService _cartService;
        private IOrderDetailService _orderDetailService;
        private IProductService _productService;

        public OrderManager(IOrderDal orderDal, ICartService cartService, IOrderDetailService orderDetailService, IProductService productService)
        {
            _orderDal = orderDal;
            _cartService = cartService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }

        public IDataResult<List<Order>> GetOrdersByUserId(int userId)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetOrdersByUserId(userId));
        }

        public IDataResult<List<Order>> GetUsersRestaurantsOrders(int userId)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetUsersRestaurantsOrders(userId));
        }


        public IResult CreateOrder(int userId)
        {
            var cart = _cartService.GetCartDtoByUserId(userId);

            CreateOrderTransaction(cart, userId);

            return new SuccessResult("Order created");
        }


        [TransactionScopeAspect]
        private void CreateOrderTransaction(CartDto cart, int userId)
        {
            decimal totalAmount = 0;

            foreach (var item in cart.Items)
            {
                totalAmount += item.ProductPrice * item.Quantity;
            }

            var order = _orderDal.OrderAdd(new Order
            {
                UserId = userId,
                RestaurantId = cart.RestaurantId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount
            });


            var orderDetails = cart.Items.Select(i => new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.ProductPrice
            }).ToList();

            _orderDetailService.AddOrderDetails(orderDetails);



            _cartService.ClearCart(userId);
        }

        public void ChangeOrderStatus(int orderId, int statusId)
        {
            _orderDal.UpdateOrderStatus(orderId, statusId);
        }

        public List<OrderBridgeStatus> GetOrderHistory(int orderId)
        {
            return _orderDal.GetOrderStatusHistory(orderId);
        }

    }
}
