using facebook_api.Models.DTO.Request.Comment;
using facebook_api.Models.DTO.Response.Comment;
using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Repository.Comment;
using facebook_api.Service.File;

namespace facebook_api.Service.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IFileService _fileService;
        public CommentService(ICommentRepository commentRepository, IFileService fileService)
        {
            _commentRepository = commentRepository;
            _fileService = fileService;
        }

        public async Task<IEnumerable<CommentResponse>> GetAll(int postID)
        {
            var comments = await _commentRepository.GetAll(postID);
            return comments.Select(comment => new CommentResponse
            {
                ID = comment.ID,
                PostID = comment.PostID,
                UserID = comment.UserID,
                Comment = comment.Comment,
                MediaType = comment.MediaType,
                FilePath = _fileService.getFilePath(comment.FileName, comment.MediaType, ImageSourceType.COMMENT),
                UpdatedAt = comment.UpdatedAt,
                CreatedAt = comment.CreatedAt,
                TotalLikes = comment.TotalLikes,
            });
        }

        public async Task Delete(int commentID)
        {
            var comment = await _commentRepository.GetByID(commentID);
            if (comment != null)
            {
                await _commentRepository.Delete(commentID);
                if (comment.MediaType != null)
                {
                    _fileService.deleteFile(comment.FileName, comment.MediaType, ImageSourceType.COMMENT, comment.UserID);
                }
            }
        }

        public async Task Create(CreateCommentRequest upsertCommentRequest)
        {
            UploadInfo uploadInfo = new UploadInfo();

            if (upsertCommentRequest.File != null)
            {
                uploadInfo = _fileService.uploadFile(upsertCommentRequest.File, ImageSourceType.COMMENT, upsertCommentRequest.UserID);
            }

            var comment = new CommentEntity
            {
                PostID = upsertCommentRequest.PostID,
                UserID = upsertCommentRequest.UserID,
                Comment = upsertCommentRequest.Comment,
                MediaType = uploadInfo.MediaType,
                FileName = uploadInfo.FileName,
            };

            await _commentRepository.Create(comment);
        }

        public async Task Update(int commentID, UpdateCommentRequest updateCommentRequest)
        {
            var uploadInfo = new UploadInfo();
            var comment = await _commentRepository.GetByID(commentID);

            if (updateCommentRequest.File != null)
            {
                _fileService.deleteFile(comment.FileName, comment.MediaType, ImageSourceType.COMMENT, comment.UserID);
                uploadInfo = _fileService.uploadFile(updateCommentRequest.File, ImageSourceType.COMMENT, comment.UserID);
            }

            var updatedComment = new CommentEntity
            {
                ID = commentID,
                Comment = updateCommentRequest.Comment,
                MediaType = uploadInfo.MediaType ?? comment.MediaType,
                FileName = uploadInfo.FileName ?? comment.FileName,
            };

            await _commentRepository.Update(updatedComment);
        }
    }
}
