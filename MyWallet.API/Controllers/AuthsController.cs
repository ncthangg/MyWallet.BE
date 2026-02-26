using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Application.Contracts.IServices;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;
using MyWallet.Domain.Constants;

namespace MyWallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleService _googleService;
        public AuthsController(IAuthService authService, IGoogleService googleService)
        {
            _authService = authService;
            _googleService = googleService;
        }

        [HttpGet("google-auth/signin")]
        public IActionResult SignIn([FromQuery] string origin)
        {
            if (string.IsNullOrWhiteSpace(origin))
                return BadRequest("Origin is required");

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(SignInGoogle), "auths",
                                          new { origin },
                                          Request.Scheme)
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }
        
        [HttpGet("google-auth/signin-google")]
        public async Task<IActionResult> SignInGoogle(string origin)
        {
            if (string.IsNullOrWhiteSpace(origin))
                return BadRequest("Origin is required");

            SignInGoogleRes result = await _authService.SignInGoogle(HttpContext);

            var response = new BaseResponseModel<SignInGoogleRes>(
                SuccessCode.Success,
                result,
                "Đăng nhập Google thành công!"
            );

            var html = _googleService.BuildSuccessHtml(response, origin);

            return Content(html, "text/html");
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            GetUserRes result = await _authService.Me();
            return Ok(new BaseResponseModel<GetUserRes>(
                               code: SuccessCode.Success,
                               message: null,
                               data: result
                               ));
        }
    }
}
