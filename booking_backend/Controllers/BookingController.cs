using booking_backend.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using booking_backend.Helpers;
using booking_backend.Services;
using MongoDB.Bson;
using System.Collections.Generic;

namespace booking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly MongoDBContext _dbContext;
        private readonly IEmailService _emailService;

        public BookingsController(MongoDBContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var booking = new Booking
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Email = bookingDTO.Email,
                    Name = bookingDTO.Name,
                    Date = bookingDTO.Date,
                    Location = bookingDTO.Location,
                    IsBooked = true
                };

                await _dbContext.Bookings.InsertOneAsync(booking);
                await _emailService.SendEmailAsync(booking.Email, "Booking Confirmation", $"Your booking is confirmed for {booking.Date} at {booking.Location}.");
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the booking: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(string id)
        {
            try {

                var booking = await _dbContext.Bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
                if (booking == null)
                {
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the booking: {ex.Message}");
            }
        }

        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes([FromQuery] DateTime date, [FromQuery] string location)
        {
            try
            {

                var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                var endOfDay = startOfDay.AddDays(1);

                var filter = Builders<Booking>.Filter.And(
                    Builders<Booking>.Filter.Eq("location", location),
                    Builders<Booking>.Filter.Gte("date", startOfDay),
                    Builders<Booking>.Filter.Lt("date", endOfDay)
                );

                var allTimes = GetAllPossibleTimes(); // Get a list of all possible times in a day.
                var bookedTimes = await _dbContext.Bookings.Find(filter).Project(b => b.Date.TimeOfDay).ToListAsync();
                var availableTimes = allTimes.Except(bookedTimes);

                return Ok(availableTimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving available times: {ex.Message}");
            }
        }

        private List<TimeSpan> GetAllPossibleTimes()
        {
            var times = new List<TimeSpan>();
            for (int hour = 9; hour < 17; hour++)
            {
                times.Add(new TimeSpan(hour, 0, 0));
                times.Add(new TimeSpan(hour, 30, 0));
            }
            return times;
        }
    }
}
