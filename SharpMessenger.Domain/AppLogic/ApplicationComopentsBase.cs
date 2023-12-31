﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessegner.Domain.UIModels;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public ValueTask<List<SearchedItemModel>> GetAvaliableUsersAsync()
        {
            return ClientSession.GetItemAsync<List<SearchedItemModel>>(AvaliableUsersSessionKey);
        }

        public ValueTask SetAvaliableUsersAsync(List<SearchedItemModel> avaliableUsersToSet)
        {
            return ClientSession.SetItemAsync(AvaliableUsersSessionKey, avaliableUsersToSet);
        }

        public virtual async ValueTask InitializeFields()
        {
            string plainName = (await StateProvider.GetAuthenticationStateAsync()).User.Identity!.Name!;
            UserName = string.Concat("@", plainName);
            AvaliableUsersSessionKey = string.Concat(UserName, "_avaliableUsers");

            string.Intern(UserName);
            string.Intern(AvaliableUsersSessionKey);
        }
    }
}
