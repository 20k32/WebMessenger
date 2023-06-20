using Microsoft.AspNetCore.Components;
using SharpMessenger.Domain.Contracts;
using SharpMessenger.Domain.Messages;
using SharpMessenger.Domain.UiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic
{
    public class MainWindowForWebUiWrapper : ComponentBase
    {
        public MainWindow CoreWindow = null!;

        public MainWindowForWebUiWrapper(MainWindow window)
        {
            CoreWindow = window;
        }

        public Task InitConnection()
        {
            CoreWindow.InitConnection();
            CoreWindow.SetSendToUserEventHandler(ChangeStateSendToUserEventHandler);
            return Task.CompletedTask;
        }

        public Task ChangeStateLoadHistoryStateAsync(ISearchedItem searchedItem)
        {
            CoreWindow.LoadHistoryAsync(searchedItem);

            StateHasChanged();

            return Task.CompletedTask;
        }

        public async Task ChangeStateOnAddDeleteButtonClick(ISearchedItem currentItem)
        {
            await CoreWindow.OnAddDeleteButtonClick(currentItem);

            StateHasChanged();
        }

        private async Task ChangeStateSendToUserEventHandler(Message message)
        {
            await CoreWindow.BaseMessageHandler(message);

            StateHasChanged();
        }

        public void ChangeStateOnMouseOver(ISearchedItem currentItem)
        {
            CoreWindow.OnMouseOver(currentItem);
            StateHasChanged();
        }
    }
}
