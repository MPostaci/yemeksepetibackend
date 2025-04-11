using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> orderDetails { get; set; }

    }
}
