using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO.Booking
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid MemberResultId { get; set; }
        public Guid ScheduleId { get; set; }
        public BookingStatusEnum Status { get; set; }
    }

    
}

public class BookingReturnDTO
{
    public Booking Booking { get; set; }
    public string Message { get; set; }
}
