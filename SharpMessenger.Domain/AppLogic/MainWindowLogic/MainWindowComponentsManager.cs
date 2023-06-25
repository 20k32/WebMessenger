﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharpMessegner.Domain.UIModels;
using SharpMessenger.Domain.AppLogic.Authentication;
using SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents;
using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic
{
    internal sealed class MainWindowComponentsManager : ApplicationComopentsBase, IMainWindowComponents
    {
        private string HistorySessionKey = null!;

        public MainWindowComponentsManager(AuthenticationStateProvider provider,
            ISessionStorageService service) : base(provider, service)
        { }
        
        public override async ValueTask InitializeFields()
        {
            await base.InitializeFields();

            HistorySessionKey = string.Concat(UserName, "_history");
        }

        public Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return StateProvider.GetAuthenticationStateAsync();
        }

        public ValueTask<List<string>> GetUserFriendsAsync()
        {
            return ClientSession.GetItemAsync<List<string>>(UserName);
        }

        public ValueTask<Dictionary<string, List<Message>>> GetUserHistoryAsync()
        {
            return ClientSession.GetItemAsync<Dictionary<string, List<Message>>>(HistorySessionKey);
        }

        public ValueTask SetUserFriendsAsync(List<string> userFriends)
        {
            return ClientSession.SetItemAsync(UserName, userFriends);
        }

        public ValueTask SetUserHistory(Dictionary<string, List<Message>> history)
        {
            return ClientSession.SetItemAsync(HistorySessionKey, history);
        }

        public async ValueTask<List<SearchedItemModel>> GetOrSetAvaliableUsersAsync(List<SearchedItemModel> avaliableUsersToSet)
        {
            var item = await ClientSession.GetItemAsync<List<SearchedItemModel>>(AvaliableUsersSessionKey);

            if (item is null)
            {
                await ClientSession.SetItemAsync(AvaliableUsersSessionKey, avaliableUsersToSet);
                return await Task.FromResult(avaliableUsersToSet);
            }

            return await Task.FromResult(item);
        }

        public CustomAuthenticationStateProvider CustomAuthenticationStateProvider =>
            (CustomAuthenticationStateProvider)StateProvider;
    }
}
