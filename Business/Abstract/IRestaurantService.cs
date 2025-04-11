using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IRestaurantService
    {
        IDataResult<Restaurant> GetById(int restaurantId);
        IDataResult<List<Restaurant>> GetList();
        IResult Add(Restaurant restaurant);
        IResult Delete(int restaurantId);
        IResult Update(Restaurant restaurant);
        IDataResult<List<int>> GetRestaurantIdByUserId(int userId);
        IDataResult<List<Restaurant>> GetRestaurantsByUserId(int userId);
        IDataResult <List<RestaurantUser>> GetRestaurantUsers();


    }
}
