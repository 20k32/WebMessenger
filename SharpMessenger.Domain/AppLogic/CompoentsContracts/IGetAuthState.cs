using Microsoft.AspNetCore.Components.Authorization;

namespace SharpMessenger.Domain.AppLogic.ComponentsContracts
{
    internal interface IGetAuthState
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }
}
