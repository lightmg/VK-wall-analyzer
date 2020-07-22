using WallStats.Bot.Enums;

namespace WallStats.Bot.Api.Models
{
    public class GroupModel : VkObject
    {
        public override ObjectType ObjectType => ObjectType.Group;
        public override bool CanViewWall => Publicity == GroupPublicity.Public || IsMember || IsAdmin;

        public GroupPublicity? Publicity { get; set; }
        public bool IsMember { get; set; }
        public bool IsAdmin { get; set; }
    }
}