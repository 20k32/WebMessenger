﻿namespace SharpMessenger.Domain.AppLogic.ComponentsContracts
{
    internal interface IUserFriendsList
    {
        ValueTask<List<string>> GetUserFriendsAsync(string key);
        ValueTask SetUserFriendsAsync(List<string> userFriends, string key);
    }
}
