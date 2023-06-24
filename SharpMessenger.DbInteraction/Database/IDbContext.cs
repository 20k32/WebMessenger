using SharpMessanger.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.DbInteraction.Database
{
    internal interface IDbContext
    { 
        ICollection<User> Users { get; set; }
    }
}
