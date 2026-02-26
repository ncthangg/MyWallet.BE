using MyWallet.Application.Contracts.IContext;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.DTOs.Request;
using MyWallet.Domain.Constants;
using MyWallet.Domain.Entities;
using MyWallet.Domain.Interface.IUnitOfWork;
using System.Reflection.Emit;
using ApplicationException = MyWallet.Application.Exceptions.ApplicationException;

namespace MyWallet.Application.Services
{
    public class BankInfoService : IBankInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IIdGenerator _idGenerator;

        public BankInfoService(IUnitOfWork unitOfWork, IUserContext userContext, IIdGenerator idGenerator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userContext = userContext;
            _idGenerator = idGenerator;
        }

        public async Task PostAsync(PostBankInfoReq req)
        {
            Guid userId = _userContext.UserId
                ?? throw new ApplicationException(ErrorCode.Unauthorized, "User ID not found in context!");

            var bank = new BankInfo()
            {
                BankCode = req.BankCode,
                NapasCode = req.NapasCode,
                SwiftCode = req.SwiftCode,
                BankName = req.BankName,
                ShortName = req.ShortName,
                LogoUrl = req.LogoUrl,
            };
            bank.Initialize(_idGenerator.NewId(), userId);

            await _unitOfWork.BankInfos.AddAsync(bank);
        }
        public async Task PutAsync(Guid id, PostBankInfoReq req)
        {
            Guid userId = _userContext.UserId
                ?? throw new ApplicationException(ErrorCode.Unauthorized, "User ID not found in context!");

            var oldItem = await _unitOfWork.BankInfos.GetByIdAsync(id)
               ?? throw new ApplicationException(ErrorCode.NotFound, ErrorMessages.EntityNotFound);

            oldItem.BankCode = req.BankCode;
            oldItem.NapasCode = req.NapasCode;
            oldItem.SwiftCode = req.SwiftCode;
            oldItem.BankName = req.BankName;
            oldItem.ShortName = req.ShortName;
            oldItem.LogoUrl = req.LogoUrl;
            oldItem.IsActive = req.IsActive;
            oldItem.SetUpdated(userId);

            await _unitOfWork.BankInfos.UpdateAsync(oldItem);
        }
    }
}
