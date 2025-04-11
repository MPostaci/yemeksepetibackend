using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IRestaurantDal : IEntityRepository<Restaurant>
    {
        List<int> GetRestaurantIdByUserId(int userId);
        List<Restaurant> GetRestaurantsByUserId(int userId);
        List<RestaurantUser> GetRestaurantUsers();

    }
}

