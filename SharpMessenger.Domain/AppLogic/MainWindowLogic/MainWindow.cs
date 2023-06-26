using Microsoft.AspNetCore.SignalR.Client;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.AppLogic.MainWindowLogic;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.Messages;
using SharpMessenger.Domain.UiModels;
using System.Runtime.InteropServices;


namespace SharpMessenger.Domain.AppLogic
{

    internal sealed class MainWindow : IMainChatPage, IAddAndDeleteButton, IDisposable
    {
        private List<string> UserFriends = new();

        private HubConnection Connection = null!;
        private MainWindowComponentsManager Manager = null!;
        public Action NotifyUserIterfaceStateChanged = null!;

        public string CurrentUserName => Manager.UserName;
        public string RecipientName = string.Empty;
        public string RowBackgroundColor = string.Empty;
        public string CursorStyle = string.Empty;

        public List<SearchedItemModel> AvailableUsers = new();
        public Dictionary<string, List<Message>> History = null!;
        public List<Message> Messages = new();

        public MainWindow(MainWindowComponentsManager manager)
        {
            Manager = manager;
        }

        public async Task OnWindowInitialized()
        {
            await Manager.InitializeFields();

            UserFriends = await Manager.GetUserFriendsAsync() ?? new();

            History = await Manager.GetUserHistoryAsync() ?? new();

            AvailableUsers = await Manager.GetAvaliableUsersAsync() ?? new();

            await InitConnection();
        }

        public async Task LoadHistoryAsync(ISearchedItem searchedItem)
        {
            if (!string.Equals(searchedItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                RecipientName = searchedItem.UserData.UserName;
                ((ComplexData)searchedItem.UserData).UnreadMessages = 0;

                Messages = GetHistoryForUser(RecipientName);
            }
            NotifyUserIterfaceStateChanged.Invoke();
        }

        public async Task InitConnection()
        {
            string token = await Manager.CustomAuthenticationStateProvider.GetToken();

            // todo: store connection in session storage to get upcoming messages even not on the main chat page on site
            Connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7105/notification?token={token}")
            .WithAutomaticReconnect()
            .Build();

            Connection.On<Message>("SendMessageToUser", BaseMessageHandler);

            await Connection.StartAsync();
        }

        public async Task BaseMessageHandler(Message message)
        {
            if (!message.Sender.Equals(CurrentUserName)
                && !message.Sender.Equals(RecipientName))
            {
                ComplexData userData = (AvailableUsers.Find(x => string.Equals(x.UserData.UserName, message.Sender))!.UserData as ComplexData)!;
                userData.UnreadMessages++;
                NotifyUserIterfaceStateChanged.Invoke();

                var tempList = GetHistoryForUser(message.Sender);
                tempList.Add(message);
                History[message.Sender] = tempList;

                await Manager.SetUserHistory(History);
            }
            else
            {
                History[RecipientName].Add(message);
                await Manager.SetUserHistory(History);

                NotifyUserIterfaceStateChanged.Invoke();
            }
        }

        public int GetUnreadMessagesForUser(ISearchedItem user)
        {
            return ((ComplexData)user.UserData).UnreadMessages;
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

            NotifyUserIterfaceStateChanged.Invoke();

            await Manager.SetUserFriendsAsync(UserFriends);
            await Manager.SetAvaliableUsersAsync(AvailableUsers);
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

            NotifyUserIterfaceStateChanged.Invoke();
        }

        public List<Message> GetHistoryForUser(string username)
        {
            if (History == null)
            {
                return null!;
            }

            ref List<Message> list = ref CollectionsMarshal.GetValueRefOrAddDefault(History, username, out bool extists)!;

            if (!extists)
            {
                list = new();
            }

            return list;
        }

        private void SignMessage(Message message)
        {
            message.Sender = CurrentUserName;
            message.Recipient = RecipientName;
            message.SendWhen = DateTime.Now.ToShortTimeString();
        }

        public async Task SendMessageAsync(Message message)
        {
            if (Connection is not null)
            {
                SignMessage(message);
                await Connection.InvokeAsync("SendToUser", message);
            }
        }

        public async void Dispose()
        {
            await Manager.SetAvaliableUsersAsync(AvailableUsers);
            await Connection.DisposeAsync();
        }
    }
}
