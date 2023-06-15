using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using SharpMessegner.ChatUserInterface.UIModels;
using SharpMessenger.Application;
using SharpMessenger.Application.Contracts;
using SharpMessenger.Application.UiModels;
using SharpMessenger.Domain.Messages;
using System;
using System.Runtime.CompilerServices;

namespace SharpMessegner.ChatUserInterface.Pages
{
    public class IndexBase : ComponentBase, IMainChatPage, IAddAndDeleteButton
    {
        [Inject]
        private ISessionStorageService Session { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider State { get; set; } = null!;

        public AuthenticationState AuthState = null!;
        private string CurrentUserName = string.Empty;
        public string RecipientName = string.Empty;
        List<string> UserFriends = new();
        public List<SearchedItemModel> AvailableUsers = new();
        public string RowBackgroundColor = string.Empty;
        public string CursorStyle = string.Empty;
        private HubConnection Connection = null!;

        protected override async Task OnInitializedAsync()
        {
            await OnWindowInitialized();
        }

        public Task LoadHistoryAsync(ISearchedItem searchedItem)
        {
            if (!string.Equals(searchedItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                RecipientName = searchedItem.UserData.UserName;
            }

            return Task.CompletedTask;
        }

        public async Task OnWindowInitialized()
        {
            AuthState = await State.GetAuthenticationStateAsync();
            CurrentUserName = string.Concat("@", AuthState.User.Identity!.Name);
            UserFriends = await Session.GetItemAsync<List<string>>(CurrentUserName);

            // get upcoming messages for every user
            AvailableUsers = UserFriends
                .Select(x => new SearchedItemModel(new ComplexData(x, default(int)), ButtonDefaults.CreateDeleteButton()))
                .ToList();
            //---

            // todo: init connection
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

        private void RemoveFromAvailableUsersList(string name)
        {
            AvailableUsers.Remove(AvailableUsers.First(x => string.Equals(x.UserData.UserName, name)));
        }

        private void AddToAvailableUsersList(string name)
        {
            AvailableUsers.Add(new(new ComplexData(name, default(int)), ButtonDefaults.CreateDeleteButton()));
        }

        public void OnAddButtonClick(ISearchedItem currentItem)
        {
            currentItem.Button = ButtonDefaults.CreateDeleteButton();

            UserFriends.Add(currentItem.UserData.UserName);
            AddToAvailableUsersList(currentItem.UserData.UserName);
        }

        public void OnDeleteButtonClick(ISearchedItem currentItem)
        {
            UserFriends.Remove(currentItem.UserData.UserName);
            RemoveFromAvailableUsersList(currentItem.UserData.UserName);

            if (string.Equals(currentItem.UserData.UserName, RecipientName))
            {
                //todo: erase history for particular chat
            }
        }

        public void OnMouseOver(ISearchedItem currentItem)
        {
            if (!string.Equals(currentItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                RowBackgroundColor = "yellow";
                CursorStyle = "pointer";
            }
            else
            {
                RowBackgroundColor = "grey";
                CursorStyle = "arrow";
            }
            StateHasChanged();
        }

        public int GetUnreadMessagesForUser(ISearchedItem user)
        {
            return (user.UserData as ComplexData)!.UnreadMessages;
        }

        public Task InitConnection()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7105/notification")
                .WithAutomaticReconnect()
                .Build();

            Connection.On<Message>("SendMessageToUser", MessageHandler);

            return Task.CompletedTask;
        }

        private void MessageHandler(Message message)
        {
        // todo: logic for receiving message
        }
    }
}
