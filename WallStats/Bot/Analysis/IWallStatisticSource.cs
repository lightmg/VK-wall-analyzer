using WallStats.Bot.Api.Models;

namespace WallStats.Bot.Analysis
{
    public interface IWallStatisticsSource
    {
        string Pseudonym { get; }
        string Get(PostModel[] wallPosts, Target target);
    }
}