using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.Booking
{
    public class BookingUpdateDTO
    {
        public Guid BookingId { get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid MemberResultId { get; set; }
        public Guid SlotId { get; set; }
        public BookingStatusEnum Status { get; set; }
    }
}
