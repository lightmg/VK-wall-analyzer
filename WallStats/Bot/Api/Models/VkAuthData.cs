using System;

namespace WallStats.Bot.Api.Models
{
    public class VkAuthData
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public long UserId { get; set; }
        public DateTime TokenExpirationTime { get; set; }
    }
}