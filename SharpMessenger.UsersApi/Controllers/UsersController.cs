using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpMessanger.Domain.Clients;
using SharpMessenger.DbInteraction.Repositories.Contracts;

namespace SharpMessenger.UsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        public readonly IUserRepository UserRepository = null!;
        public UsersController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        [HttpGet("GetUserData")]
        public IEnumerable<User> Get()
        {
            return UserRepository.GetUsers().Result;
        }
    }
}
