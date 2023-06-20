namespace SharpMessenger.Domain.Contracts
{
    public interface IMainChatPage : IOnInitialized, IInitializeConnection
    {
        Task LoadHistoryAsync(ISearchedItem searchedItem);
    }
}