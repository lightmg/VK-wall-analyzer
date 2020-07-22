using WallStats.Bot.Api.Models;
using WallStats.Configuration;

namespace WallStats.Bot.Api.Proto
{
    /// <summary>
    /// Contains VK API methods that will work with app token
    /// </summary>
    public interface IAppAuthVkApi : IAnyAuthVkApi
    {
        VkApiTokenCheckResult CheckToken(string token);
        IUserAuthVkApi AuthorizeUser(UserAuthData authData, ulong appId, out bool isSuccessful);
    }
}