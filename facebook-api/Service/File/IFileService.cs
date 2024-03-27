using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.Enum;

namespace facebook_api.Service.File
{
    public interface IFileService
    {
        MediaType getMediaType(IFormFile file);
        UploadInfo uploadFile(IFormFile file, ImageSourceType imageSource, int userID);
        string getFilePath(string fileName, string mediaType, ImageSourceType imageSourceType);
        MediaType convertStringToMediaType(string mediaType);
        void deleteFile(string fileName, string mediaType, ImageSourceType imageSourceType, int userID);
    }
}
