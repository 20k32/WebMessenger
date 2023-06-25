using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharpMessenger.Domain.Messages;
using SharpMessenger.UsersApi.Hubs.Interfaces;
using System.Security.Claims;

namespace SharpMessenger.UsersApi.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        [Authorize]
        public async Task SendToUser(Message message)
        {
            string recipient = message.Recipient.Substring(1);
            await Clients.Caller.SendMessageToUser(message);
            await Clients.User(recipient).SendMessageToUser(message);
           
        }
    }
}
