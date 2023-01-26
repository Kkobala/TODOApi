using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODOAppApi.Auth;
using TODOAppApi.Db.Entities;
using TODOAppApi.Models.Requests;
using TODOAppApi.Repositories;

namespace TODOAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISendEmailRequestRepository _sendEmailRequestRepository;

        public AuthController(
            IConfiguration configuration,
            ISendEmailRequestRepository sendEmailRequestRepository,
            UserManager<UserEntity> userManager,
            TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _sendEmailRequestRepository = sendEmailRequestRepository;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "Pong";
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            // Create
            var entity = new UserEntity();
            entity.UserName = request.Email;
            entity.Email = request.Email;
            var result = await _userManager.CreateAsync(entity, request.Password);

            if (!result.Succeeded)
            {
                var firstError = result.Errors.First();
                return BadRequest(firstError.Description);
            }

            // var token = await _userManager.GeneratePasswordResetTokenAsync(entity);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // TODO:Check user credentials...
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // User not found
                return NotFound("User not found");
            }
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isCorrectPassword)
            {
                return BadRequest("Invalid email or password");
            }

            return Ok(_tokenGenerator.Generate(user.Id.ToString()));
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequest request)
        {
            // 0. Find user
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // 1. Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 2. Insert email into SendEmailRequest table
            var sendEmailRequestEntity = new SendEmailRequestEntity();
            sendEmailRequestEntity.ToAddress = request.Email;
            sendEmailRequestEntity.Status = SendEmailRequestStatus.New;
            sendEmailRequestEntity.CreatedAt = DateTime.Now;

            var url = _configuration["PasswordResetUrl"]!
                .Replace("{userId}", user.Id.ToString())
                .Replace("{token}", token);
            var resetUrl = $"<a href=\"{url}\">Reset password</a>";
            sendEmailRequestEntity.Body = $"Hello, your password reset link is: {resetUrl}";

            _sendEmailRequestRepository.Insert(sendEmailRequestEntity);
            await _sendEmailRequestRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return NotFound("User not found");
            }
            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            // TODO: Return result
            if (!resetResult.Succeeded)
            {
                var firstError = resetResult.Errors.First();
                return StatusCode(500, firstError.Description);
            }

            return Ok();
        }
    }
}