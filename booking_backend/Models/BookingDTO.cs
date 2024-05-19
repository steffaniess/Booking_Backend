namespace booking_backend.Models
{
    public class BookingDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public bool IsBooked { get; set; }
    }
}
