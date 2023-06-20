namespace SharpMessenger.Domain.Contracts
{
    public interface ISearchUserPage : IOnInitialized, IAddAndDeleteButton
    {
        Task OnSearchButtonClick();
    }
}
