using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, YemekSepetiDBContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new YemekSepetiDBContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();

            }
        }

        public List<User> GetUsersOfCertainRole(string claimName)
        {
            using (var context = new YemekSepetiDBContext())
            {
                var result = from userOperationClaim in context.UserOperationClaims
                             join operationClaim in context.OperationClaims
                                 on userOperationClaim.OperationClaimId equals operationClaim.Id
                             join user in context.Users
                                 on userOperationClaim.UserId equals user.Id
                             where operationClaim.Name == claimName
                             select new User
                             {
                                 Id = user.Id,
                                 Name = user.Name,
                                 Email = user.Email
                             };
                return result.ToList();
            }
        }

    }
}
