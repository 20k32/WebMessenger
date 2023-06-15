using SharpMessanger.Domain.Clients;
using SharpMessenger.DbInteraction.Database;
using SharpMessenger.DbInteraction.Repositories.Contracts;

namespace SharpMessenger.DbInteraction.Repositories
{
    public class UserRepository : IUserRepository
    {
        IDbContext CurrentDbContext = null!;
        public UserRepository(IDbContext context)
        {
            CurrentDbContext = context;
        }

        public Task<User> DeleteUser(User user)
        {
            CurrentDbContext.Users.Remove(user);

            return Task.FromResult(user);
        }

        public Task<User> DeleteUser(string userNameReference)
        {
            var user = CurrentDbContext.Users.FirstOrDefault(user => user.UserNameReference == userNameReference);

            if(user != null)
            {
                CurrentDbContext.Users.Remove(user);
            }

            return Task.FromResult(user)!;
        }

        public Task<User> GetUserByName(string userName)
        {
            var result =
                Task.FromResult(
                    CurrentDbContext
                    .Users
                    .FirstOrDefault(user => user.Name == userName));

            return result!;
        }

        public User AddUser(User user)
        {
            CurrentDbContext.Users.Add(user);
            return user;
        }

        public Task<User> GetUserByNameReference(string userNameReference)
        {
            var result = CurrentDbContext
                    .Users
                    .FirstOrDefault(user => user.UserNameReference == userNameReference)!;

            return Task.FromResult(result);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(CurrentDbContext.Users.AsEnumerable());
        }

        public Task<User> UpdateUser(User user)
        {
            var currentUser = CurrentDbContext.Users.FirstOrDefault(user => user.UserNameReference == user.UserNameReference);
            
            if(currentUser != null)
            {
                currentUser.Name = user.Name;
                currentUser.Password = user.Password;
            }

            return Task.FromResult(currentUser)!;
        }
    }
}
