using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
namespace DataAccess.Concrete.EntityFramework
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, YemekSepetiDBContext>, IOrderDal
    {
        private YemekSepetiDBContext _context;

        public EfOrderDal(YemekSepetiDBContext context)
        {
            _context = context;
        }
        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Where(o => o.OrderId == orderId)
                .FirstOrDefault();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            using (var context = new YemekSepetiDBContext())
            {
                //    var result = from o in context.Orders
                //                 join od in context.OrderDetails on o.OrderId equals od.OrderId
                //                 join obs in context.OrderBridgeStatuses on o.OrderId equals obs.OrderId
                //                 join os in context.OrderStatuses on obs.OrderStatusId equals os.Id
                //                 where o.UserId == userId
                //                 select new List<Order>
                //                 {
                //                     new Order
                //                     {
                //                         StatusHistory = 
                //                     }
                //                 };
                var result = context.Orders
                                   .Where(o => o.UserId == userId)
                                   .Include(o => o.StatusHistory)
                                   .Include(o => o.OrderDetails)
                                   .ToList();
            return result;
        }


        }
        public List<Order> GetUsersRestaurantsOrders(int userId)
        {
            using (var context = new YemekSepetiDBContext())
            {
                var orderQuery = from o in context.Orders
                                 join ru in context.RestaurantUsers on o.RestaurantId equals ru.RestaurantId
                                 where ru.UserId == userId
                                 select new { o };

                var result = orderQuery.Select(x => new Order
                {
                    OrderId = x.o.OrderId,
                    UserId = x.o.UserId,
                    RestaurantId = x.o.RestaurantId,
                    TotalAmount = x.o.TotalAmount,
                    OrderDate = x.o.OrderDate,
                    OrderDetails = x.o.OrderDetails,
                    StatusHistory = x.o.StatusHistory
                }).ToList();

                return result;
            }
        }


        public Order OrderAdd(Order order)
        {
            var addedEntity = _context.Entry(order);
            addedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();

            var initialStatus = new OrderBridgeStatus
            {
                OrderId = order.OrderId,
                OrderStatusId = 1
            };

            _context.OrderBridgeStatuses.Add(initialStatus);
            _context.SaveChanges();
            return order;

        }

        public void UpdateOrderStatus(int orderId, int orderStatusId)
        {
            var statusUpdate = new OrderBridgeStatus
            {
                OrderId = orderId,
                OrderStatusId = orderStatusId
            };
            _context.OrderBridgeStatuses.Add(statusUpdate);
            _context.SaveChanges();
        }

        public List<OrderBridgeStatus> GetOrderStatusHistory(int orderId)
        {
            return _context.OrderBridgeStatuses
                .Where(osb => osb.OrderId == orderId)
                //.Include(osb => osb.Order)
                //.Include(osb => osb.Status) // Include related OrderStatus
                .OrderByDescending(osb => osb.ChangedAt)
                .ToList();
        }
        //public OrderStatus GetOrderStatus(int orderId)
        //{

        //    var result = from orderStatus in _context.OrderStatuses
        //                    join orderBridgeStatus in _context.OrderBridgeStatuses
        //                        on orderStatus.Id equals orderBridgeStatus.OrderStatusId
        //                    where orderBridgeStatus.OrderId == orderId
        //                    select new OrderStatus { Id = orderStatus.Id, Name = orderStatus.Name };
        //    return result.FirstOrDefault();

        //}

    }
}
