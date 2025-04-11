using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Business.BusinessAspects.Autofac;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }


        [PerformanceAspect(5)]
        public IDataResult<List<Product>> GetList()
        {
            //Thread.Sleep(5000);
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }

        [LogAspect(typeof(FileLogger))]
        [CacheAspect(duration: 10)]
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }

        [SecuredOperation("Admin, Restaurant")]
        public IDataResult<List<Product>> GetListByRestaurantId(int restaurantId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.RestaurantId == restaurantId).ToList());
        }

        [SecuredOperation("Admin, Restaurant")]
        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.Name)
                //,CheckIfCategoryIsEnabled()
                );

            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductNameExists(string productName)
        {

            var result = _productDal.GetList(p => p.Name == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        //private IResult CheckIfCategoryIsEnabled()
        //{
        //    var result = _categoryService.GetList();
        //    if (result.Data.Count<10)
        //    {
        //        return new ErrorResult(Messages.ProductNameAlreadyExists);
        //    }

        //    return new SuccessResult();
        //}

        [CacheRemoveAspect("IProductService.Get")]
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("Admin,Restaurant")]
        public IResult Delete(int productId)
        {
            //IDataResult<Product> result = (FindItemByName(product.ProductName));

            var product = _productDal.Get(p => p.ProductId == productId);

            product.IsDeleted = true;

            _productDal.SoftDelete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        [SecuredOperation("Admin,Restaurant")]
        public IResult Update(Product product)
        {

            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        public IDataResult<Dictionary<int, decimal>> GetProductsPrices(List<CartItem> items)
        {
            var products = _productDal.GetProductsPrices(items);
            return new SuccessDataResult<Dictionary<int, decimal>>(products);
        }

        public IDataResult<decimal> GetProductPrice(int productId)
        {
            var product = _productDal.Get(p => p.ProductId == productId);
            return new SuccessDataResult<decimal>(product.Price);
        }

        public IResult ToggleFeaturedProduct(Product product)
        {
            product.IsFeatured = (!product.IsFeatured);

            _productDal.Update(product);
            return new SuccessResult("FeaturedProduct successfully updated");
        }


        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
