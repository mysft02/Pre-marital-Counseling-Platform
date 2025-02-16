namespace SWP391.Domain
{
    public class BookingResult
    {
        public Guid BookingResultId { get; set; }
        public Guid BookingId { get; set; }
        public string Description { get; set; }
        public Booking Booking { get; set; }
    }
}
