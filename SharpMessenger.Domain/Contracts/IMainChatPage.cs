using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.Contracts
{
    public interface IMainChatPage : IOnInitialized, IInitializeConnection
    {
        Task LoadHistoryAsync(ISearchedItem searchedItem);
        Task SendMessageAsync(Message message);
    }
}