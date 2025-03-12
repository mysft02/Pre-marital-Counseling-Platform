using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class QuizCreateDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class QuizResultCreateDTO
    {
        public decimal Score { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class QuizResultDTO
    {
        public Guid QuizResultId { get; set; }
        public Guid QuizId { get; set; }
        public decimal Score { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class QuizResultProfile : Profile
    {
        public QuizResultProfile()
        {
            CreateMap<QuizResultCreateDTO, QuizResult>().ReverseMap();
            CreateMap<QuizResult, QuizResultDTO>().ReverseMap();
        }
    }
}
