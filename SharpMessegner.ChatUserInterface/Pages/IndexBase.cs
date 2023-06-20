using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessenger.Domain.AppLogic;
using SharpMessenger.Domain.AppLogic.MainWindowLogic;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.Messages;
using SharpMessenger.Domain.UiModels;


namespace SharpMessegner.ChatUserInterface.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private ISessionStorageService Session { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider State { get; set; } = null!;

        public MainWindow Window = null!;


        protected override async Task OnInitializedAsync()
        {
            Window = new(new MainWindowComponentsManager(State, Session));

            await Window.OnWindowInitialized();
        }

        public async Task LoadHistoryAsync(ISearchedItem searchedItem)
        {
            await Window.LoadHistoryAsync(searchedItem);
            StateHasChanged();
        }

        public void OnMouseOver(ISearchedItem currentItem)
        {
           Window.OnMouseOver(currentItem);
           StateHasChanged();
        }

        public int GetUnreadMessagesForUser(ISearchedItem user)
        {
            return (user.UserData as ComplexData)!.UnreadMessages;
        }

        public async Task InitConnection()
        {
            await Window.InitConnection();
            Window.SetSendToUserEventHandler(MessageHandler);
        }

        private async Task MessageHandler(Message message)
        {
            await Window.BaseMessageHandler(message);
            StateHasChanged();
        }

        public async Task OnAddDeleteButtonClick(ISearchedItem currentItem)
        {
            await Window.OnAddDeleteButtonClick(currentItem);
            StateHasChanged();
        }
    }
}
