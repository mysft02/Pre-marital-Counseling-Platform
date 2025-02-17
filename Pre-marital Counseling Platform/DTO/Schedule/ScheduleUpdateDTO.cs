namespace SWP391.DTO.Schedule
{
    public class ScheduleUpdateDTO
    {
        public Guid ScheduleId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public bool IsAvailable { get; set; }
    }
}
