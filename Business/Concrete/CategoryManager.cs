using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().ToList());
        }

        public IResult Add(Category category)
        {
            _categoryDal.Add(category);
            return new SuccessResult("Category created successfully");
        }

        public IResult Delete(int categoryId)
        {
            var category = _categoryDal.Get(p => p.CategoryId == categoryId);
            
            category.IsDeleted = true;

            _categoryDal.SoftDelete(category);
            return new SuccessResult("CategoryDeletedSuccessfully");
        }
    }
}
