using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Primitives;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.AppLogic.Authentication;
using SharpMessenger.Domain.AppLogic.MainWindowLogic;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.Messages;
using SharpMessenger.Domain.UiModels;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpMessenger.Domain.AppLogic
{
    public class MainWindow : IMainChatPage, IAddAndDeleteButton, IDisposable
    {

        private string HistorySessionKey = null!;

        private List<string> UserFriends = new();

        private HubConnection Connection = null!;
        private MainWindowComponentsManager Manager = null!;

        public AuthenticationState AuthState = null!;
        public Action NotifyUserIterfaceStateChanged = null!;

        public string CurrentUserName = string.Empty;
        public string RecipientName = string.Empty;
        public string RowBackgroundColor = string.Empty;
        public string CursorStyle = string.Empty;

        public List<SearchedItemModel> AvailableUsers = new();
        public Dictionary<string, List<Message>> History = null!;

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

            History = await Manager.GetUserHistoryAsync(HistorySessionKey) ?? new();

            // get upcoming messages for every user
            AvailableUsers = UserFriends
                .Select(x => new SearchedItemModel(new ComplexData(x, default(int)), ButtonDefaults.CreateDeleteButton()))
                .ToList();
            //---

            // todo: init connection
            await InitConnection();
        }

        public async Task LoadHistoryAsync(ISearchedItem searchedItem)
        {
            if (!string.Equals(searchedItem.Button.ButtonClass,
                    ButtonDefaults.ADD_BUTTON_CLASS))
            {
                RecipientName = searchedItem.UserData.UserName;
            }

            NotifyUserIterfaceStateChanged.Invoke();
        }

        public async Task InitConnection()
        {
            /*Connection = new HubConnectionBuilder()
                   .WithUrl("https://localhost:7105/notification", options =>
                   {
                       options.AccessTokenProvider = async () => await Manager.CustomAuthenticationStateProvider.GetToken();
                   })
                   .WithAutomaticReconnect()
                   .Build();*/

            string token = await Manager.CustomAuthenticationStateProvider.GetToken();

            Connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7105/notification", options =>
            {
                options.Headers.Add("Authorization", new AuthenticationHeaderValue(
                        "Bearer", token).ToString());
            })
            .WithAutomaticReconnect()
            .Build();

            Connection.On<Message>("SendMessageToUser", BaseMessageHandler);

            await Connection.StartAsync();
        }

        public async Task BaseMessageHandler(Message message)
        {
            try
            {
                /*if (!message.Recipient.Equals(RecipientName))
                {
                    ComplexData userData = (AvailableUsers.Find(x => string.Equals(x.UserData.UserName, RecipientName))!.UserData as ComplexData)!;
                    userData.UnreadMessages++;
                }

                History[RecipientName].Add(message);

                await Manager.SetUserHistory(History, HistorySessionKey);

                NotifyUserIterfaceStateChanged.Invoke();*/
                Console.WriteLine(message.Data);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            
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

            NotifyUserIterfaceStateChanged.Invoke();

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
            NotifyUserIterfaceStateChanged.Invoke();
        }

        public List<Message> GetHistoryForUser(string username)
        {
            if(History == null)
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
            await Connection.DisposeAsync();
        }
    }
}
