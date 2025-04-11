using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Core.Extensions;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Dtos;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private readonly IRestaurantService _restaurantService;
        private Cloudinary _cloudinary;

        public ProductsController(IProductService productService, Cloudinary cloudinary, IRestaurantService restaurantService)
        {
            _productService = productService;
            _restaurantService = restaurantService;
            _cloudinary = cloudinary;

        }

        [HttpGet("getall")]
        //[Authorize(Roles = "Product.List")]
        public IActionResult GetList()
        {
            
            var result = _productService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("getlistbyrestaurantid")]
        public IActionResult GetListByRestaurantId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var restaurantIds = _restaurantService.GetRestaurantIdByUserId(Convert.ToInt32(userId));
            List<Product> result = new List<Product>();
            foreach (int restaurantId in restaurantIds.Data)
            {
                var products = _productService.GetListByRestaurantId(restaurantId);
                foreach (var product in products.Data)
                {
                    result.Add(product);
                }

            }

            if (restaurantIds.Success)
            {
                return Ok(result);
            }
            return BadRequest("Couldnt fetch products by restaurantids");
        }

        [HttpGet("getlistbycategory")]
        public IActionResult GetListByCategory(int categoryId)
        {
            var result = _productService.GetListByCategory(categoryId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int productId)
        {
            var result = _productService.GetById(productId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        //[HttpPost("add")]
        //public IActionResult Add(Product product)
        //{
        //    var result = _productService.Add(product);
        //    if (result.Success)
        //    {
        //        return Ok(result.Message);
        //    }

        //    return BadRequest(result.Message);
        //}
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] ProductCreateDto productDto)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(productDto.ImageFile.FileName, productDto.ImageFile.OpenReadStream()),
                // Optionally, add a transformation (resize, crop, etc.)
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //var restaurantId = _restaurantService.GetRestaurantIdByUserId(Convert.ToInt32(userId));

            var product = new Product
            {
                Name = productDto.Name,
                RestaurantId = productDto.RestaurantId,
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
                Price = productDto.Price,
                IsFeatured = productDto.IsFeatured,
                Image = uploadResult.SecureUri.ToString() // Save the Cloudinary URL here
            };

            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Product product)
        {
            var result = _productService.Update(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("delete/{productId}")]
        public IActionResult Delete(int productId)
        {
            var result = _productService.Delete(productId);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("togglefeaturedproduct/{productId}")]
        public IActionResult ToggleFeaturedProduct(int productId)
        {
            var product = _productService.GetById(productId);

            var result = _productService.ToggleFeaturedProduct(product.Data);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("transaction")]
        public IActionResult TransactionTest(Product product)
        {
            var result = _productService.TransactionalOperation(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

    }
}