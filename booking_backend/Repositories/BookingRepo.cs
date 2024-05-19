using booking_backend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace booking_backend.Repositories
{
    public class BookingRepo
    {
        private readonly IMongoCollection<Booking> _bookings;

        public BookingRepo(IMongoClient client)
        {
            var database = client.GetDatabase("bookingDb");
            _bookings = database.GetCollection<Booking>("bookings");
        }

        public async Task<List<Booking>> GetBookingsByDateAndLocation(DateTime date, string location)
        {
            // Skapa ett datumintervall för att hitta bokningar under hela dagen
            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);
            var endOfDay = startOfDay.AddDays(1);

            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Gte(b => b.Date, startOfDay),
                Builders<Booking>.Filter.Lt(b => b.Date, endOfDay),
                Builders<Booking>.Filter.Eq(b => b.Location, location)
            );

            return await _bookings.Find(filter).ToListAsync();
        }

        public async Task CreateBooking(Booking booking)
        {
            await _bookings.InsertOneAsync(booking);
        }
    }
}
