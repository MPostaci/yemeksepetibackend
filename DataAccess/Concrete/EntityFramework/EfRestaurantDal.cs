using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRestaurantDal : EfEntityRepositoryBase<Restaurant, YemekSepetiDBContext>, IRestaurantDal
    {
        public List<int> GetRestaurantIdByUserId(int userId)
        {
            using (var context = new YemekSepetiDBContext())
            {

                //var restaurantIds = context.RestaurantUsers.(r => r.UserId == userId).RestaurantId;
                var result = from ru in context.RestaurantUsers
                             where ru.UserId == userId
                             select ru.RestaurantId;
                return result.ToList();
            }
        }
        public List<Restaurant> GetRestaurantsByUserId(int userId)
        {
            using (var context = new YemekSepetiDBContext())
            {
                return (from r in context.Restaurants
                        join ru in context.RestaurantUsers on r.Id equals ru.RestaurantId
                        where ru.UserId == userId && r.IsDeleted != true
                        select new Restaurant
                        {
                            Id = r.Id,
                            Address = r.Address,
                            Image = r.Image,
                            Name = r.Name,
                            Phone = r.Phone
                        }).ToList();
            }
        }

        public List<RestaurantUser> GetRestaurantUsers()
        {
            using (var context = new YemekSepetiDBContext())
            {
                return (from ru in context.RestaurantUsers
                        select new RestaurantUser
                        {
                            Id = ru.Id,
                            RestaurantId = ru.RestaurantId,
                            UserId = ru.UserId,
                        }).ToList();
            }
        }

       
    }
}
