using System;

namespace WallStats.Configuration
{
    public class UserAuthData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserAccessToken { get; set; }
        public DateTime TokenExpirationTime { get; set; }
    }
}