using SharpMessanger.Domain.Clients;
using SharpMessenger.DbInteraction.Repositories;

namespace SharpMessenger.DbInteraction.Database
{
    // true database with entity framework core
    internal sealed class AppDbContext : IDbContext
    {
        public ICollection<User> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }


}
