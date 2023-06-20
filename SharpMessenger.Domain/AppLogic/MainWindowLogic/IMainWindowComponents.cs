using SharpMessenger.Domain.AppLogic.ComponentsContracts;

namespace SharpMessenger.Domain.AppLogic.MainWindowLogic.MainWindowComponents;

public interface IMainWindowComponents : IGetAuthState, IUserFriendsList, IGetUserHistory
{ }
