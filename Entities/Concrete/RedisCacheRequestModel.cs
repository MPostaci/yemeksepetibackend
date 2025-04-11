using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class RedisCacheRequestModel : IEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
