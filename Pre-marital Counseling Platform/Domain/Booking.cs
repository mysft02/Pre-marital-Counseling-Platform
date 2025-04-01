using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class Booking : BaseEntities
    {
        public Guid BookingId{ get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid ScheduleId { get; set; }
        public BookingStatusEnum Status { get; set; }
        public decimal Fee { get; set; }
        public decimal Commission { get; set; }
        public User User { get; set; }
        public Therapist Therapist { get; set; }
        public Schedule Schedule { get; set; }
        public Feedback Feedback { get; set; }
        public BookingResult BookingResult { get; set; }
    }
}
