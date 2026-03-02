using MyWallet.Application.Common.Mapper;
using MyWallet.Application.Contracts.IContext;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;
using MyWallet.Domain.Constants;
using MyWallet.Domain.Entities;
using MyWallet.Domain.Helper;
using MyWallet.Domain.Interface.IRepositories;
using MyWallet.Domain.Interface.IUnitOfWork;
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

        public async Task<PagingVM<GetBankInfoRes>> GetsAsync(int pageNumber, int pageSize, bool? isActive, string? searchValue)
        {
            //var isAdmin = _userContext.IsAdmin();

            //if (!isAdmin)
            //{
            //    throw new ApplicationException(ErrorCode.Unauthorized, ErrorMessages.Unauthorized);
            //}

            // Sử dụng custom repository method cho join phức tạp
            var (items, totalCount) = await _unitOfWork.BankInfos.GetBankInfosAsync(pageNumber,
                                                                      pageSize,
                                                                      isActive,
                                                                      searchValue);

            var userDict = await UserHelper.GetUserNameDictAsync((List<BankInfo>)items, _unitOfWork.Users);

            var list = items.Select(p => BankInfoMapper.ToGetBankInfoRes(p, userDict)).ToList();

            return new PagingVM<GetBankInfoRes>
            {
                List = list,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
        public async Task PostAsync(PostBankInfoReq req)
        {
            //Guid userId = _userContext.UserId
            //    ?? throw new ApplicationException(ErrorCode.Unauthorized, "User ID not found in context!");
            Guid userId = Guid.NewGuid();
            var bank = new BankInfo()
            {
                BankCode = req.BankCode,
                NapasCode = req.NapasCode,
                SwiftCode = req.SwiftCode,
                BankName = req.BankName,
                ShortName = req.ShortName,
                LogoUrl = req.LogoUrl,
                IsActive = req.IsActive,
            };
            bank.Initialize(_idGenerator.NewId(), userId);

            await _unitOfWork.BankInfos.AddAsync(bank);
        }
        public async Task PutAsync(Guid id, PutBankInfoReq req)
        {
            //Guid userId = _userContext.UserId
            //    ?? throw new ApplicationException(ErrorCode.Unauthorized, "User ID not found in context!");

            var oldItem = await _unitOfWork.BankInfos.GetByIdAsync(id)
               ?? throw new ApplicationException(ErrorCode.NotFound, ErrorMessages.EntityNotFound);

            oldItem.BankCode = req.BankCode;
            oldItem.NapasCode = req.NapasCode;
            oldItem.SwiftCode = req.SwiftCode;
            oldItem.BankName = req.BankName;
            oldItem.ShortName = req.ShortName;
            oldItem.LogoUrl = req.LogoUrl;
            oldItem.IsActive = req.IsActive;
            //oldItem.SetUpdated(userId);

            await _unitOfWork.BankInfos.UpdateAsync(oldItem);
        }
    }
}
