using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetOrdersByUserId(int userId);
        IResult CreateOrder(int userId);
        void ChangeOrderStatus(int orderId, int statusId);
        List<OrderBridgeStatus> GetOrderHistory(int orderId);
        IDataResult<List<Order>> GetUsersRestaurantsOrders(int userId);

    }
}
