using System;
using VkNet.Model;
using VkNet.Model.Attachments;
using WallStats.Bot.Api.Models;
using GroupPublicity = WallStats.Bot.Enums.GroupPublicity;

namespace WallStats.Helpers
{
    public static class VkNetModelsConverters
    {
        public static PostModel ToModel(this Post post)
        {
            return new PostModel
            {
                Text = post.Text
            };
        }

        public static UserModel ToModel(this User user)
        {
            return new UserModel
            {
                Name = $"{user.FirstName} {user.LastName}",
                Id = user.Id,
                IsClosed = user.IsClosed ?? true,
                CanAccessClosed = user.CanAccessClosed ?? false,
                CanPost = user.CanPost
            };
        }

        public static GroupModel ToModel(this Group group)
        {
            return new GroupModel
            {
                Name = group.Name,
                Id = group.Id,
                IsAdmin = group.IsAdmin,
                IsMember = group.IsMember ?? false,
                CanPost = group.CanPost,
                Publicity = group.IsClosed switch
                {
                    VkNet.Enums.GroupPublicity.Public => GroupPublicity.Public,
                    VkNet.Enums.GroupPublicity.Closed => GroupPublicity.Closed,
                    VkNet.Enums.GroupPublicity.Private => GroupPublicity.Private,
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
        }
    }
}