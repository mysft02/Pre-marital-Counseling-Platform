namespace SWP391.Domain
{
    public class Booking : BaseEntities
    {
        public Guid BookingId{ get; set; }
        public Guid MemberId { get; set; }
        public Guid TherapistId { get; set; }
        public Guid MemberResultId { get; set; }
        public Guid SlotId { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public Therapist Therapist { get; set; }
        public MemberResult MemberResult { get; set; }
        public Schedule Slot { get; set; }
    }
}
