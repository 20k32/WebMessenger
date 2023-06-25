using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMessenger.Domain.AppLogic
{
    internal class ApplicationComopentsBase
    {

        protected string AvaliableUsersSessionKey = null!;
        public string UserName = string.Empty;
        protected AuthenticationStateProvider StateProvider = null!;
        protected ISessionStorageService ClientSession = null!;

        public ApplicationComopentsBase(AuthenticationStateProvider provider, ISessionStorageService service) =>
            (StateProvider, ClientSession) = (provider, service);

        public virtual async ValueTask InitializeFields()
        {
            UserName = (await StateProvider.GetAuthenticationStateAsync()).User.Identity!.Name!;
            AvaliableUsersSessionKey = string.Concat(UserName, "_avaliableUsers");

            string.Intern(UserName);
            string.Intern(AvaliableUsersSessionKey);
        }
    }
}
