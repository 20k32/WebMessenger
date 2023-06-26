namespace SharpMessenger.Domain.UiModels
{
    public sealed class PlainStringData : Data
    {
        public PlainStringData(string data) =>
            UserName = data;

        public PlainStringData()
        {
            UserName = null!;
        }
    }
}
