namespace WallStats.Bot.IO
{
    public interface IInputOutputSource
    {
        string Get(bool secureInput = false);
        void Print(string message);
    }
}