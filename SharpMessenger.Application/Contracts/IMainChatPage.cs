using SharpMessenger.Application.Contracts;

namespace SharpMessenger.Application
{
    public interface IMainChatPage : IOnInitialized, IInitializeConnection
    {
        Task LoadHistoryAsync(ISearchedItem searchedItem);
    }
}