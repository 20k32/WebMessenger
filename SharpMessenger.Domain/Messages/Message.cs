namespace SharpMessenger.Domain.Messages
{
    public sealed class Message
    {
        public string Title { get; set; } = null!;
        public string Data { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Recipient { get; set; } = null!;
        public string SendWhen { get; set; } = null!;

        public Message() : this("default_title", "default_data", "default_sender", "default_recipient", DateTime.Now.ToShortTimeString())
        { }

        public Message(string title, string data, string sender, string recipient, string sendWhen) =>
            (Title, Data, Sender, SendWhen, Recipient) = (title, data, sender, sendWhen, recipient);
    }
}
