using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCartDal : EfEntityRepositoryBase<Cart, YemekSepetiDBContext>, ICartDal
    {

        public Cart GetCartByUserId(int userId)
        {
           // using (var context = new YemekSepetiDBContext())
            {
                return _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefault(c => c.UserId == userId);
            }
        }

        private YemekSepetiDBContext _context;
        //restoran id, her bir itemin productid quantity productprice

        public EfCartDal(YemekSepetiDBContext context)
        {
            _context = context;
        }
        public CartDto GetCartDtoByUserId(int userId)
        {
            var cartItemsQuery = from cartItem in _context.CartItems.AsNoTracking()
                                 join cart in _context.Carts.AsNoTracking() on cartItem.CartId equals cart.CartId
                                 join product in _context.Products.AsNoTracking() on cartItem.ProductId equals product.ProductId
                                 join restaurant in _context.Restaurants.AsNoTracking() on product.RestaurantId equals restaurant.Id
                                 where cart.UserId == userId
                                 select new { cart, cartItem, product, restaurant };

            var itemsList = cartItemsQuery.Select(x => new CartItemDto
            {
                ProductId = x.product.ProductId,
                ProductName = x.product.Name,
                ProductPrice = x.product.Price,
                ProductDescription = x.product.Description,
                Quantity = x.cartItem.Quantity,
                ProductImage = x.product.Image,
            }).ToList();

            var firstResult = cartItemsQuery.FirstOrDefault();
            if (firstResult == null)
                return null; // or handle empty cart accordingly

            var cartDto = new CartDto
            {
                CartId = firstResult.cart.CartId,
                RestaurantId = firstResult.restaurant.Id,
                RestaurantName = firstResult.restaurant.Name,
                Items = itemsList
            };

            return cartDto;


        }
        public int GetRestaurantIdByProductId(int productId)
        {
            using (var context = new YemekSepetiDBContext())
            {
                return context.Products.FirstOrDefault(p => p.ProductId == productId).RestaurantId;
            }
        }

        public string GetRestaurantNameByRestaurantId(int restaurantId)
        {
            using (var context = new YemekSepetiDBContext())
            {
                return context.Restaurants.FirstOrDefault(p => p.Id == restaurantId).Name;
            }
        }
    }
}
