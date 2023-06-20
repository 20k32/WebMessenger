namespace SharpMessenger.Domain.Contracts
{
    public interface IAddAndDeleteButton
    {
        void OnAddButtonClick(ISearchedItem button);
        void OnDeleteButtonClick(ISearchedItem button);
    }
}
