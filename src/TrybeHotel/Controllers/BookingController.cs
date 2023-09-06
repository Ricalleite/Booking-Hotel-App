using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var bookingResult = _repository.Add(bookingInsert, userEmail);

                return Created("New booking done!", bookingResult);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "client")]
        public IActionResult GetBooking(int Bookingid)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var bookingSearch = _repository.GetBooking(Bookingid, userEmail);

                if (bookingSearch == null) 
                {
                    return Unauthorized();
                }
                return Ok(bookingSearch);
            }
            catch (Exception) 
            {
                return BadRequest("Request not possible!");
            }
        }
    }
}
