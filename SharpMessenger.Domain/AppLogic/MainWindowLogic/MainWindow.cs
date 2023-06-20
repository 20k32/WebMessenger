using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.SignalR.Client;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.AppLogic.MainWindowLogic;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.Messages;
using SharpMessenger.Domain.UiModels;

namespace SharpMessenger.Domain.AppLogic
{
    public class MainWindow : IMainChatPage, IAddAndDeleteButton
    {

        private string CurrentUserName = string.Empty;
        private string HistorySessionKey = null!;
        private HubConnection Connection = null!;
        private List<string> UserFriends = new();

        public AuthenticationState AuthState = null!;
        public string RecipientName = string.Empty;
        public List<SearchedItemModel> AvailableUsers = new();
        public string RowBackgroundColor = string.Empty;
        public string CursorStyle = string.Empty;
        public Dictionary<string, List<Message>> History = null!;

        private MainWindowComponentsManager Manager = null!;

        public MainWindow(MainWindowComponentsManager manager)
        {
            Manager = manager;
        }

        public async Task OnWindowInitialized()
        {
            AuthState = await Manager.GetAuthenticationStateAsync();

            CurrentUserName = string.Concat("@", AuthState.User.Identity!.Name);
            UserFriends = await Manager.GetUserFriendsAsync(CurrentUserName);
            
            HistorySessionKey = string.Concat(CurrentUserName, "_history");
            History = await Manager.GetUserHistory(HistorySessionKey);

            // get upcoming messages for every user
            AvailableUsers = UserFriends
                .Select(x => new SearchedItemModel(new ComplexData(x, default(int)), ButtonDefaults.CreateDeleteButton()))
                .ToList();
            //---

            // todo: init connection
        }

        public virtual Task LoadHistoryAsync(ISearchedItem searchedItem)
        {
            if (!string.Equals(searchedItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                RecipientName = searchedItem.UserData.UserName;
            }
            //StateHasChanged();
            return Task.CompletedTask;
        }

        public Task InitConnection()
        {
            Connection = new HubConnectionBuilder()
                 .WithUrl("https://localhost:7105/notification")
                 .WithAutomaticReconnect()
                 .Build();    

            return Task.CompletedTask;
        }

        public void SetSendToUserEventHandler(Func<Message, Task> handler)
        {
            Connection.On<Message>("SendMessageToUser", handler);
        }

        public async Task BaseMessageHandler(Message message)
        {
            if (UserFriends.Contains(message.Sender))
            {
                if (!string.Equals(RecipientName, message.Sender))
                {
                    ComplexData userData = (AvailableUsers.Find(x => string.Equals(x.UserData.UserName, RecipientName))!.UserData as ComplexData)!;
                    userData.UnreadMessages++;
                }
            }
            //StateHasChanged();
            History[RecipientName].Add(message);
            await Manager.SetUserHistory(History, HistorySessionKey);
        }

        public int GetUnreadMessagesForUser(ISearchedItem user)
        {
            return (user.UserData as ComplexData)!.UnreadMessages;
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
            await Manager.SetUserFriendsAsync(UserFriends, CurrentUserName);
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
            //StateHasChanged();
        }
    }
}
