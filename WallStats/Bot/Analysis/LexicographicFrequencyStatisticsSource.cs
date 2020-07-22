using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using WallStats.Bot.Api.Models;

namespace WallStats.Bot.Analysis
{
    public class LexicographicFrequencyStatisticsSource : IWallStatisticsSource
    {
        public string Pseudonym => "lexicographic";

        public string Get(PostModel[] wallPosts, Target target)
        {
            var absoluteFrequencies = new ConcurrentDictionary<char, uint>();
            foreach (var post in wallPosts)
            {
                var text = post.Text;
                foreach (var s in text)
                {
                    var symbol = char.ToLower(s);
                    if (!IsSymbolValid(symbol))
                        continue;
                    if (absoluteFrequencies.ContainsKey(symbol))
                        absoluteFrequencies[symbol] = absoluteFrequencies[symbol] + 1;
                    else absoluteFrequencies[symbol] = 1;
                }
            }

            var totalSymbolsCount = absoluteFrequencies.Sum(x => x.Value);
            var relativeFrequencies = absoluteFrequencies
                .OrderByDescending(x => x.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value / (double) totalSymbolsCount);

            return $"{target.FullName}, статистика для последних {wallPosts.Length} постов: " +
                   $"{JsonConvert.SerializeObject(relativeFrequencies, Formatting.Indented)}";
        }

        private bool IsSymbolValid(char symbol)
        {
            return char.IsLetterOrDigit(symbol)
                   || !char.IsPunctuation(symbol)
                   && !char.IsSeparator(symbol)
                   && !char.IsWhiteSpace(symbol);
        }
    }
}