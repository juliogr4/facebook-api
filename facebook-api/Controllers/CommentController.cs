using facebook_api.Models.DTO.Request.Comment;
using facebook_api.Models.DTO.Request.Post;
using facebook_api.Models.DTO.Response.Comment;
using facebook_api.Models.DTO.Response.File;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Service.Comment;
using facebook_api.Service.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPut("{commentID}")]
        public async Task<IActionResult> Update(int commentID, [FromForm] UpdateCommentRequest updateCommentRequest)
        {
            await _commentService.Update(commentID, updateCommentRequest);
            return Ok(new { mensagem = "Comment updated successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCommentRequest createCommentRequest)
        {
            await _commentService.Create(createCommentRequest);
            return Ok(new { mensagem = "Comment created successfully" });
        }

        [HttpGet("{postID}")]
        public async Task<IActionResult> GetAll(int postID)
        {
            var comments = await _commentService.GetAll(postID);
            return Ok(comments);
        }

        [HttpDelete("{commentID}")]
        public async Task<IActionResult> Delete(int commentID)
        {
            await _commentService.Delete(commentID);
            return Ok(new { mensagem = "Comment deleted successfully" });
        }
    }
}
