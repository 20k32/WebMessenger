using System.Runtime.CompilerServices;

namespace SharpMessanger.Domain.Clients
{
    public sealed class User : Client
    {
        public string UserNameReference { get; set; } = null!;

        // for serialization purpose only
        public User()
        { }

        public User(string name, string password, string userNameReference, string role) : base(name, password, role)
        {
            UserNameReference = userNameReference;
        }
    }
}
