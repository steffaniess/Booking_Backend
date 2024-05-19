using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace booking_backend.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("isBooked")]
        public bool IsBooked { get; set; }

    }

    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("capacity")]
        public int Capacity { get; set; }
    }
}
