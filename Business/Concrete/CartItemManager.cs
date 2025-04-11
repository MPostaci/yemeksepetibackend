using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CartItemManager : ICartItemService
    {
        private ICartItemDal _cartItemDal;

        public CartItemManager(ICartItemDal cartItemDal)
        {
            _cartItemDal = cartItemDal;
        }

        public List<CartItem> GetCartItems(int cartId)
        {
            return _cartItemDal.GetList(ci => ci.CartId == cartId).ToList();
        }

        public IResult AddToCart(CartItem cartItem)
        {
            _cartItemDal.Add(cartItem);
            return new SuccessResult("Item added to cart");
        }

        public IResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _cartItemDal.Get(ci => ci.CartItemId == cartItemId);
            _cartItemDal.Delete(cartItem);
            return new SuccessResult("Item removed from cart");
        }
        public IResult UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            var cartItem = _cartItemDal.Get(ci => ci.CartItemId == cartItemId);
            cartItem.Quantity = newQuantity;
            _cartItemDal.Update(cartItem);
            return new SuccessResult("Cart item quantity updated");
        }
    }
}
