using WallStats.Bot.Api.Models;

namespace WallStats.Bot.Api.Proto
{
    /// <summary>
    /// Contains VK API methods that requires authorization with user token
    /// </summary>
    public interface IUserAuthVkApi : IAnyAuthVkApi
    {
        VkAuthData AuthData { get; }

        bool TryCreatePost(Target target, string text);
    }
}