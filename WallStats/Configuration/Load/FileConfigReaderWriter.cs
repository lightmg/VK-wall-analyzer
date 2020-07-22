using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WallStats.Helpers;

namespace WallStats.Configuration.Load
{
    public class FileConfigReaderWriter : IConfigReadWriter
    {
        private readonly JsonSerializer jsonSerializer =
            JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter() {DateTimeFormat = "dd.MM.yyyy HH:mm"},
                    new StringEnumConverter(),
                }
            });

        private const string ExampleConfigLogin = "Enter your login (email or phone number) here";
        private const string OptionsFileName = "vkws.options";
        public int ReadPriority => 1;

        public bool TryLoad(out AppConfig config)
        {
            config = default;
            var file = GetFile();
            if (!file.Exists)
                return false;
            using var fileStream = file.OpenRead();
            using var streamReader = new StreamReader(fileStream);
            using var jsonReader = new JsonTextReader(streamReader);
            config = jsonSerializer.Deserialize<AppConfig>(jsonReader);
            return config != null && config.AuthData.Login != ExampleConfigLogin;
        }

        public bool TrySave(AppConfig config)
        {
            var file = GetFile();
            using var fileStream = file.OpenWrite();
            using var streamWriter = new StreamWriter(fileStream);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            try
            {
                jsonSerializer.Serialize(jsonWriter, config);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private static FileInfo GetFile()
        {
            return FileSystemHelpers.GetFile(OptionsFileName);
        }
    }
}