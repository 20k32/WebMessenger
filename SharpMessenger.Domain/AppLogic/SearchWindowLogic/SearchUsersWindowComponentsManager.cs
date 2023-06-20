using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessanger.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.Domain.AppLogic.SearchWindowLogic
{
    public class SearchUsersWindowComponentsManager : ApplicationComopentsBase, ISearchUsersWindowComponents
    {
        private HttpClient Client = null!;

        public SearchUsersWindowComponentsManager(AuthenticationStateProvider provider, ISessionStorageService service, HttpClient client) 
            : base(provider, service)
        {
            Client = client;
        }

        public Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return StateProvider.GetAuthenticationStateAsync();
        }

        public ValueTask<List<string>> GetUserFriendsAsync(string key)
        {
            return ClientSession.GetItemAsync<List<string>>(key);
        }

        public ValueTask SetUserFriendsAsync(List<string> userFriends, string key)
        {
            return ClientSession.SetItemAsync<List<string>>(key, userFriends);
        }

        public Task<IEnumerable<User>> GetClientsFromServer() =>
            Client.GetFromJsonAsync<IEnumerable<User>>("api/Users/GetUserData/")!;
    }
}
