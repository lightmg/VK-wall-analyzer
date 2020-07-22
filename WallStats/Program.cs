using System;
using System.Linq;
using VkNet;
using WallStats.Bot;
using WallStats.Bot.Analysis;
using WallStats.Bot.Api;
using WallStats.Bot.IO;
using WallStats.Configuration;
using WallStats.Configuration.Load;

namespace WallStats
{
    public class Program
    {
        public static void Main()
        {
            #region Can be replaced with any DI container

            var ioSource = new ConsoleInputOutputSource();
            var statisticSources = new IWallStatisticsSource[] {new LexicographicFrequencyStatisticsSource()};
            var availableStatisticsNames = statisticSources.Select(s => s.Pseudonym).ToArray();
            var configLoaders = new IConfigReader[] {new RuntimeIOConfigReader(ioSource, availableStatisticsNames)};
            var apiAccessor = new AppAuthVkApi(
                () => new VkApi(null, new CaptchaSolver(ioSource)),
                "ae07bd0eae07bd0eae07bd0e67ae74a9b3aae07ae07bd0ef1167d1e5d7b1df7b87288b7", 
                ioSource);
            var configurationController =
                new ConfigurationController(configLoaders, new[] {new FileConfigReaderWriter()});
            var statisticsCollector = new WallStatisticsCollector(statisticSources);
            var bot = new StatBot(configurationController, apiAccessor, statisticsCollector, ioSource);

            #endregion

            bot.Run();
            Console.ReadLine();
        }
    }
}