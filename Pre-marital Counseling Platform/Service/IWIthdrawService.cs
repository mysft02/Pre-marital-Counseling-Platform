using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWP391.Domain;
using SWP391.DTO;
using SWP391.Infrastructure.DataEnum;
using SWP391.Infrastructure.DbContext;

namespace SWP391.Service
{
    public interface IWithdrawService
    {
        Task<IActionResult> HandleCreateWithdraw(WithdrawCreateDTO withdrawCreateDTO, string? userId);
        Task<IActionResult> HandleGetAllWithdraws();
        Task<IActionResult> HandleGetWithdrawByUserId(Guid id);
        Task<IActionResult> HandleUpdateWithdraw(WithdrawUpdateDTO withdrawUpdateDTO, string? userId);
    }

    public class WithdrawService : ControllerBase, IWithdrawService
    {
        private readonly PmcsDbContext _context;
        private readonly IMapper _mapper;

        public WithdrawService(PmcsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleCreateWithdraw(WithdrawCreateDTO withdrawCreateDTO, string? userId)
        {
            try
            {
                var withdraw = new WithdrawDTO
                {
                    CustomerId = withdrawCreateDTO.CustomerId,
                    Money = withdrawCreateDTO.Money,
                };

                var wallet = _context.Wallets.FirstOrDefault(x => x.UserId == withdraw.CustomerId);

                if(wallet.Balance < withdraw.Money)
                {
                    return BadRequest("Not enough balance");
                }

                wallet.Balance -= withdraw.Money;
                _context.Wallets.Update(wallet);
                var transaction = new TransactionCreateDTO
                {
                    Amount = -withdraw.Money,
                    Description = "Withdraw Money"
                };

                var transMapped = _mapper.Map<Transaction>(transaction);
                transMapped.CreatedBy = withdraw.CustomerId;
                transMapped.CreatedAt = DateTime.Now;
                transMapped.UpdatedAt = DateTime.Now;
                transMapped.UpdatedBy = withdraw.CustomerId;
                _context.Transactions.Add(transMapped);

                var withdrawMapped = _mapper.Map<MoneyWithdraw>(withdraw);
                withdrawMapped.CreatedBy = Guid.Parse(userId);
                withdrawMapped.CreatedAt = DateTime.Now;
                withdrawMapped.UpdatedBy = Guid.Parse(userId);
                withdrawMapped.UpdatedAt = DateTime.Now;

                _context.MoneyWithdraws.Add(withdrawMapped);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return Ok(withdraw);
                }
                else
                {
                    return BadRequest("Create failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetAllWithdraws()
        {
            try
            {
                List<MoneyWithdraw> withdraws = new List<MoneyWithdraw>();
                withdraws = _context.MoneyWithdraws
                    .ToList();

                return Ok(withdraws);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleGetWithdrawByUserId(Guid id)
        {
            try
            {
                var withdraw = _context.MoneyWithdraws
                    .Where(c => c.CustomerId == id)
                    .ToList();

                return Ok(withdraw);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleUpdateWithdraw(WithdrawUpdateDTO withdrawUpdateDTO, string? userId)
        {
            try
            {
                var withdraw = _context.MoneyWithdraws
                    .FirstOrDefault(x => x.Id == withdrawUpdateDTO.Id);

                if(withdraw.Status != WithdrawStatusEnum.Pending)
                {
                    return BadRequest("Withdraw is not pending");
                }

                if (withdrawUpdateDTO.Status == WithdrawStatusEnum.Rejected)
                {
                    var wallet = _context.Wallets
                        .FirstOrDefault(x => x.UserId == withdraw.CustomerId);

                    wallet.Balance += withdraw.Money;
                    _context.Wallets.Update(wallet);

                    var transaction = new TransactionCreateDTO
                    {
                        Amount = withdraw.Money,
                        Description = "Cancel Withdraw Money"
                    };
                    
                    var transMapped = _mapper.Map<Transaction>(transaction);
                    transMapped.CreatedBy = Guid.Parse(userId);
                    transMapped.CreatedAt = DateTime.Now;
                    transMapped.UpdatedAt = DateTime.Now;
                    transMapped.UpdatedBy = Guid.Parse(userId);
                    _context.Transactions.Add(transMapped);
                }

                withdraw.Status = withdrawUpdateDTO.Status;
                withdraw.UpdatedBy = Guid.Parse(userId);
                withdraw.UpdatedAt = DateTime.Now;

                _context.MoneyWithdraws.Update(withdraw);

                if (_context.SaveChanges() > 0)
                {
                    return Ok(withdraw);
                }
                else
                {
                    return BadRequest("Update failed");
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
