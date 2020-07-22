using System.Collections.Generic;
using System.Linq;
using WallStats.Bot.Api.Models;

namespace WallStats.Bot.Analysis
{
    public class WallStatisticsCollector : IWallStatisticsCollector
    {
        private readonly IEnumerable<IWallStatisticsSource> statisticSources;

        public WallStatisticsCollector(IEnumerable<IWallStatisticsSource> statisticSources)
        {
            this.statisticSources = statisticSources;
        }

        public IEnumerable<string> Collect(PostModel[] wallPosts, Target target)
        {
            return statisticSources.Select(x => x.Get(wallPosts, target));
        }
    }
}