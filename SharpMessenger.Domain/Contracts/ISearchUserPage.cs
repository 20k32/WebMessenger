namespace SharpMessenger.Domain.Contracts
{
    internal interface ISearchUserPage : IOnInitialized, IAddAndDeleteButton
    {
        Task OnSearchButtonClick();
    }
}
