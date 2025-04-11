using Core.Entities;

namespace Entities.Dtos
{
    public class CartItemDto : IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
    }

}
