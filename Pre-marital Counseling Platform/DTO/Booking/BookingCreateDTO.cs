namespace SWP391.DTO
{
    public class BookingCreateDTO
    {
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
