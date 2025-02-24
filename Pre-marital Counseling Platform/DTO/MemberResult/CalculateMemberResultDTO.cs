using AutoMapper;
using Microsoft.Identity.Client;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class CalculateMemberResultDTO
    {
        public Guid QuizId { get; set; }
        public Guid MemberId { get; set; }
        public Guid QuizResultId { get; set; }
        public decimal Score { get; set; }
    }

    public class MemberResultProfile : Profile
    {
        public MemberResultProfile()
        {
            CreateMap<MemberResult, MemberResultDTO>();
            CreateMap<MemberResult, CreateMemberResultDTO>().ReverseMap();
        }
    }
}
