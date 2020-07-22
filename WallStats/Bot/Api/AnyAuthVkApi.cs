using System;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;
using WallStats.Bot.Api.Models;
using WallStats.Bot.Api.Proto;
using WallStats.Bot.Enums;
using WallStats.Helpers;

namespace WallStats.Bot.Api
{
    public abstract class AnyAuthVkApi : IAnyAuthVkApi
    {
        private readonly IVkApi vkApi;

        protected AnyAuthVkApi(IVkApi vkApi)
        {
            this.vkApi = vkApi;
        }

        public bool TryResolveScreenName(string screenName, out ApiResolveScreenNameResponse result)
        {
            var response = vkApi.Utils.ResolveScreenName(screenName);
            if (response?.Id == null)
            {
                result = null;
                return false;
            }

            result = new ApiResolveScreenNameResponse
            {
                Id = response.Id.Value,
                Type = response.Type switch
                {
                    VkObjectType.User => ObjectType.User,
                    VkObjectType.Group => ObjectType.Group,
                    VkObjectType.Application => ObjectType.Application,
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
            return true;
        }


        public PostModel[] GetPosts(Target target, ulong postsCount,
            ApiGetPostsFilter mode = ApiGetPostsFilter.All)
        {
            var wallFilter = mode switch
            {
                ApiGetPostsFilter.OwnerOnly => WallFilter.Owner,
                ApiGetPostsFilter.OthersOnly => WallFilter.Others,
                ApiGetPostsFilter.All => WallFilter.All,
                _ => throw new ArgumentOutOfRangeException()
            };

            var response = vkApi.Wall.Get(new WallGetParams
            {
                Count = postsCount,
                Extended = false,
                Filter = wallFilter,
                // OwnerId should be negative to get Group wall posts: https://vk.com/dev/wall.get
                OwnerId = target.TargetType == TargetType.Group
                    ? -target.Id
                    : target.Id
            });

            return response.WallPosts
                .EmptyIfNull()
                .Select(p => p.ToModel())
                .ToArray();
        }

        public UserModel[] GetUsers(params long[] ids)
        {
            return vkApi.Users.Get(ids, ProfileFields.CanPost)
                .EmptyIfNull()
                .Select(u => u.ToModel())
                .ToArray();
        }

        public GroupModel[] GetGroups(params long[] ids)
        {
            var groupIds = ids.Select(x => x.ToString());
            return vkApi.Groups.GetById(groupIds, null, GroupsFields.CanPost)
                .EmptyIfNull()
                .Select(x => x.ToModel())
                .ToArray();
        }

        public string GetTargetName(Target target)
        {
            var targetObject = target.TargetType switch
            {
                TargetType.Group => GetGroups(target.Id).SingleOrDefault() as VkObject,
                TargetType.User => GetUsers(target.Id).SingleOrDefault() as VkObject,
                _ => throw new ArgumentOutOfRangeException()
            };

            return targetObject?.Name;
        }

        public void LogOut() => vkApi.LogOut();
        public void Dispose() => LogOut();
    }
}