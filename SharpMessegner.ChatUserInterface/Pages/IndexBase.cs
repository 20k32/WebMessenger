using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessenger.Domain.AppLogic;
using SharpMessenger.Domain.AppLogic.MainWindowLogic;
using System.Runtime.CompilerServices;

namespace SharpMessegner.ChatUserInterface.Pages
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject]
        private ISessionStorageService Session { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider State { get; set; } = null!;

        internal MainWindow Window = null!;

        protected override async Task OnInitializedAsync()
        {
            Window = new(new MainWindowComponentsManager(State, Session));

            Window.NotifyUserIterfaceStateChanged += StateHasChanged;
            await Window.OnWindowInitialized();
        }

        public void Dispose()
        {
            Window.NotifyUserIterfaceStateChanged -= StateHasChanged;
            Window.Dispose();
        }
    }
}
