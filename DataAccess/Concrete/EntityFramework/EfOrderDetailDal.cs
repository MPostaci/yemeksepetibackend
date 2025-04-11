using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOrderDetailDal : EfEntityRepositoryBase<OrderDetail, YemekSepetiDBContext>, IOrderDetailDal
    {

        private YemekSepetiDBContext _context;

        public EfOrderDetailDal(YemekSepetiDBContext context)
        {
            _context = context;
        }
        public void AddRange(List<OrderDetail> orderDetails)
        {
            foreach(var item in orderDetails)
            {
                var addedEntity = _context.Entry(item);
                addedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();
            }

            return;
            /*
            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();*/
        }
    }
}
