namespace SharpMessenger.Application.Contracts
{
    public interface ISearchUserPage : IOnInitialized, IAddAndDeleteButton
    {
        Task OnSearchButtonClick();
    }
}
