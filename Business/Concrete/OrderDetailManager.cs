using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class OrderDetailManager : IOrderDetailService
    {
        private IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public IResult AddOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailDal.Add(orderDetail);
            return new SuccessResult("Order detail added");
        }

        public IResult AddOrderDetails(List<OrderDetail> orderDetails)
        {
            _orderDetailDal.AddRange(orderDetails);
            return new SuccessResult("Order details added");
        }
    }
}
