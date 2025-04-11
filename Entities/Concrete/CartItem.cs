using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class CartItem : IEntity
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int RestaurantId { get; set; }

        // Navigation Properties
        //public virtual Cart Cart { get; set; }

    }
}
