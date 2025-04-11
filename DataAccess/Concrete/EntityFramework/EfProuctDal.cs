using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, YemekSepetiDBContext>, IProductDal
    {
        YemekSepetiDBContext _context;

        public EfProductDal(YemekSepetiDBContext context)
        {
            _context = context;
        }

        public Dictionary<int, decimal> GetProductsPrices(List<CartItem> items)
        {

            return _context.Products
                .Where(p => items.Select(i => i.ProductId).Contains(p.ProductId))
                .ToDictionary(p => p.ProductId, p => p.Price);
        }

        public decimal GetProductPrice(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId).Price;
        }

    }
}
