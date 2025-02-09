namespace SWP391.Domain
{
    public class TherapistSpecification
    {
        public Guid TherapistId { get; set; }
        public Guid SpecificationId { get; set; }
        public Therapist Therapist { get; set; }
        public Specification Specification { get; set; }
    }
}
