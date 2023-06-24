using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessanger.Domain.Clients;
using Blazored.SessionStorage;
using System.Net.Http.Json;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.UiModels;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.AppLogic.SearchWindowLogic;

namespace SharpMessegner.ChatUserInterface.Pages
{
    public class SearchUsersBase : ComponentBase
    {

        [Inject]
        private ISessionStorageService Session { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider State { get; set; } = null!;

        [Inject]
        private HttpClient Client { get; set; } = null!;

        internal SearchUsersWindow Window = null!;

        protected override async Task OnInitializedAsync()
        {
            Window = new(new SearchUsersWindowComponentsManager(State, Session, Client));

            await Window.OnWindowInitialized();
        }
    }
}
