using SharpMessanger.Domain.Clients;

namespace SharpMessenger.DbInteraction.Database
{
    public class ListDbContext : IDbContext
    {
        public ICollection<User> Users { get; set; }

        public ListDbContext()
        {
            Users = new List<User>()
            {
                new("admin", "admin", "@admin", "admin"),
                new("user", "user", "@user", "user")
            };
        }
    }
}
