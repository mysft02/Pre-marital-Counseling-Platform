namespace SWP391.DTO
{
    public class ScheduleCreateDTO
    {
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public bool IsAvailable { get; set; }
    }
}
