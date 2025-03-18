using AutoMapper;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.DTO.VnPay;
using SWP391.Infrastructure.DbContext;
using SWP391.Infrastructure.Repository;

namespace SWP391.Service
{
    public interface IPaymentService
    {
        Task<IActionResult> HandleCreateVNPayUrl(HttpContext context, VnPayRequestDTO vnPayRequestDTO, string userId, string url);
        Task<IActionResult> HandleVNPay(VnPayProcessDTO vnPayProcessDTO);
    }

    public class PaymentService : ControllerBase, IPaymentService
    {
        private readonly VnPayRepo _vnPayRepo;
        private readonly string? _vnpVersion;
        private readonly string? _vnpCommand;
        private readonly string? _vnpTmnCode;
        private readonly string? _vnpCurrCode;
        private readonly string? _vnpLocale;
        private readonly string? _vnpBaseUrl;
        private readonly string? _vnpHashSecret;
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public PaymentService(IConfiguration config, PmcsDbContext context, IMapper mapper)
        {
            _vnPayRepo = new VnPayRepo();
            _vnpVersion = config["VNPay:Version"];
            _vnpCommand = config["VNPay:Command"];
            _vnpTmnCode = config["VNPay:TmnCode"];
            _vnpCurrCode = config["VNPay:CurrCode"];
            _vnpLocale = config["VNPay:Locale"];
            _vnpBaseUrl = config["VNPay:BaseUrl"];
            _vnpHashSecret = config["VNPay:HashSecret"];
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleCreateVNPayUrl(HttpContext context, VnPayRequestDTO vnPayRequestDTO, string userId, string url)
        {
            try
            {
                var tick = DateTime.Now.Ticks.ToString();

                var user = _context.Users.FirstOrDefault(c => c.UserId.ToString() == userId);

                var vnpReturnUrl = "https://wed-wise-mu.vercel.app/home/wallet";

                _vnPayRepo.AddRequestData("vnp_Version", _vnpVersion);
                _vnPayRepo.AddRequestData("vnp_Command", _vnpCommand);
                _vnPayRepo.AddRequestData("vnp_TmnCode", _vnpTmnCode);
                _vnPayRepo.AddRequestData("vnp_Amount", (vnPayRequestDTO.Amount * 100).ToString());
                _vnPayRepo.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                _vnPayRepo.AddRequestData("vnp_CurrCode", _vnpCurrCode);
                _vnPayRepo.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
                _vnPayRepo.AddRequestData("vnp_Locale", _vnpLocale);
                _vnPayRepo.AddRequestData("vnp_OrderInfo", user.Email);
                _vnPayRepo.AddRequestData("vnp_OrderType", "other"); //default value: other
                _vnPayRepo.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
                _vnPayRepo.AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());

                var paymentUrl = _vnPayRepo.CreateRequestUrl(_vnpBaseUrl, _vnpHashSecret);

                return new OkObjectResult(new { Url = paymentUrl });

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        public async Task<IActionResult> HandleVNPay(VnPayProcessDTO vnPayProcessDTO)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(c => c.Email == vnPayProcessDTO.Email);

                var wallet = _context.Wallets
                    .FirstOrDefault(c => c.UserId == user.UserId);

                wallet.Balance += vnPayProcessDTO.Amount;
                _context.Wallets.Update(wallet);

                var transaction = new TransactionCreateDTO
                {
                    Amount = vnPayProcessDTO.Amount,
                    Description = "Topup Money"
                };

                var transactionMapped = _mapper.Map<Transaction>(transaction);
                transactionMapped.UpdatedBy = user.UserId;
                transactionMapped.CreatedBy = user.UserId;
                transactionMapped.UpdatedAt = DateTime.Now;
                transactionMapped.CreatedAt = DateTime.Now;

                _context.Transactions.Add(transactionMapped);

                _context.SaveChanges();

                return Ok("Update Balance Success!");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
