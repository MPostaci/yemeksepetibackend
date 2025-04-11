using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        List<Order> GetOrdersByUserId(int userId);
        List<Order> GetUsersRestaurantsOrders(int userId);
        Order OrderAdd(Order order);
        void UpdateOrderStatus(int orderId, int statusId);
        List<OrderBridgeStatus> GetOrderStatusHistory(int orderId);

    }
}
