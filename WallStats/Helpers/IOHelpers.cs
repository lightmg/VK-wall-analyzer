using System;
using System.Collections.Generic;
using System.Linq;
using WallStats.Bot.IO;

namespace WallStats.Helpers
{
    public static class IOHelpers
    {
        public static string RequestInput(this IInputOutputSource io, string message, bool secureInput = false)
        {
            io.Print(message);
            return io.Get(secureInput);
        }

        public static IEnumerable<T> RequestMultipleChoice<T>(this IInputOutputSource io, string message,
            params T[] options)
        {
            io.Print(message);
            io.Print("Enter index of options to select, split variants with space");
            for (var i = 0; i < options.Length; i++)
                io.Print($"{i}. {options[i].ToString()}");
            var splitInput = io.Get()
                .Trim()
                .Split(' ')
                .OrderBy(x => x);
            string lastElem = null;
            foreach (var inputEntry in splitInput)
            {
                // 1st condition makes output values unique, works because we ordered array before,
                // and if some elements are repeating, they will be near to each other
                if (inputEntry == lastElem || !uint.TryParse(inputEntry, out var num) || num >= options.Length)
                    continue;
                lastElem = inputEntry;
                yield return options[num];
            }
        }

        public static bool TryRequestSingleEnumEntry<T>(this IInputOutputSource io, string message, out T selected)
            where T : struct, Enum
        {
            var options = EnumHelpers.GetValues<T>().ToArray();
            io.Print(message);
            io.Print("Enter index of single option to select");
            for (var i = 0; i < options.Length; i++)
                io.Print($"{i}. {options[i].ToString()}");
            if (!uint.TryParse(io.Get(), out var index) || index >= options.Length)
            {
                selected = default;
                return false;
            }
            selected = options[index];
            return true;
        }
    }
}