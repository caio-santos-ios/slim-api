using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace api_slim.src.Handlers
{
    public class CloudinaryHandler
    {
        private readonly string CloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL") ?? "";

        public async Task<string> Upload(string filePath, string folderProject, string folderModel)
        {
            Cloudinary cloudinary = new(CloudinaryUrl);
            cloudinary.Api.Secure = true;

            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(filePath),
                Folder = $"{folderProject}/{folderModel}"
            };

            ImageUploadResult result = await cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString() ?? string.Empty;
        }
        
        public async Task<bool> Delete(string publicId, string folderProject, string folderModel)
        {
            Cloudinary cloudinary = new(CloudinaryUrl);
            cloudinary.Api.Secure = true;

            DeletionParams deletionParams = new ($"{folderProject}/{folderModel}/{publicId}");
            DeletionResult result = await cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }
    }
}