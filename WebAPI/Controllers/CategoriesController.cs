using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        private Cloudinary _cloudinary;

        public CategoriesController(ICategoryService categoryService, Cloudinary cloudinary)
        {
            _categoryService = categoryService;
            _cloudinary = cloudinary;
        }

        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _categoryService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] CategoryCreateDto categoryCreateDto)
        {
            if (categoryCreateDto == null)
                return BadRequest("CategoryDto not found");

            if (categoryCreateDto.ImageFile == null || categoryCreateDto.ImageFile.Length == 0)
                return BadRequest("Image file is required");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(categoryCreateDto.ImageFile.FileName, categoryCreateDto.ImageFile.OpenReadStream()),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            var category = new Category
            {
                CategoryName = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                Image = uploadResult.SecureUri.ToString()
            };

            var result = _categoryService.Add(category);
            if (result.Success)
            {
                return Ok(result.Message); 
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete/{categoryId}")]
        public IActionResult Delete(int categoryId)
        {
            var result = _categoryService.Delete(categoryId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}