using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class CartDto : IDto
    {
        public int CartId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public List<CartItemDto> Items { get; set; }
    }

}
