using SharpMessanger.Domain.Clients;

namespace SharpMessenger.DbInteraction.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByNameReference(string userNameReference);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(User user);
        Task<User> DeleteUser(string userNameReference);
        Task<User> GetUserByName(string userName);
        User AddUser(User user);
    }
}
