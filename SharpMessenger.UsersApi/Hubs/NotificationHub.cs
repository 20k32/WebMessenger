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
            // the user with id rep or @rep not exists in signalR !!!
            // todo: solve
            string rep = message.Recipient.Substring(1);
            await Clients.Caller.SendMessageToUser(message);
            try
            {
                var user1 =  Clients.User(message.Recipient);
                var user2 = Clients.User(rep);
                var client1 = Clients.Client(message.Recipient);
                var client2 = Clients.Client(rep);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
