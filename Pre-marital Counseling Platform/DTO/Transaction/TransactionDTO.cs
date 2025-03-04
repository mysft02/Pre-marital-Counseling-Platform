using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

    public class TransactionCreateDTO
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<TransactionCreateDTO, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDTO>().ReverseMap();
        }
    }
}
