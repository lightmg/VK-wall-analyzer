using System.Collections.Generic;
using System.Linq;
using WallStats.Configuration.Load;

namespace WallStats.Configuration
{
    public class ConfigurationController
    {
        private readonly IConfigReader[] configReaders;
        private readonly IConfigReadWriter[] configReadWriters;

        public ConfigurationController(IEnumerable<IConfigReader> configReaders,
            IEnumerable<IConfigReadWriter> configReadWriters)
        {
            var rw = configReadWriters.ToArray();
            this.configReaders = configReaders.Union(rw)
                .OrderByDescending(x => x.ReadPriority)
                .ToArray();
            this.configReadWriters = rw.OrderByDescending(x => x.ReadPriority)
                .ToArray();
        }

        public AppConfig LoadConfigOrNull()
        {
            foreach (var reader in configReaders)
                if (reader.TryLoad(out var config))
                    return config;

            return null;
        }

        public bool TrySaveConfig(AppConfig appConfig)
        {
            return configReadWriters.Any(writer => writer.TrySave(appConfig));
        }
    }
}