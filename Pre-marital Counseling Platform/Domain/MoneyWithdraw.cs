using SWP391.Infrastructure.DataEnum;

namespace SWP391.Domain
{
    public class MoneyWithdraw : BaseEntities
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Money { get; set; }
        public WithdrawStatusEnum Status { get; set; }
        public User Customer { get; set; }
    }
}
