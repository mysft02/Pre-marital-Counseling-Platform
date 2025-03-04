namespace SWP391.DTO.VnPay
{
    public class VnPayRequestDTO
    {
        public decimal Amount { get; set; }
    }

    public class VnPayProcessDTO
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
    }
}
