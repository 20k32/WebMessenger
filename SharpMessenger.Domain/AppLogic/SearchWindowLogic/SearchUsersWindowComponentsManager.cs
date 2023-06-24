using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessanger.Domain.Clients;
using SharpMessenger.Domain.AppLogic.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SharpMessenger.Domain.AppLogic.SearchWindowLogic
{
    internal sealed class SearchUsersWindowComponentsManager : ApplicationComopentsBase, ISearchUsersWindowComponents
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

        // BUG WAS HERE
        public async Task<IEnumerable<User>> GetClientsFromServer()
        {
            string token = await ((CustomAuthenticationStateProvider)StateProvider).GetToken();

            IEnumerable<User> result = null!;

            if (!string.IsNullOrWhiteSpace(token))
            {
                //USE THIS HEADERS WITH [AUTHROZIE] ATTRIBUTE ON SERVER!!!
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                result = await Client.GetFromJsonAsync<IEnumerable<User>>("/api/Users/GetUserData/");
            }

            return result;
        }
    }
}
