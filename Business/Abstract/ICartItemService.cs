using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICartItemService
    {
        List<CartItem> GetCartItems(int cartId);
        IResult AddToCart(CartItem cartItem);
        IResult RemoveFromCart(int cartItemId);
        IResult UpdateCartItemQuantity(int cartItemId, int newQuantity);
    }
}
