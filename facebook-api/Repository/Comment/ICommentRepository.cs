using facebook_api.Models.DTO.Request.Comment;
using facebook_api.Models.DTO.Response.Comment;
using facebook_api.Models.Entities;

namespace facebook_api.Repository.Comment
{
    public interface ICommentRepository
    {
        Task Create(CommentEntity comment);
        Task Update(CommentEntity comment);
        Task<IEnumerable<CommentEntity>> GetAll(int postID);
        Task Delete(int commentID);
        Task<CommentEntity> GetByID(int commentID);
    }
}
