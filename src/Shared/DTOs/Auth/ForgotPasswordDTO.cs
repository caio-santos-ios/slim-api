namespace api_slim.src.Shared.DTOs.Auth
{
    public class ForgotPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
    }
}