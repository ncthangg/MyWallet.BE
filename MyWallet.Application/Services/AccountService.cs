using MyWallet.Application.Contracts.IContext;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IUnitOfWork;
namespace MyWallet.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public AccountService(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userContext = userContext;
        }

        public async Task<GetAccountRes> GetAccountAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);

            if (account == null)
                throw new KeyNotFoundException($"Account {accountId} not found");

            return new GetAccountRes()
            {
                
            };
        }

        //public async Task<IEnumerable<GetAccountRes>> GetUserAccountsAsync(Guid userId)
        //{
        //    if (userId == Guid.Empty)
        //        throw new ArgumentException("Invalid user ID", nameof(userId));

        //    var accounts = await _unitOfWork.Accounts.GetByUserIdAsync(userId);
        //    return _mapper.Map<IEnumerable<GetAccountRes>>(accounts);
        //}

        public async Task<Guid> CreateAccountAsync(PostAccountReq request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Check duplicate
            bool exists = await _unitOfWork.Accounts.AccountNumberExistsAsync(
                request.UserId,
                request.AccountNumber
            );

            if (exists)
                throw new InvalidOperationException("Account number already exists");

            // Create entity
            var account = new Account
            {
                UserId = request.UserId,
                AccountNumber = request.AccountNumber.Trim(),
                AccountHolder = request.AccountHolder.Trim(),
                BankCode = request.BankCode.Trim(),
                BankName = request.BankName?.Trim(),
                AccountType = request.AccountType,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Validate domain entity
            if (!account.IsValidAccount())
                throw new InvalidOperationException("Invalid account");

            // Save
            await _unitOfWork.Accounts.AddAsync(account);

            return account.Id;
        }

        public async Task UpdateAccountAsync(Guid accountId, PutAccountReq request)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account == null)
                throw new KeyNotFoundException($"Account {accountId} not found");

            // Update fields
            account.AccountHolder = request.AccountHolder?.Trim() ?? account.AccountHolder;
            account.BankName = request.BankName?.Trim();
            account.UpdatedAt = DateTime.UtcNow;

            // Validate
            if (!account.IsValidAccount())
                throw new InvalidOperationException("Invalid account");

            await _unitOfWork.Accounts.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID", nameof(accountId));

            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account == null)
                throw new KeyNotFoundException($"Account {accountId} not found");

            await _unitOfWork.Accounts.DeleteAsync(accountId);
        }

        public Task<IEnumerable<GetAccountRes>> GetUserAccountsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
