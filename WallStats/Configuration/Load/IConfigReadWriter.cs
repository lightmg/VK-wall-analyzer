namespace WallStats.Configuration.Load
{
    public interface IConfigReadWriter : IConfigReader
    {
        bool TrySave(AppConfig config);
    }
}