using System;
using System.Text;

namespace WallStats.Bot.IO
{
    public class ConsoleInputOutputSource : IInputOutputSource
    {
        public string Get(bool secureInput = false)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            if (!secureInput)
                return Console.ReadLine();

            var input = new StringBuilder();
            ConsoleKeyInfo pressedKey;
            while ((pressedKey = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    if (input.Length <= 0)
                        continue;
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b"); //Magical string that deletes last console character
                }
                else
                {
                    input.Append(pressedKey.KeyChar);
                    Console.Write('*');
                }
            }
            Console.WriteLine();

            return input.ToString();
        }

        public void Print(string message) => Console.WriteLine(message);
    }
}