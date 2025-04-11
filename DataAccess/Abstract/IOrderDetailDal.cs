using Core.DataAccess;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IOrderDetailDal : IEntityRepository<OrderDetail>
    {
        void AddRange(List<OrderDetail> orderDetails);
    }
}
