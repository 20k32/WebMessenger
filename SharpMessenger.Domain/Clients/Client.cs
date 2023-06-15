namespace SharpMessanger.Domain.Clients
{
    public class Client
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        // for serialization purpose only
        public Client()
        { }

        public Client(string name, string password, string role)
        {
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
