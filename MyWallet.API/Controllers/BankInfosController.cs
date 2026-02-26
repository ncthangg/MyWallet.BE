using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.DTOs.Request;
using MyWallet.Application.DTOs.Response.Base;
using MyWallet.Application.Services;
using MyWallet.Domain.Constants;
using MyWallet.Domain.Interface.IRepositories;

namespace MyWallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankInfosController : ControllerBase
    {
        private readonly IBankInfoService _bankInfoService;
        public BankInfosController(IBankInfoService bankInfoService)
        {
            _bankInfoService = bankInfoService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PostBankInfoReq request)
        {
            await _bankInfoService.PostAsync(request);
            return Ok(new BaseResponseModel<string>(
                code: SuccessCode.Success,
                data: null,
                message: SuccessMessages.CreateSuccess));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] PostBankInfoReq request)
        {
            await _bankInfoService.PutAsync(id, request);
            return Ok(new BaseResponseModel<string>(
               code: SuccessCode.Success,
               data: null,
               message: SuccessMessages.UpdateSuccess));
        }
        //[Authorize]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> SoftDelete(string id)
        //{
        //    await _baseService.SoftDeleteAsync(id);
        //    return Ok(new BaseResponseModel<string>(
        //      code: SuccessCode.Success,
        //      data: null,
        //      message: SuccessMessages.DeleteSuccess));
        //}
        //[Authorize]
        //[HttpDelete("{id}/hard")]
        //public async Task<IActionResult> HardDelete(string id)
        //{
        //    await _baseService.HardDeleteAsync(id);
        //    return Ok(new BaseResponseModel<string>(
        //      code: SuccessCode.Success,
        //      data: null,
        //      message: SuccessMessages.DeleteForeverSuccess));
        //}
    }
}