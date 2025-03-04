namespace SWP391.DTO
{
    public class TherapistUpdateDTO
    {
        public Guid TherapistId { get; set; }
        public string TherapistName { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public decimal ConsultationFee { get; set; }
        public string MeetUrl { get; set; }
    }
}
