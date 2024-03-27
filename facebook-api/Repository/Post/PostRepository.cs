using Dapper;
using facebook_api.Database;
using facebook_api.Models.Entities;
using facebook_api.Utils;

namespace facebook_api.Repository.Post
{
    public class PostRepository : IPostRepository
    {
        private readonly DapperContext _dapperContext;
        public PostRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> CheckFriendshipStatus(int sourceID, int userID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_friendship_check_status;

                var parameters = new DynamicParameters();
                parameters.Add("source_id", sourceID);
                parameters.Add("user_id", userID);

                return await conn.QuerySingleOrDefaultAsync<int>(procedure, parameters) == 1;
            }
        }

        public async Task Delete(int postID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_post_delete;

                var parameters = new DynamicParameters();
                parameters.Add("post_id", postID);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }

        public async Task<PostEntity> GetByID(int postID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_post_get_by_id;

                var parameters = new DynamicParameters();
                parameters.Add("post_id", postID);

                var post = await conn.QuerySingleAsync<PostEntity>(procedure, parameters);
                return post;
            }
        }

        public async Task<IEnumerable<PostEntity>> GetFeed(int userID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_post_get_feed_posts;

                var parameters = new DynamicParameters();
                parameters.Add("user_id", userID);

                var posts = await conn.QueryAsync<PostEntity>(procedure, parameters);
                return posts;
            }
        }

        public async Task<IEnumerable<PostEntity>> GetUserProfilePosts(int userID)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_post_get_user_profile_posts;

                var parameters = new DynamicParameters();
                parameters.Add("user_id", userID);

                var posts = await conn.QueryAsync<PostEntity>(procedure, parameters);
                return posts;
            }
        }

        public async Task Upsert(PostEntity post)
        {
            using (var conn = _dapperContext.CreateConnection())
            {
                var procedure = Procedures.pr_post_upsert;
                
                var parameters = new DynamicParameters();

                if(post.ID > 0)
                {
                    parameters.Add("post_id", post.ID);
                }

                parameters.Add("user_id", post.UserID);
                parameters.Add("message", post.Message);
                parameters.Add("media_type", post.MediaType);
                parameters.Add("file_name", post.FileName);

                await conn.ExecuteAsync(procedure, parameters);
            }
        }
    }
}
