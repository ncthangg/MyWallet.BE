using Microsoft.Extensions.Configuration;
using MyWallet.Application.Contracts.ISubServices;
using MyWallet.Application.DTOs.Response;
using MyWallet.Application.DTOs.Response.Base;
using System.Text.Json;

namespace MyWallet.Infrastructure.SubService
{
    public class GoogleService : IGoogleService
    {
        private readonly string[] _allowedOrigins;

        public GoogleService(IConfiguration config)
        {
            _allowedOrigins = config.GetSection("Auth:AllowedOrigins").Get<string[]>()
                            ?? [];
        }

        public string BuildSuccessHtml(BaseResponseModel<SignInGoogleRes> response, string origin)
        {
            if (!_allowedOrigins.Contains(origin))
            {
                throw new UnauthorizedAccessException("Invalid origin");
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // 3) Build HTML
            return $@"
        <html>
            <body>
                <script>
                    window.opener.postMessage({json}, '{origin}');
                    window.close();
                </script>
            </body>
        </html>
    ";
        }
    }

}
