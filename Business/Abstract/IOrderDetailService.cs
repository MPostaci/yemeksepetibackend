using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IOrderDetailService
    {

        IResult AddOrderDetail(OrderDetail orderDetail);
        IResult AddOrderDetails(List<OrderDetail> orderDetails);
    }
}
