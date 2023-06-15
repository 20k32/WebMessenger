using Microsoft.IdentityModel.Tokens;
using SharpMessenger.DbInteraction.Repositories.Contracts;
using SharpMessenger.Presentation.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace SharpMessenger.UsersApi.Authentication
{
    public class JwtAuthenticationManager
    {
        public const string JWT_SECURITY_KEY = "supersecret_test_security_key!342";
        private const int JWT_TOKEN_VALIDITY_MINUTES = 20;
        private readonly IUserRepository UserRepository = null!;

        public JwtAuthenticationManager(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public UserSession? GenerateJwtToken(string userName, string password)
        {
            if(string.IsNullOrWhiteSpace(userName)
               || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var userAccount = UserRepository.GetUserByName(userName).Result;
            
            if(userAccount == null)
            {
                userAccount = UserRepository.AddUser(new(userName, password, "@" + userName, "user"));
            }
            else if(userAccount.Password != password)
            {
                return null!;
            }

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINUTES);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, userAccount.Name),
                new(ClaimTypes.Role, userAccount.Role),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityHandler.WriteToken(securityToken);

            var userSession = new UserSession()
            {
                UserName = userAccount.Name,
                Role = userAccount.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };

            return userSession;
        }
    }
}
