using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RestaurantUserManager : IRestaurantUserService
    {
        IRestaurantUserDal _restaurantUserDal;

        public RestaurantUserManager(IRestaurantUserDal restaurantUserDal)
        {
            _restaurantUserDal = restaurantUserDal;
        }

        public IDataResult<IEnumerable<RestaurantUser>> GetRestaurantUsers()
        {
            var restaurantUsers = _restaurantUserDal.GetList();

            if(restaurantUsers != null)
            {
                return new SuccessDataResult<IEnumerable<RestaurantUser>>(restaurantUsers);
            }
            return new ErrorDataResult<IEnumerable<RestaurantUser>>(restaurantUsers, "Failed to fetch restaurant users");
        }

        public IResult Add(int userId, int restaurantId)
        {
            var restUser = new RestaurantUser
            {
                UserId = userId,
                RestaurantId = restaurantId
            };

            _restaurantUserDal.Add(restUser);
            return new SuccessResult("Restoran User added succesfully");
        }

        public IResult Update(int userId, int restaurantId)
        {
            var restaurantUser = _restaurantUserDal.Get(r => r.RestaurantId == restaurantId);

            restaurantUser.UserId = userId;

            _restaurantUserDal.Update(restaurantUser);
            return new SuccessResult("Restoran User added succesfully");
        }
    }
}
