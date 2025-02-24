using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class SaveMemberAnswerDTO
    {
        public Guid AnswerId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuestionId { get; set; }
    }

    public class MemberAnswerProfile : Profile
    {
        public MemberAnswerProfile()
        {
            CreateMap<MemberAnswer, MemberAnswerDTO>();
            CreateMap<MemberAnswer, CreateMemberAnswerDTO>().ReverseMap();

        }
    }
}
