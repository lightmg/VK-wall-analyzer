using WallStats.Bot.Enums;

namespace WallStats.Bot.Api.Models
{
    public class ApiResolveScreenNameResponse
    {
        public ObjectType Type { get; set; }
        public long Id { get; set; }
    }
}