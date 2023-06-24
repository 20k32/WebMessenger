namespace SharpMessenger.Domain.UiModels
{
    internal sealed class PlainStringData : Data
    {
        public override string UserName { get; set; }

        public PlainStringData(string data) =>
            UserName = data;

        public PlainStringData()
        {
            UserName = null!;
        }
    }
}
