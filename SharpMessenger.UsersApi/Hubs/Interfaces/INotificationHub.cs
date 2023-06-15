using SharpMessenger.Domain.Messages;

namespace SharpMessenger.UsersApi.Hubs.Interfaces
{
    public interface INotificationHub
    {
        Task SendMessageToUser(string userId, Message message);
    }
}
