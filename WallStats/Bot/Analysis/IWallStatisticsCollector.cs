using System.Collections.Generic;
using WallStats.Bot.Api.Models;

namespace WallStats.Bot.Analysis
{
    public interface IWallStatisticsCollector
    {
        IEnumerable<string> Collect(PostModel[] wallPosts, Target target);
    }
}