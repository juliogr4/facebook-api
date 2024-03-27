using facebook_api.Models.DTO.Request.Friendship;
using facebook_api.Service.Friendship;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] SendRequest sendRequest)
        {
            await _friendshipService.SendRequest(sendRequest);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequest updateRequest)
        {
            await _friendshipService.UpdateRequest(updateRequest);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetByStatus([FromQuery] int userID, [FromQuery] string status)
        {
            var users = await _friendshipService.GetByStatus(userID, status);
            return Ok(users);
        }
    }
}
