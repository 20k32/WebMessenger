using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.Domain.AppLogic
{
    public class ApplicationComopentsBase
    {
        protected AuthenticationStateProvider StateProvider = null!;
        protected ISessionStorageService ClientSession = null!;

        public ApplicationComopentsBase(AuthenticationStateProvider provider, ISessionStorageService service) =>
            (StateProvider, ClientSession) = (provider, service);
    }
}
