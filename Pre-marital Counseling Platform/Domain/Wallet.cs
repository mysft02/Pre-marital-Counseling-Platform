namespace SWP391.Domain
{
    public class Wallet
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
    }
}
