namespace SWP391.DTO.Feedback
{
    public class FeedbackDTO
    {
        public Guid FeedbackId { get; set; }
        public Guid BookingId { get; set; }
        public decimal Rating { get; set; }
        public string FeedbackTitle { get; set; }
        public string FeedbackContent { get; set; }
        public bool IsSatisfied { get; set; }
    }
}
