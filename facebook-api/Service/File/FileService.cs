
using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.Enum;
using System.IO;

namespace facebook_api.Service.File
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public MediaType getMediaType(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).TrimStart('.').ToUpper();

            foreach(var videoExtension in VideoExtensionType.GetValues(typeof(VideoExtensionType)))
            {
                if (videoExtension.ToString() == fileExtension)
                    return MediaType.VIDEO;
            }

            foreach(var imageExtension in ImageExtensionType.GetValues(typeof(ImageExtensionType)))
            {
                if(imageExtension.ToString() == fileExtension)
                    return MediaType.IMAGE;
            }

            throw new Exception("Unsupported media type");
        }

        public UploadInfo uploadFile(IFormFile file, ImageSourceType imageSourceType, int userID)
        {
            var mediaType = getMediaType(file);

            string relativePath = Path.Combine(
                mediaType.ToString(),
                imageSourceType.ToString(),
                userID.ToString()
            );

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            
            if(!Directory.Exists(rootPath))
            { 
                Directory.CreateDirectory(rootPath); 
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using(var stream = new FileStream(Path.Combine(rootPath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return new UploadInfo { FileName = fileName, MediaType = mediaType.ToString().ToLower() };
        }

        public void deleteFile(string fileName, string mediaType, ImageSourceType imageSourceType, int userID)
        {
            var media = convertStringToMediaType(mediaType);

            if (media != MediaType.NONE)
            {
                string relativePath = Path.Combine(
                    media.ToString(), 
                    imageSourceType.ToString(), 
                    userID.ToString(), 
                    fileName
                );

                string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(rootPath)) {
                    System.IO.File.Delete(rootPath); 
                }
            }
        }

        public string getFilePath(string fileName, string mediaType, ImageSourceType imageSourceType)
        {
            var media = convertStringToMediaType(mediaType);

            if (media == MediaType.NONE) 
            { 
                return string.Empty; 
            }
            var filePath = Path.Combine(mediaType.ToString().ToLower(), imageSourceType.ToString().ToLower(), fileName);
            return filePath;
        }

        public MediaType convertStringToMediaType(string mediaType)
        {
            if(string.IsNullOrWhiteSpace(mediaType))
            {
                return MediaType.NONE;
            }

            if(Enum.TryParse<MediaType>(mediaType.ToUpper(), out MediaType result))
            {
                return result;
            }

            return MediaType.NONE;
        }
    }
}
