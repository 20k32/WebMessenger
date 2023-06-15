using Blazored.SessionStorage;
using System.Text;
using System.Text.Json;

namespace SharpMessegner.ChatUserInterface.Extensions
{
    public static class SessionStorageServiceExtensions
    {
        public static async Task SaveItemEncryptedAsync<T>(this ISessionStorageService sessionStorageService, string key, T item)
        {
            string jsonItem = JsonSerializer.Serialize(item);
            byte[] jsonItemBytes = Encoding.UTF8.GetBytes(jsonItem);
            string base64JsonItem = Convert.ToBase64String(jsonItemBytes);

            await sessionStorageService.SetItemAsync(key, base64JsonItem);
        }

        public static async Task<T> ReadEncryptedItemAsync<T>(this ISessionStorageService sessionStorageService, string key)
        {
            string base64JsonItem = await sessionStorageService.GetItemAsync<string>(key);
            byte[] jsonItemBytes = Convert.FromBase64String(base64JsonItem);
            string jsonItem = Encoding.UTF8.GetString(jsonItemBytes);
            T resultItem = JsonSerializer.Deserialize<T>(jsonItem)!;

            return resultItem;
        }
    }
}
