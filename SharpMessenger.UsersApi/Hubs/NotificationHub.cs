using Microsoft.AspNetCore.SignalR;
using SharpMessenger.Domain.Messages;
using SharpMessenger.UsersApi.Hubs.Interfaces;

namespace SharpMessenger.UsersApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        public async Task SendToUser(Message message)
        {
            await Clients.Caller.SendMessageToUser("You", message);
            await Clients.User(message.Recipient).SendMessageToUser(message.Sender, message);
        }
    }
}
