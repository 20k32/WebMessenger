using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents
{
    public interface IGetUserHistory
    {
        ValueTask<Dictionary<string, List<Message>>> GetUserHistory(string key);
        ValueTask SetUserHistory(Dictionary<string, List<Message>> history, string key);
    }
}
