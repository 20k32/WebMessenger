namespace SharpMessenger.Domain.Messages
{
    public class Message
    {
        public string Title = null!;
        public string Data = null!;
        public string Sender = null!;
        public string Recipient = null!;
        public string SendWhen = null!;

        public Message() : this("default_title", "default_data", "default_sender", "default_recipient", DateTime.Now.ToShortTimeString())
        { }

        public Message(string title, string data, string sender, string recipient, string sendWhen) =>
            (Title, Data, Sender, SendWhen, Recipient) = (title, data, sender, sendWhen, recipient);
    }
}
