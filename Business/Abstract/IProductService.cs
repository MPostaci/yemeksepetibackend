using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<Product> GetById(int productId);
        IDataResult<List<Product>> GetList();
        IDataResult<List<Product>> GetListByRestaurantId(int restaurantId);
        IDataResult<List<Product>> GetListByCategory(int categoryId);
        IResult Add(Product product);
        IResult Delete(int productId);
        IResult Update(Product product);
        IDataResult<Dictionary<int, decimal>> GetProductsPrices(List<CartItem> items);
        IDataResult<decimal> GetProductPrice(int productId);
        IResult ToggleFeaturedProduct(Product product);
        IResult TransactionalOperation(Product product);

    }
}
