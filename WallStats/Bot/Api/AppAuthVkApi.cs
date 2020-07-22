using System;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using WallStats.Bot.Api.Models;
using WallStats.Bot.Api.Proto;
using WallStats.Bot.IO;
using WallStats.Configuration;
using WallStats.Helpers;

namespace WallStats.Bot.Api
{
    public class AppAuthVkApi : AnyAuthVkApi, IAppAuthVkApi
    {
        private readonly IVkApi appAuthorizedVkApi;
        private readonly Func<IVkApi> vkApiFactory;
        private readonly IInputOutputSource io;

        public AppAuthVkApi(Func<IVkApi> vkApiFactory, string appToken, IInputOutputSource io)
            : this(vkApiFactory.Invoke(), appToken, io)
        {
            this.vkApiFactory = vkApiFactory;
        }

        private AppAuthVkApi(IVkApi vkApi, string appToken, IInputOutputSource io) : base(vkApi)
        {
            if (!TryAuthorize(vkApi, new ApiAuthParams {AccessToken = appToken}))
                throw new InvalidOperationException("Unsuccessful app authorization");
            appAuthorizedVkApi = vkApi;
            this.io = io;
        }

        public VkApiTokenCheckResult CheckToken(string token)
        {
            var response = appAuthorizedVkApi.Secure.CheckToken(token);

            return new VkApiTokenCheckResult
            {
                Token = token,
                ExpirationTime = response.Expire,
                UserId = response.UserId
            };
        }

        public IUserAuthVkApi AuthorizeUser(UserAuthData authData, ulong appId, out bool isSuccessful)
        {
            var userAuthorizedVkApi = vkApiFactory.Invoke();
            var apiAuthParams = authData.UserAccessToken.IsNullOrEmpty() ||
                                authData.TokenExpirationTime <= DateTime.Now ||
                                CheckToken(authData.UserAccessToken).IsExpired
                ? new ApiAuthParams
                {
                    Login = authData.Login,
                    Password = authData.Password,
                    ApplicationId = appId,
                    Settings = Settings.Wall,
                    TwoFactorAuthorization = () => io.RequestInput("Enter 2FA Code: ", true)
                }
                : new ApiAuthParams
                {
                    AccessToken = authData.UserAccessToken,
                    ApplicationId = appId,
                    Settings = Settings.Wall,
                };
            isSuccessful = TryAuthorize(userAuthorizedVkApi, apiAuthParams);

            if (isSuccessful == false)
                return null;

            var token = userAuthorizedVkApi.Token;
            var tokenExpirationTime = appAuthorizedVkApi.Secure.CheckToken(token).Expire;
            authData.UserAccessToken = token;
            authData.TokenExpirationTime = tokenExpirationTime;

            var currentUser = userAuthorizedVkApi.Users.Get(Enumerable.Empty<string>(), null, NameCase.Nom)
                .FirstOrDefault();
            if (currentUser == null)
                throw new VkApiException("Didn't got current user from API (users.get method)");

            return new UserAuthVkApi(userAuthorizedVkApi, new VkAuthData
            {
                Token = token,
                UserId = currentUser.Id,
                TokenExpirationTime = tokenExpirationTime,
                Username = $"{currentUser.FirstName} {currentUser.LastName}"
            });
        }

        private bool TryAuthorize(IVkApi vkApi, IApiAuthParams apiAuthParams)
        {
            try
            {
                vkApi.Authorize(apiAuthParams); //FIXME капча, будь она неладна
            }
            catch (VkAuthorizationException authException)
            {
                io.Print($"Authorization unsuccessful: {authException.Message}");
                return false;
            }
            catch (Exception e)
            {
                io.Print("Unhandled exception occured");
                io.Print(e.ToString());
                throw;
            }

            return true;
        }
    }
}