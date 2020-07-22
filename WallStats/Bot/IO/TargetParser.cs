using System;
using System.Linq;
using System.Text.RegularExpressions;
using WallStats.Bot.Api.Models;
using WallStats.Bot.Api.Proto;
using WallStats.Bot.Enums;

namespace WallStats.Bot.IO
{
    public class TargetParser
    {
        private readonly Regex idRegex = new Regex(@"^(?<prefix>(?:id)|(?:public)|(?:club))(?<id>\d{1,19})$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IUserAuthVkApi userAuthVkApi;

        public TargetParser(IUserAuthVkApi userAuthVkApi)
        {
            this.userAuthVkApi = userAuthVkApi;
        }

        public TargetParsingResult TryParse(string rawInput, out Target target)
        {
            target = default;
            bool isGroup;
            if (TryExtractRequisites(rawInput, out var id, out var prefix))
                switch (prefix.ToLower())
                {
                    case "id":
                        isGroup = false;
                        break;
                    case "club":
                    case "public":
                        isGroup = true;
                        break;
                    default:
                        return TargetParsingResult.NotExists;
                }
            else if (userAuthVkApi.TryResolveScreenName(rawInput, out var response))
                switch (response.Type)
                {
                    case ObjectType.User:
                        isGroup = false;
                        id = response.Id;
                        break;
                    case ObjectType.Group:
                        isGroup = true;
                        id = response.Id;
                        break;
                    case ObjectType.Application:
                        return TargetParsingResult.ApplicationLink;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
                return TargetParsingResult.NotExists;

            var profiles = isGroup
                ? userAuthVkApi.GetGroups(id).Cast<VkObject>().ToArray()
                : userAuthVkApi.GetUsers(id).Cast<VkObject>().ToArray();

            if (profiles.Length == 0)
                return TargetParsingResult.NotExists;
            if (profiles.Length > 1)
                return TargetParsingResult.FewMatches;

            var targetProfile = profiles[0];
            if (!targetProfile.CanViewWall)
                return TargetParsingResult.WallClosed;

            target = Target.Create(targetProfile.Id, targetProfile.Name, isGroup ? TargetType.Group : TargetType.User);
            return targetProfile.CanPost
                ? TargetParsingResult.Ok
                : TargetParsingResult.WallReadOnly;
        }

        private bool TryExtractRequisites(string rawInput, out long id, out string prefix)
        {
            var matchResult = idRegex.Match(rawInput);
            if (!long.TryParse(matchResult.Groups["id"].Value, out id))
            {
                id = default;
                prefix = default;
                return false;
            }

            prefix = matchResult.Groups["prefix"].Value;
            return true;
        }
    }
}