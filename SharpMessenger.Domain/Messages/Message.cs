namespace SharpMessenger.Domain.Messages
{
    public class Message
    {
        public string Title = null!;
        public string Data = null!;
        public string Sender = null!;
        public string Recipient = null!;
        public DateTime SendWhen;

        public Message() : this("default_title", "default_data", "default_sender", "default_recipient")
        { }

        public Message(string title, string data, string sender, string recipient) =>
            (Title, Data, Sender, SendWhen, Recipient) = (title, data, sender, DateTime.Now, recipient);
    }
}
