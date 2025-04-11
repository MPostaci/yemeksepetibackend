using Business.Abstract;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantService _restaurantService;
        private IRestaurantUserService _restaurantUserService;
        private readonly Cloudinary _cloudinary;

        public RestaurantsController(IRestaurantService restaurantService, Cloudinary cloudinary, IRestaurantUserService restaurantUserService)
        {
            _restaurantService = restaurantService;
            _cloudinary = cloudinary;
            _restaurantUserService = restaurantUserService;
        }

        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _restaurantService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] RestaurantCreateDto restaurantDto)
        {
            if (restaurantDto == null)
                return BadRequest("Invalid restaurant data.");

            // Ensure a file was provided
            if (restaurantDto.ImageFile == null || restaurantDto.ImageFile.Length == 0)
                return BadRequest("Image file is required.");

            // Upload the image to Cloudinary
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(restaurantDto.ImageFile.FileName, restaurantDto.ImageFile.OpenReadStream()),
                // Optionally, add a transformation (resize, crop, etc.)
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };    

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Create your restaurant instance
            var restaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                Address = restaurantDto.Address,
                Phone = restaurantDto.Phone,
                Image = uploadResult.SecureUri.ToString() // Save the Cloudinary URL here
            };

            var result = _restaurantService.Add(restaurant);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("update")]
        public IActionResult Update(Restaurant restaurant)
        {
            var result = _restaurantService.Update(restaurant);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("delete/{restaurantId}")]
        public IActionResult Delete(int restaurantId)
        {
            var result = _restaurantService.Delete(restaurantId);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("getusersrestaurants")]
        public IActionResult getUsersRestaurants()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _restaurantService.GetRestaurantsByUserId(Convert.ToInt32(userId));

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet("getrestaurantusers")]
        public IActionResult GetRestaurantUsers()
        {
            var result = _restaurantService.GetRestaurantUsers();

            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
        }

        // ------------------------------------------

        [HttpGet("getrestaurantuserslist")]
        public IActionResult GetRestaurantUsersList()
        {
            var result = _restaurantUserService.GetRestaurantUsers();

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }
        [HttpPost("addrestaurantuser")]
        public IActionResult SetRestaurantToUser(int userId, int restaurantId)
        {
            var result = _restaurantUserService.Add(userId, restaurantId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
        }
        [HttpPost("updaterestaurantuser")]
        public IActionResult UpdateRestaurantUser(int userId, int restaurantId)
        {
            var result = _restaurantUserService.Update(userId, restaurantId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
        }

    }
}


