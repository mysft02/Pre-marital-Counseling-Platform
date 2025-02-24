namespace SWP391.Domain
{
    public class Therapist : BaseEntities
    {
        public Guid TherapistId { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public decimal ConsultationFee { get; set; }
        public List<Schedule>? Schedules { get; set; }
        public List<TherapistSpecification> Specialty { get; set; }
    }
}
