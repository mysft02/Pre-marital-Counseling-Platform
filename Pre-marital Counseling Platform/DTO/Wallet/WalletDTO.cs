using AutoMapper;
using SWP391.Domain;

namespace SWP391.DTO
{
    public class WalletDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }

    public class WalletCreateDTO
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }

    public class WalletResponseDTO
    {
        public Wallet Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDTO>();
            CreateMap<Wallet, WalletCreateDTO>().ReverseMap();
        }
    }
}
