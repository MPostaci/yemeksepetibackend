using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IRestaurantUserService
    {
        IDataResult<IEnumerable<RestaurantUser>> GetRestaurantUsers();
        IResult Add(int userId, int restaurantId);
        IResult Update(int userId, int restaurantId);

    }
}
