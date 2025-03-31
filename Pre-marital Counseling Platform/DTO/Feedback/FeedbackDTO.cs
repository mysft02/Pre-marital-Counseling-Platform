using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
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

    public class FeedBackReturnDTO
    {
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public Guid FeedbackId { get; set; }
        public Guid BookingId { get; set; }
        public decimal Rating { get; set; }
        public string FeedbackTitle { get; set; }
        public string FeedbackContent { get; set; }
        public bool IsSatisfied { get; set; }
    }

    public class FeedbackProfile : Profile{
        public FeedbackProfile(){
            CreateMap<Feedback, FeedbackDTO>();
            CreateMap<FeedbackDTO, Feedback>().ReverseMap();
        }
    }
}
