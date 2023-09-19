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
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
            string? userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            var bookingToAdd = _repository.Add(bookingInsert, userEmail!);

            if(bookingToAdd == null) {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }

            return Created("", bookingToAdd);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
            string? userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            var response = _repository.GetBooking(Bookingid, userEmail!);

            if (response == null) {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}