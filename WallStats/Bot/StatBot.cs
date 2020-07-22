using System;
using WallStats.Bot.Analysis;
using WallStats.Bot.Api.Proto;
using WallStats.Bot.IO;
using WallStats.Configuration;
using WallStats.Helpers;

namespace WallStats.Bot
{
    public class StatBot
    {
        private readonly ConfigurationController configurationController;
        private readonly IInputOutputSource io;
        private readonly IAppAuthVkApi vkApi;
        private readonly IWallStatisticsCollector wallStatisticsCollector;

        public StatBot(ConfigurationController configurationController,
            IAppAuthVkApi vkApi,
            IWallStatisticsCollector wallStatisticsCollector, IInputOutputSource io)
        {
            this.configurationController = configurationController;
            this.vkApi = vkApi;
            this.wallStatisticsCollector = wallStatisticsCollector;
            this.io = io;
        }

        public void Run()
        {
            const string requestInputMessage = "Enter shorten link or Id of target (group or user), for example: " +
                                               "\"id123141\", \"public1555\", \"club1234\", \"lightmg\", \"banktochka\"\n" +
                                               "Or just press Enter to exit";

            var config = configurationController.LoadConfigOrNull();
            if (config == null)
            {
                io.Print("Configuration could not be loaded by any of known ways");
                return;
            }

            using var userAuthVkApi = vkApi.AuthorizeUser(config.AuthData, config.AppId, out var authSuccessful);
            if (!authSuccessful)
            {
                io.Print("Unsuccessful authorization! Bot shutting down...");
                return;
            }

            io.Print($"Logged in as {userAuthVkApi.AuthData.Username} [id{userAuthVkApi.AuthData.UserId}]");
            var targetParser = new TargetParser(userAuthVkApi);
            string rawInput;
            while (!(rawInput = io.RequestInput(requestInputMessage)).IsNullOrEmpty())
            {
                var hasCreatePostPermission = false;
                switch (targetParser.TryParse(rawInput, out var target))
                {
                    case TargetParsingResult.WallReadOnly:
                        io.Print("Target Ok, but wall is read-only, result will be not posted on target's profile");
                        break;
                    case TargetParsingResult.Ok:
                        hasCreatePostPermission = true;
                        break;
                    case TargetParsingResult.NotExists:
                        io.Print("Can't find user or group with this id/link");
                        continue;
                    case TargetParsingResult.WallClosed:
                        io.Print("Target is OK, but no access to wall, choose another target");
                        continue;
                    case TargetParsingResult.ApplicationLink:
                        io.Print("Id/link recognized, it's for application, choose another target");
                        continue;
                    case TargetParsingResult.FewMatches:
                        throw new InvalidOperationException("Wow! There is few VK users/groups with this link/id");
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                io.Print($"Recognized target: {target.FullName}");
                io.Print(config.PostResultOnTargetWall
                    ? "Executing in read-only mode"
                    : "Will send stats to target's wall");
                var posts = userAuthVkApi.GetPosts(target, config.PostsToAnalyzeCount, config.PostsFilterMode);
                foreach (var statisticsEntry in wallStatisticsCollector.Collect(posts, target))
                {
                    io.Print(statisticsEntry);
                    if (config.PostResultOnTargetWall && hasCreatePostPermission &&
                        userAuthVkApi.TryCreatePost(target, statisticsEntry))
                        io.Print($"Statistics entry sent to wall of target {target.FullName}");
                }
            }

            io.Print("Logged out");
            var savingResult = config.SaveAfterExecution && configurationController.TrySaveConfig(config);
            io.Print($"Configuration {(savingResult ? string.Empty : "not ")}saved");
        }
    }
}