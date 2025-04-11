using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;

namespace Business.Concrete
{
    public class OrderStatusManager : IOrderStatusService
    {
        // Siparis Iletildi
        // Siparis Hazirlaniyor
        // Siparis Yolda
        // Siparis Teslim Edildi
        // Siparis Onaylandi
        // Siparis Reddedildi

        //EfOrderStatusDal _orderStatusDal;

        //public OrderStatusManager(EfOrderStatusDal orderStatusDal)
        //{
        //    _orderStatusDal = orderStatusDal;
        //}

        //[SecuredOperation("Restaurant")]
        //public IResult UpdateOrderStatus(int orderId, int orderStatusId)
        //{


        //    _orderStatusDal.Update(orderId, orderStatusId);
        //    return new SuccessResult(Messages.ProductUpdated);
        //}
    }
}
