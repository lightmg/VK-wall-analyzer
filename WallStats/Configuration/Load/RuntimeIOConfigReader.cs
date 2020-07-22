using System;
using System.Linq;
using WallStats.Bot.Enums;
using WallStats.Bot.IO;
using WallStats.Helpers;

namespace WallStats.Configuration.Load
{
    public class RuntimeIOConfigReader : IConfigReader
    {
        private readonly IInputOutputSource io;
        private readonly string[] availableStatisticsNames;

        public RuntimeIOConfigReader(IInputOutputSource io, string[] availableStatisticsNames)
        {
            this.io = io;
            this.availableStatisticsNames = availableStatisticsNames;
        }

        private const int DefaultPostsCount = 5;
        private const ApiGetPostsFilter DefaultPostsFilter = ApiGetPostsFilter.All;
        public int ReadPriority => 0;

        public bool TryLoad(out AppConfig config)
        {
            config = null;
            if (!TryRequestUlong("Enter APP_ID (not token)", out var appId))
                return false;

            var statsToCollect = io.RequestMultipleChoice("Select statistics to collect", availableStatisticsNames)
                .ToArray();
            if (statsToCollect.IsNullOrEmpty())
                return false;

            var login = io.RequestInput("Enter user login (email or phone number)");
            var password = io.RequestInput("Enter password", true);
            if (!TryRequestUlong("Enter count of posts to load", out var postsCount))
            {
                io.Print($"Can't recognize, will use default posts count: {DefaultPostsCount}");
                postsCount = DefaultPostsCount;
            }

            var postResultOnTargetWall = RequestBool("Do you wanna allow posting result on target's wall?");

            if (!io.TryRequestSingleEnumEntry<ApiGetPostsFilter>("Select filter to load posts", out var selected))
            {
                io.Print($"Can't recognize filter, will use default: {DefaultPostsFilter}");
                selected = DefaultPostsFilter;
            }

            var appToken = io.RequestInput("Enter app service key");
            var saveConfigAfterExecution = RequestBool("Do you wanna save config after execution?");

            config = new AppConfig
            {
                AuthData = new UserAuthData
                {
                    Login = login,
                    Password = password
                },
                PostResultOnTargetWall = postResultOnTargetWall,
                PostsToAnalyzeCount = postsCount,
                RequiredStatistics = statsToCollect,
                AppId = appId,
                PostsFilterMode = selected,
                AppToken = appToken,
                SaveAfterExecution = saveConfigAfterExecution
            };
            return true;
        }

        private bool TryRequestUlong(string message, out ulong value)
        {
            var input = io.RequestInput(message);
            return ulong.TryParse(input, out value);
        }

        private bool RequestBool(string message)
        {
            io.Print(message);
            return io.RequestInput("Print \"Y\" to enable").Equals("y", StringComparison.OrdinalIgnoreCase);
        }
    }
}