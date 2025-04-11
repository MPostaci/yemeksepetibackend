using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class OrderBridgeStatus : IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        //public Order Order { get; set; }
        //public OrderStatus Status { get; set; }
    }
}
