using Microsoft.AspNetCore.Components.Authorization;

namespace SharpMessenger.Domain.AppLogic.ComponentsContracts
{
    public interface IGetAuthState
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }
}
