using MongoDB.Driver;
using booking_backend.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace booking_backend.Helpers
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDBConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string is not configured.");
            }

            Console.WriteLine($"Attempting to connect to MongoDB with connection string: {connectionString}");

            // Configure MongoClient settings to use TLS 1.2
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoClientSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            try
            {

                var client = new MongoClient(mongoClientSettings);
                _database = client.GetDatabase("Booking_Db");
                Console.WriteLine("Successfully connected to MongoDB.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
                throw;
            }
        }
        public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("bookings");
    }
}
