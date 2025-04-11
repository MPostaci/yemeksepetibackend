using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        private ICartItemService _cartItemService;
        private IProductService _productService;

        public CartManager(ICartDal cartDal, ICartItemService cartItemService, IProductService productService)
        {
            _cartDal = cartDal;
            _cartItemService = cartItemService;
            _productService = productService;
        }

        public Cart GetCartByUserId(int userId)
        {
            return _cartDal.GetCartByUserId(userId);
        }

        public CartDto GetCartDtoByUserId(int userId)
        {
            return _cartDal.GetCartDtoByUserId(userId);
        }

        public IResult CreateCart(int userId)
        {
            IResult result = BusinessRules.Run(CheckIfCartAlreadyCreated(userId));

            if (result != null)
            {
                return result;
            }

            _cartDal.Add(new Cart { UserId = userId });
            return new SuccessResult("Cart created");
        }

        private IResult CheckIfCartAlreadyCreated(int userId)
        {
            _cartDal.Get(c => c.UserId == userId);
            if (_cartDal.Get(c => c.UserId == userId) != null)
            {
                return new ErrorResult("Cart already created");
            }
            return new SuccessResult();
        }

        public IResult AddItemToCart(int userId, int productId, int quantity)
        {
            var cart = _cartDal.GetCartByUserId(userId);
            if (cart == null)
                throw new Exception("Cart not found");

            var existingItem = _cartItemService.GetCartItems(cart.CartId)
                .FirstOrDefault(ci => ci.ProductId == productId);

            int newRestaurantId = _cartDal.GetRestaurantIdByProductId(productId);

            if (existingItem != null && existingItem.RestaurantId != newRestaurantId)
            {
                return new ErrorResult("You can't add items from different restaurants to the same cart");
            }
            else if (existingItem != null && existingItem.RestaurantId == newRestaurantId)
            {
                _cartItemService.UpdateCartItemQuantity(existingItem.CartItemId, quantity);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    RestaurantId = newRestaurantId,
                };
                _cartItemService.AddToCart(cartItem);
            }

            cart.LastUpdatedAt = DateTime.Now;
            _cartDal.Update(cart);
            return new SuccessResult();

        }

        //public IResult ExtractItemFromCart()
        //{
        //    var cart = _cartItemService.
        //}
        public IResult RemoveItemFromCart(int userId, int productId)
        {
            var cart = _cartDal.GetCartByUserId(userId);
            if (cart == null)
                return new ErrorResult("Cart not found");

            var existingItem = _cartItemService.GetCartItems(cart.CartId)
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem == null)
                return new ErrorResult("Item not found in cart");

            _cartItemService.RemoveFromCart(existingItem.CartItemId);
            cart.LastUpdatedAt = DateTime.Now;
            _cartDal.Update(cart);
            return new SuccessResult("Item removed from cart");
        }

        public IResult ClearCart(int userId)
        {
            var cart = _cartDal.GetCartByUserId(userId);
            if (cart == null)
                return new ErrorResult("Cart not found");

            var cartItems = _cartItemService.GetCartItems(cart.CartId);
            foreach (var item in cartItems)
            {
                _cartItemService.RemoveFromCart(item.CartItemId);
            }
            cart.LastUpdatedAt = DateTime.Now;
            _cartDal.Update(cart);
            return new SuccessResult("Cart cleared");
        }
    }
}
