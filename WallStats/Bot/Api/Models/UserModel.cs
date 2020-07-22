using WallStats.Bot.Enums;

namespace WallStats.Bot.Api.Models
{
    public class UserModel : VkObject
    {
        public override ObjectType ObjectType => ObjectType.User;
        public override bool CanViewWall => !IsClosed || CanAccessClosed;
        
        public bool IsClosed { get; set; }
        public bool CanAccessClosed { get; set; }
    }
}