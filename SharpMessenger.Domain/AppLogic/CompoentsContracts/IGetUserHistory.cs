﻿using SharpMessenger.Domain.Messages;

namespace SharpMessenger.Domain.AppLogic.ComponentsContracts
{
    internal interface IGetUserHistory
    {
        ValueTask<Dictionary<string, List<Message>>> GetUserHistoryAsync(string key);
        ValueTask SetUserHistory(Dictionary<string, List<Message>> history, string key);
    }
}
