namespace SWP391.DTO.Booking
{
    public class BookingCreateDTO
    {
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid MemberResultId { get; set; }
        public Guid SlotId { get; set; }
    }
}
