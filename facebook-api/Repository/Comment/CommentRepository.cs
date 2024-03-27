using Dapper;
using facebook_api.Database;
using facebook_api.Models.DTO.Request.Comment;
using facebook_api.Models.Entities;
using facebook_api.Utils;
using Microsoft.Extensions.Hosting;

namespace facebook_api.Repository.Comment
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DapperContext _dapperContext;
        public CommentRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task Delete(int commentID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_comment_delete;

                var parameters = new DynamicParameters();
                parameters.Add("comment_id", commentID);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task<IEnumerable<CommentEntity>> GetAll(int postID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_comment_get_all;

                var parameters = new DynamicParameters();
                parameters.Add("post_id", postID);

                var posts = await conn.QueryAsync<CommentEntity>(procedure, parameters);
                return posts;
            }
        }

        public async Task<CommentEntity> GetByID(int commentID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_comment_get_by_id;

                var parameters = new DynamicParameters();
                parameters.Add("comment_id", commentID);

                var post = await conn.QueryFirstAsync<CommentEntity>(procedure, parameters);
                return post;
            }
        }

        public async Task Create(CommentEntity comment)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_comment_create;

                var parameters = new DynamicParameters();
                parameters.Add("post_id", comment.PostID);
                parameters.Add("user_id", comment.UserID);
                parameters.Add("comment", comment.Comment);
                parameters.Add("media_type", comment.MediaType);
                parameters.Add("file_name", comment.FileName);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task Update(CommentEntity comment)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_comment_update;

                var parameters = new DynamicParameters();
                parameters.Add("comment_id", comment.ID) ;
                parameters.Add("comment", comment.Comment);
                parameters.Add("media_type", comment.MediaType);
                parameters.Add("file_name", comment.FileName);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }
    }
}
