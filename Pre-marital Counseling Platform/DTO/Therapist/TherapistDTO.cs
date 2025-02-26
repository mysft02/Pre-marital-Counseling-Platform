using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class TherapistDTO
    {
        public Guid TherapistId { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }
        public decimal ConsultationFee { get; set; }
        public string MeetUrl { get; set; }
    }

    public class TherapistProfile : Profile
    {
        public TherapistProfile()
        {
            CreateMap<Therapist, TherapistDTO>();
            CreateMap<TherapistCreateDTO, Therapist>().ReverseMap();
        }
    }
}
