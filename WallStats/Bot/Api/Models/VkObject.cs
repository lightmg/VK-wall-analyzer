using WallStats.Bot.Enums;

namespace WallStats.Bot.Api.Models
{
    public abstract class VkObject
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public bool CanPost { get; set; }
        
        public abstract ObjectType ObjectType { get; }
        public abstract bool CanViewWall { get; }
    }
}