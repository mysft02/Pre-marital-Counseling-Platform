using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class ScheduleUpdateDTO
    {
        public Guid ScheduleId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public ScheduleStatusEnum Status { get; set; }
    }
}
