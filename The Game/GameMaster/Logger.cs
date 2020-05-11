using System;

namespace GameMaster
{
    class Logger
    {
        public static void Error(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"[Error] : {text}");
            System.Console.ResetColor();
        }
        public static void Error(FormattableString text)
        {
            Error(text.ToString());
        }
        public static void Warning(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"[Warning] : {text}");
            System.Console.ResetColor();
        }
        public static void Warning(FormattableString text)
        {
            Warning(text.ToString());
        }
        public static void Log(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
        }
        public static void Log(FormattableString text)
        {
            Log(text.ToString());
        }
    }
}
