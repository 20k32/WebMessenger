using SharpMessanger.Domain.Clients;

namespace SharpMessenger.DbInteraction.Database
{
    // true database with entity framework core
    public class AppDbContext : IDbContext
    {
        public ICollection<User> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
