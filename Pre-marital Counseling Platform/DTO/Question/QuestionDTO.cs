using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class QuestionDTO
    {
        public Guid QuestionId { get; set; }
        public Guid QuizId { get; set; }
        public string QuestionContent { get; set; }
        public QuestionStatusEnum QuestionStatus { get; set; }
    }

    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDTO>().ReverseMap();
            CreateMap<QuestionCreateDTO, Question>().ReverseMap();
        }
    }
}
