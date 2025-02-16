namespace SWP391.Domain
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }
        public Guid TherapistId { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public bool IsAvailable { get; set; }
        public Therapist Therapist { get; set; }
    }
}
