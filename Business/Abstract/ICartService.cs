using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICartService
    {
        Cart GetCartByUserId(int userId);
        CartDto GetCartDtoByUserId(int userId);
        IResult CreateCart(int userId);
        IResult AddItemToCart(int userId, int productId, int quantity);
        IResult RemoveItemFromCart(int userId, int productId);
        IResult ClearCart(int userId);
    }
}
