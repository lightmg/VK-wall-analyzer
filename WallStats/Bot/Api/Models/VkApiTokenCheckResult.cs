using System;

namespace WallStats.Bot.Api.Models
{
    public class VkApiTokenCheckResult
    {
        public bool IsExpired =>
            ExpirationTime != new DateTime(1970, 01, 01, 00, 00, 00) && DateTime.Now >= ExpirationTime;

        public DateTime ExpirationTime { get; set; }
        public string Token { get; set; }
        public ulong UserId { get; set; }
    }
}