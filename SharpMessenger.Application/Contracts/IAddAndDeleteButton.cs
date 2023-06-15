using SharpMessegner.ChatUserInterface.UIModels;

namespace SharpMessenger.Application.Contracts
{
    public interface IAddAndDeleteButton
    {
        void OnAddButtonClick(ISearchedItem button);
        void OnDeleteButtonClick(ISearchedItem button);
    }
}
