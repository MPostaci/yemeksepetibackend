using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class RestaurantCreateDto : IDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
