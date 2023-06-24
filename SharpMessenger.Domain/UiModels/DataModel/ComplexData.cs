namespace SharpMessenger.Domain.UiModels
{
    public sealed class ComplexData : Data
    {
        public int UnreadMessages { get; set; }
        public override string UserName { get; set; }

        public ComplexData(string data, int unreadMessages) =>
            (UserName, UnreadMessages) = (data, unreadMessages);
    }
}
