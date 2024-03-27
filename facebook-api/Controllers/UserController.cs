using facebook_api.Models.DTO.Request.Email;
using facebook_api.Models.DTO.Request.User;
using facebook_api.Models.Entities;
using facebook_api.Models.Enum;
using facebook_api.Service.Email;
using facebook_api.Service.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace facebook_api.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest createUserRequest)
        {
            await _userService.Create(createUserRequest);
            return Ok(new { message = "user created successfully. Confirm your e-mail" });
        }

        [HttpGet("verify-email/{emailToken}")]
        public async Task<IActionResult> VerifyEmail(string emailToken)
        {
            var user = await _userService.GetByToken(emailToken, TokenType.EMAIL);

            if (user == null)
            {
                return BadRequest(new { message = "This token is not valid" });
            }

            if (user.EmailVerifiedAt != null)
            {
                return Ok(new { message = "This token has already been verified" });
            }
            
            if(user.EmailTokenExpiresAt < DateTime.UtcNow)
            {
                return BadRequest(new { message = "This token has expired. A new email was sent to you again" });
            }

            await _userService.VerifyEmail(user.ID);

            return Ok(new { message = "Email verified successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromForm] string email)
        {
            await _userService.ForgotPassword(email);
            return Ok(new { message = "An email to reset the password was sent" });
        }

        [HttpPost($"change-password")]
        public async Task<ActionResult> ChangePassword([FromForm] string passwordToken, [FromForm]  string password, [FromForm]  string confirmPassword)
        {
            var user = await _userService.GetByToken(passwordToken, TokenType.PASSWORD);
            return Ok();
        }
    }
}
