using AutoMapper;
using SWP391.Domain;
using SWP391.Infrastructure.DataEnum;

namespace SWP391.DTO
{
    public class WithdrawDTO
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Money { get; set; }
        public WithdrawStatusEnum Status { get; set; }
    }

    public class WithdrawCreateDTO
    {
        public Guid CustomerId { get; set; }
        public decimal Money { get; set; }
    }

    public class WithdrawUpdateDTO
    {
        public Guid Id { get; set; }
        public WithdrawStatusEnum Status { get; set; }
    }

    public class MoneyWithdrawProfile : Profile
    {
        public MoneyWithdrawProfile()
        {
            CreateMap<MoneyWithdraw, WithdrawDTO>().ReverseMap();
            CreateMap<WithdrawCreateDTO, MoneyWithdraw>().ReverseMap();
            CreateMap<WithdrawUpdateDTO, MoneyWithdraw>().ReverseMap();
        }
    }
}
