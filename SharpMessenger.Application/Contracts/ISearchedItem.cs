using SharpMessenger.Application.UiModels;

namespace SharpMessenger.Application.Contracts
{
    public interface ISearchedItem
    {
        Button Button { get; set; }
        Data UserData { get; set; }
    }
}
