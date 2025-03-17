using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public Therapist Therapist { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
