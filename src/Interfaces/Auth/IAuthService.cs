using api_slim.src.Models.Base;
using api_slim.src.Responses.Auth;
using api_slim.src.Shared.DTOs.Auth;

namespace api_slim.src.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ResponseApi<AuthResponse>> LoginAsync(LoginDTO request);
        Task<ResponseApi<AuthResponse>> RefreshTokenAsync(string token);
        Task<ResponseApi<api_slim.src.Models.User>> ResetPasswordAsync(ResetPasswordDTO request);
        Task<ResponseApi<api_slim.src.Models.User>> RequestForgotPasswordAsync(ForgotPasswordDTO request);
        Task<ResponseApi<api_slim.src.Models.User>> ForgotPasswordAsync(ResetPasswordDTO request);
    }
}