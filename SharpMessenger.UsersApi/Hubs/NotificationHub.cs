﻿using Microsoft.AspNetCore.Authorization;
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
            string rep = message.Recipient.Substring(1);
            await Clients.Caller.SendMessageToUser(message);
            var user = Clients.User(rep);
            await Clients.User(rep).SendMessageToUser(message);
           
        }
    }
}
