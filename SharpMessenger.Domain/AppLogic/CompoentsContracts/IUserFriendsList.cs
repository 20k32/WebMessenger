namespace SharpMessenger.Domain.AppLogic.ComponentsContracts
{
    internal interface IUserFriendsList
    {
        ValueTask<List<string>> GetUserFriendsAsync();
        ValueTask SetUserFriendsAsync(List<string> userFriends);
    }
}
