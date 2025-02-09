namespace SWP391.Domain
{
    public class Transaction : BaseEntities
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
