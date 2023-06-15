using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.Models
{
    public class UserHistory
    {
        public List<Message> UserMessages = null!;

        public UserHistory(List<Message> userMessages)
        {
            UserMessages = userMessages;
        }

        public UserHistory() : this(new())
        { }
    }
}
