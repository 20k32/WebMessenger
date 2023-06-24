using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.Contracts
{
    internal interface IMainChatPage : IOnInitialized, IInitializeConnection
    {
        Task LoadHistoryAsync(ISearchedItem searchedItem);
        Task SendMessageAsync(Message message);
    }
}