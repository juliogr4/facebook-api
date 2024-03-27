using facebook_api.Models.DTO.Request.Comment;
using facebook_api.Models.DTO.Response.Comment;

namespace facebook_api.Service.Comment
{
    public interface ICommentService
    {
        Task Create(CreateCommentRequest upsertCommentRequest);
        Task Update(int commentID, UpdateCommentRequest updateCommentRequest);
        Task<IEnumerable<CommentResponse>> GetAll(int postID);
        Task Delete (int commentID);
    }
}
