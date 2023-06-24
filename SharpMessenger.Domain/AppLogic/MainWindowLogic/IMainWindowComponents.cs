using SharpMessenger.Domain.AppLogic.ComponentsContracts;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents;

internal interface IMainWindowComponents : IGetAuthState, IUserFriendsList, IGetUserHistory
{ }
