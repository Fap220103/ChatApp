using CloudinaryDotNet.Actions;

namespace ChatApp_Api.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publidId);
    }
}
