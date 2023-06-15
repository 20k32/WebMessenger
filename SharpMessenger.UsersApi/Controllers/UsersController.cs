﻿using Microsoft.AspNetCore.Mvc;
using SharpMessanger.Domain.Clients;
using SharpMessenger.DbInteraction.Repositories.Contracts;

namespace TestUsersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository UserRepository = null!;
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
