using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents;
using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic
{
    public class MainWindowComponentsManager : IMainWindowComponents
    {

        private AuthenticationStateProvider StateProvider = null!;
        private ISessionStorageService ClientSession = null!;

        public MainWindowComponentsManager(AuthenticationStateProvider provider, ISessionStorageService session)
        {
            StateProvider = provider;
            ClientSession = session;
        }

        public Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return StateProvider.GetAuthenticationStateAsync();
        }

        public ValueTask<List<string>> GetUserFriendsAsync(string key)
        {
            return ClientSession.GetItemAsync<List<string>>(key);
        }

        public ValueTask<Dictionary<string, List<Message>>> GetUserHistory(string key)
        {
            return ClientSession.GetItemAsync<Dictionary<string, List<Message>>>(key);
        }

        public ValueTask SetUserFriendsAsync(List<string> userFriends, string key)
        {
            return ClientSession.SetItemAsync(key, userFriends);
        }

        public ValueTask SetUserHistory(Dictionary<string, List<Message>> history, string key)
        {
            return ClientSession.SetItemAsync(key, history);
        }
    }
}
