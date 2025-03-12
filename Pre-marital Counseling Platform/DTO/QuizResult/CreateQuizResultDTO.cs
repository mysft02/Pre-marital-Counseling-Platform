using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class CreateQuizResultDTO
    {
        public Guid QuizResultId { get; set; }
        public Guid QuizId { get; set; }
        public decimal Score { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateQuizResultProfile : Profile
    {
        public CreateQuizResultProfile()
        {
            CreateMap<QuizResult, CreateQuizResultDTO>().ReverseMap();
            CreateMap<CreateQuizResultDTO, QuizResult>().ReverseMap();
        }
    }
}
