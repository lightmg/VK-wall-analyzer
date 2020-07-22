using WallStats.Bot.Enums;

namespace WallStats.Bot
{
    public class Target
    {
        public long Id { get; set; }
        public TargetType TargetType { get; set; }
        public string FullName { get; set; }

        public static Target Create(long id, string name, TargetType targetType) => new Target
        {
            Id = id, 
            FullName = name, 
            TargetType = targetType
        };
    }
}