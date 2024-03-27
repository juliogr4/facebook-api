using facebook_api.Models.DTO.Request.Post;
using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.DTO.Response.Post;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Service.File;
using facebook_api.Service.Post;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPut("{postID}")]
        public async Task<IActionResult> Update(int postID, [FromForm] UpsertPostRequest upsertPostRequest)
        {
            await _postService.Update(postID, upsertPostRequest);
            return Ok(new { mensagem = "Post updated successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UpsertPostRequest upsertPostRequest)
        {
            await _postService.Create(upsertPostRequest);
            return Ok(new { mensagem = "Post created successfully" });
        }

        [HttpDelete("{postID}")]
        public async Task<IActionResult> Delete(int postID)
        {
            await _postService.Delete(postID);
            return Ok(new { mensagem = "Post deleted successfully" });
        }

        [HttpGet("feed/{userID}")]
        public async Task<IActionResult> GetFeed(int userID)
        {
            var posts = await _postService.GetFeed(userID);
            return Ok(posts);
        }

        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetUserProfilePosts(int userID)
        {
            var sourceID = 1;
            var posts = await _postService.GetUserProfilePosts(sourceID, userID);
            return Ok(posts);
        }
    }
}
