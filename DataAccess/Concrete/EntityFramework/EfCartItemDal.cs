using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCartItemDal : EfEntityRepositoryBase<CartItem, YemekSepetiDBContext>, ICartItemDal
    {
    }
}
