using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class AnswerDTO
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string? AnswerContent { get; set; }
        public decimal Score { get; set; }
    }

    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDTO>().ReverseMap();
            CreateMap<Answer, CreateAnswerDTO>().ReverseMap();
            CreateMap<Answer, AnswerCreateDTO>().ReverseMap();
        }
    }
}
