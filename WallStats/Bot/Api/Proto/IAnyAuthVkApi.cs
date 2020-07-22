using System;
using WallStats.Bot.Api.Models;
using WallStats.Bot.Enums;

namespace WallStats.Bot.Api.Proto
{
    /// <summary>
    /// Contains VK API methods that will work with any authorization
    /// </summary>
    public interface IAnyAuthVkApi : IDisposable
    {
        bool TryResolveScreenName(string screenName, out ApiResolveScreenNameResponse result);
        PostModel[] GetPosts(Target target, ulong postsCount, ApiGetPostsFilter mode = ApiGetPostsFilter.All);
        UserModel[] GetUsers(params long[] ids);
        GroupModel[] GetGroups(params long[] ids);
        string GetTargetName(Target target);
        void LogOut();
    }
}