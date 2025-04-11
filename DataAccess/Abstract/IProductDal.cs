using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductDal:IEntityRepository<Product>
    {
        Dictionary<int, decimal> GetProductsPrices(List<CartItem> items);
        decimal GetProductPrice(int productId);

    }
}
