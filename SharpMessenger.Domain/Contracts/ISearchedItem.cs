using SharpMessenger.Domain.UiModels;

namespace SharpMessenger.Domain.Contracts
{
    public interface ISearchedItem
    {
        Button Button { get; set; }
        Data UserData { get; set; }
    }
}
