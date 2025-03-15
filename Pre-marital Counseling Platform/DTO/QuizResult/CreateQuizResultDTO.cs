using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class CreateQuizResultDTO
    {
        public Guid QuizId { get; set; }
        public decimal Score { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class UpdateQuizResultDTO
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
            CreateMap<QuizResult, CreateQuizResultDTO>().ReverseMap()
                .ForMember(x => x.QuizId, opt => opt.MapFrom(x => x.QuizId));
        }
    }

    public class UpdateQuizResultProfile : Profile
    {
        public UpdateQuizResultProfile()
        {
            CreateMap<QuizResult, UpdateQuizResultDTO>().ReverseMap();
            CreateMap<UpdateQuizResultDTO, QuizResult>().ReverseMap();
        }
    }
}
