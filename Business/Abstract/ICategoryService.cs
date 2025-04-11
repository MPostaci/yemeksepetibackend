﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetList();
        IResult Add(Category category);
        IResult Delete(int categoryId);
    }
}
