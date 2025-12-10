namespace api_slim.src.Shared.DTOs.User
{
    public class SaveUserPhotoDTO
    {
        public string Id { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }
}