using facebook_api.Models.DTO.Request.Reaction;
using facebook_api.Service.Reaction;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Controllers
{
    [Route("reaction")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("like")]
        public async Task<IActionResult> Create(ReactionRequest reactionRequest)
        {
            await _reactionService.Create(reactionRequest);
            return Ok();
        }

        [HttpDelete("unlike/{reactionID}")]
        public async Task<IActionResult> Delete(int reactionID)
        {
            await _reactionService.Delete(reactionID);
            return Ok();
        }
    }
}
