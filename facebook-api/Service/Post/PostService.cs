using facebook_api.Models.DTO.Request.Post;
using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.DTO.Response.Post;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Repository.Post;
using facebook_api.Service.File;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;

namespace facebook_api.Service.Post
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IFileService _fileService;

        public PostService(IPostRepository postRepository, IFileService fileService)
        {
            _postRepository = postRepository;
            _fileService = fileService;
        }

        public async Task<bool> CheckFriendshipStatus(int sourceID, int userID)
        {
            return await _postRepository.CheckFriendshipStatus(sourceID, userID);
        }

        public async Task Delete(int postID)
        {
            var post = await _postRepository.GetByID(postID);
            if (post != null)
            {
                await _postRepository.Delete(postID);
                if (post.MediaType != null)
                {
                    _fileService.deleteFile(post.FileName, post.MediaType, ImageSourceType.POST, post.UserID);
                }
            }
        }

        public async Task<IEnumerable<PostResponse>> GetFeed(int userID)
        {
            var posts = await _postRepository.GetFeed(userID);
            return posts.Select(post => new PostResponse
            {
                ID = post.ID,
                UserID = post.UserID,
                Message = post.Message,
                MediaType = post.MediaType,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                FilePath = _fileService.getFilePath(post.FileName, post.MediaType, ImageSourceType.POST),
                TotalComments = post.TotalComments,
                TotalLikes = post.TotalLikes,
                UserLastLike = post.UserLastLike
            });
        }

        public async Task<UserProfilePostsResponse> GetUserProfilePosts(int sourceID, int userID)
        {
            var postResponse = Enumerable.Empty<PostResponse>();
            bool areFriends = await _postRepository.CheckFriendshipStatus(sourceID, userID);
            if (areFriends) 
            {
                var posts = await _postRepository.GetUserProfilePosts(userID);
                postResponse = posts.Select(post => new PostResponse
                {
                    ID = post.ID,
                    UserID = post.UserID,
                    Message = post.Message,
                    MediaType = post.MediaType,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    FilePath = _fileService.getFilePath(post.FileName, post.MediaType, ImageSourceType.POST),
                    TotalComments = post.TotalComments,
                    TotalLikes = post.TotalLikes,
                    UserLastLike = post.UserLastLike
                });
            }

            return new UserProfilePostsResponse { AreFriends = areFriends, Posts = postResponse };
        }

        public async Task Create(UpsertPostRequest upsertPostRequest)
        {
            UploadInfo uploadInfo = new UploadInfo();
            if (upsertPostRequest.File != null)
            {
                uploadInfo = _fileService.uploadFile(upsertPostRequest.File, ImageSourceType.POST, upsertPostRequest.UserID);
            }

            var post = new PostEntity
            {
                UserID = upsertPostRequest.UserID,
                Message = upsertPostRequest.Message,
                MediaType = uploadInfo.MediaType,
                FileName = uploadInfo.FileName
            };

            await _postRepository.Upsert(post);
        }

        public async Task Update(int postID, UpsertPostRequest upsertPostRequest)
        {
            var uploadInfo = new UploadInfo();
            var post = await _postRepository.GetByID(postID);

            if (upsertPostRequest.File != null)
            {
                _fileService.deleteFile(post.FileName, post.MediaType, ImageSourceType.POST, post.UserID);
                uploadInfo = _fileService.uploadFile(upsertPostRequest.File, ImageSourceType.POST, upsertPostRequest.UserID);
            }

            var updatedPost = new PostEntity
            {
                ID = postID,
                UserID = upsertPostRequest.UserID,
                Message = upsertPostRequest.Message,
                MediaType = uploadInfo.MediaType ?? post.MediaType,
                FileName = uploadInfo.FileName ?? post.FileName
            };

            await _postRepository.Upsert(updatedPost);
        }
    }
}
