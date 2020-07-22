using Newtonsoft.Json;
using WallStats.Bot.Enums;

namespace WallStats.Configuration
{
    public class AppConfig
    {
        public UserAuthData AuthData { get; set; }
        public string[] RequiredStatistics { get; set; }
        public ulong AppId { get; set; }
        public string AppToken { get; set; }
        public ulong PostsToAnalyzeCount { get; set; }
        public bool PostResultOnTargetWall { get; set; }
        public ApiGetPostsFilter PostsFilterMode { get; set; }
        [JsonIgnore] public bool SaveAfterExecution { get; set; } 
    }
}