using SWP391.Domain;

namespace SWP391.DTO
{
    public class TherapistDTO
    {
        public Guid TherapistId { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}
