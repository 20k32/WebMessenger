using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents
{
    public interface IUserFriendsList
    {
        ValueTask<List<string>> GetUserFriendsAsync(string key);
        ValueTask SetUserFriendsAsync(List<string> userFriends, string key);
    }
}
