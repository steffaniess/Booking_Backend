using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace booking_backend.Models
{
    public class EmailModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string Id { get; set; }

        [BsonElement("name")]
        [SwaggerSchema("Namn för avsändare")]
        public string Name { get; set; }

        [BsonElement("email")]
        [SwaggerSchema("E-postadress för avsändare")]
        public string Email { get; set; }

        [BsonElement("message")]
        [SwaggerSchema("Meddelande att skicka")]
        public string Message { get; set; }
    }
}