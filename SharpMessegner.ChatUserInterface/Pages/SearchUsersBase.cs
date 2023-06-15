using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessanger.Domain.Clients;
using SharpMessegner.ChatUserInterface.UIModels;
using Blazored.SessionStorage;
using System.Net.Http.Json;
using SharpMessenger.Application.Contracts;
using SharpMessenger.Application.UiModels;

namespace SharpMessegner.ChatUserInterface.Pages
{
    public class SearchUsersBase : ComponentBase, ISearchUserPage
    {

        [Inject]
        private ISessionStorageService Session { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider State { get; set; } = null!;

        [Inject]
        private HttpClient Client { get; set; } = null!;

        public List<string> UserFriends = null!;
        public string SearchOptions = string.Empty;
        public SearchedItemModel[] SearchedData = Array.Empty<SearchedItemModel>();
        public AuthenticationState AuthState = null!;
        private string CurrentUserName = null!;

        protected override async Task OnInitializedAsync()
        {
            await OnWindowInitialized();
        }

        public async Task OnSearchButtonClick()
        {
            IEnumerable<User> users = await Client.GetFromJsonAsync<IEnumerable<User>>("api/Users/GetUserData/")
                ?? Array.Empty<User>();

            SearchedData = users
              .Where(x => x.UserNameReference.Contains(SearchOptions) && !string.Equals(x.UserNameReference, CurrentUserName))
              .Select(x => UserFriends.Contains(x.UserNameReference)
                           ? new SearchedItemModel(new PlainStringData(x.UserNameReference), ButtonDefaults.CreateDeleteButton())
                           : new SearchedItemModel(new PlainStringData(x.UserNameReference), ButtonDefaults.CreateAddButton()))
              .ToArray();
        }

        public async Task OnAddDeleteButtonClick(ISearchedItem currentItem)
        {
            if (string.Equals(currentItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                OnAddButtonClick(currentItem);
            }
            else
            {
                OnDeleteButtonClick(currentItem);
            }

            await Session.SetItemAsync<List<string>>(CurrentUserName, UserFriends);
        }

        public void OnAddButtonClick(ISearchedItem currentItem)
        {
            currentItem.Button = ButtonDefaults.CreateDeleteButton();
            UserFriends.Add(currentItem.UserData.UserName);
        }

        public void OnDeleteButtonClick(ISearchedItem currentItem)
        {
            currentItem.Button = ButtonDefaults.CreateAddButton();
            UserFriends.Remove(currentItem.UserData.UserName);
        }

        public async Task OnWindowInitialized()
        {
            AuthState = await State.GetAuthenticationStateAsync();
            CurrentUserName = string.Concat("@", AuthState.User.Identity!.Name);
            UserFriends = await Session.GetItemAsync<List<string>>(CurrentUserName);
        }
    }
}
