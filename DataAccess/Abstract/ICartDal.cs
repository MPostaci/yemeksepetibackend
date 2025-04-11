using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface ICartDal : IEntityRepository<Cart>
    {
        Cart GetCartByUserId(int userId);

        CartDto GetCartDtoByUserId(int userId);
        int GetRestaurantIdByProductId(int productId);
    }
}
