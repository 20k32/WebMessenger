using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpMessenger.DbInteraction.Repositories.Contracts;
using SharpMessenger.Presentation.Shared;
using SharpMessenger.UsersApi.Authentication;

namespace SharpMessenger.UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        IUserRepository UserRepository = null!;
        public AccountController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
        {
            if(loginRequest.UserName.Length > 19)
            {
                return Unauthorized();
            }

            var jwtAuthenticationManager = new JwtAuthenticationManager(UserRepository);
            var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.UserName, loginRequest.Password);

            if (userSession == null)
            {
                return Unauthorized();
            }

            return userSession;
        }
    }
}
