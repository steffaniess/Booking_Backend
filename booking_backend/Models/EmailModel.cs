using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace booking_backend.Models
{
    public class EmailModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }
    }
}
