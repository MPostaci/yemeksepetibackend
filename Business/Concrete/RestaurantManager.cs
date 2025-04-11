using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class RestaurantManager : IRestaurantService
    {
        IRestaurantDal _restaurantDal;

        public RestaurantManager(IRestaurantDal restaurantDal)
        {
            _restaurantDal = restaurantDal;
        }
        public IDataResult<List<Restaurant>> GetList()
        {
            return new SuccessDataResult<List<Restaurant>>(_restaurantDal.GetList().ToList());

        }

        public IDataResult<Restaurant> GetById(int restaurantId)
        {
            return new SuccessDataResult<Restaurant>(_restaurantDal.Get(r => r.Id == restaurantId));
        }

        [SecuredOperation("Restaurant.Add,Admin")]
        public IResult Add(Restaurant restaurant)
        {
            IResult result = BusinessRules.Run(CheckIfSameRestaurantExists(restaurant));

            if (result != null)
            {
                return result;
            }

            _restaurantDal.Add(restaurant);
            return new SuccessResult(Messages.RestaurantAdded);
        }

        private IResult CheckIfSameRestaurantExists(Restaurant restaurant)
        {
            var result = _restaurantDal.GetList(p => p.Name == restaurant.Name && restaurant.Address == p.Address).Any();
            if (result)
            {
                return new ErrorResult(Messages.RestaurantAlreadyExists);
            }

            return new SuccessResult();
        }

        //[SecuredOperation("Admin")]
        public IDataResult<List<RestaurantUser>> GetRestaurantUsers()
        {
            return new SuccessDataResult<List<RestaurantUser>>(_restaurantDal.GetRestaurantUsers());
        }

        [SecuredOperation("Restaurant.Delete,Admin")]
        public IResult Delete(int restaurantId)
        {
            var result = _restaurantDal.Get(p => p.Id == restaurantId);

            result.IsDeleted = true;

            _restaurantDal.SoftDelete(result);
            return new SuccessResult(Messages.RestaurantDeleted);
        }


        [SecuredOperation("Restaurant.Update,Admin")]
        public IResult Update(Restaurant restaurant)
        {
            _restaurantDal.Update(restaurant);
            return new SuccessResult(Messages.RestaurantUpdated);
        }

        public IDataResult<List<int>>GetRestaurantIdByUserId(int userId)
        {
            return new SuccessDataResult<List<int>>(_restaurantDal.GetRestaurantIdByUserId(userId));
        }
        public IDataResult<List<Restaurant>> GetRestaurantsByUserId(int userId)
        {
            return new SuccessDataResult<List<Restaurant>>(_restaurantDal.GetRestaurantsByUserId(userId));
        }

    }
}
