using System;
using System.IO;
using System.Net;
using VkNet.Utils.AntiCaptcha;
using WallStats.Bot.IO;
using WallStats.Helpers;

namespace WallStats.Bot.Api
{
    public class CaptchaSolver : ICaptchaSolver
    {
        private readonly IInputOutputSource io;

        public CaptchaSolver(IInputOutputSource io)
        {
            this.io = io;
        }

        public string Solve(string url)
        {
            var captchaFile = FileSystemHelpers.GetFile("captcha.jpg");
            new WebClient().DownloadFile(new Uri(url), captchaFile.FullName);
            captchaFile.OpenFile();
            var solved = io.RequestInput("Please, solve captcha and enter result here");
            if (captchaFile.Exists)
            {
                try
                {
                    captchaFile.Delete();
                }
                catch (IOException)
                {
                    io.Print("File is opened somewhere else and will be not deleted. " +
                             $"You can delete it manually, path is: {captchaFile.FullName}");
                }
            }
            return solved;
        }

        public void CaptchaIsFalse()
        {
            io.Print("Last solved captcha is solved incorrectly");
        }
    }
}