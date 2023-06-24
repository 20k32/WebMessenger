namespace SharpMessenger.Domain.Contracts
{
    internal interface IAddAndDeleteButton
    {
        void OnAddButtonClick(ISearchedItem button);
        void OnDeleteButtonClick(ISearchedItem button);
    }
}
