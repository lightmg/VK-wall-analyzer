using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using WallStats.Bot.Api.Models;
using WallStats.Bot.Api.Proto;
using WallStats.Bot.Enums;

namespace WallStats.Bot.Api
{
    public class UserAuthVkApi : AnyAuthVkApi, IUserAuthVkApi
    {
        private const int DailyPostsLimitReachedErrorCode = 214;
        private readonly IVkApi vkApi;

        public UserAuthVkApi(IVkApi userAuthorizedVkApi, VkAuthData authData) : base(userAuthorizedVkApi)
        {
            AuthData = authData;
            vkApi = userAuthorizedVkApi;
        }

        public VkAuthData AuthData { get; }

        public bool TryCreatePost(Target target, string text)
        {
            try
            {
                vkApi.Wall.Post(new WallPostParams
                {
                    Message = text,
                    OwnerId = target.TargetType == TargetType.Group
                        ? -target.Id
                        : target.Id,
                    MuteNotifications = true
                });
                return true;
            }
            catch (VkApiException e)
            {
                if (e.ErrorCode == DailyPostsLimitReachedErrorCode)
                    return false;
                throw;
            }
        }
    }
}