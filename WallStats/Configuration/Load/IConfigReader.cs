namespace WallStats.Configuration.Load
{
    public interface IConfigReader
    {
        int ReadPriority { get; }
        bool TryLoad(out AppConfig config);
    }
}