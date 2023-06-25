using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessanger.Domain.Clients;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.UiModels;
using System.Net.Http.Json;

namespace SharpMessenger.Domain.AppLogic.SearchWindowLogic
{
    internal sealed class SearchUsersWindow : ISearchUserPage
    {
        private SearchUsersWindowComponentsManager Manager = null!;

        public List<string> UserFriends = null!;
        public string SearchOptions = string.Empty;
        public SearchedItemModel[] SearchedData = Array.Empty<SearchedItemModel>();

        public SearchUsersWindow(SearchUsersWindowComponentsManager manager) =>
            Manager = manager;
            

        public async Task OnSearchButtonClick()
        {
            IEnumerable<User> users = await Manager.GetClientsFromServer() ?? Array.Empty<User>();

            SearchedData = users
              .Where(x => x.UserNameReference.Contains(SearchOptions) && !string.Equals(x.UserNameReference, Manager.UserName))
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

            await Manager.SetUserFriendsAsync(UserFriends);
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
            await Manager.InitializeFields();
            UserFriends = await Manager.GetUserFriendsAsync() ?? new();
        }
    }
}
