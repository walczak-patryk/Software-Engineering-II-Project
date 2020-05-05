using System;
using System.IO;

namespace CommunicationServerLibrary
{
    public enum ProgramType
    {
        Player,
        CommunicationServer,
        GameMaster,
        Default
    }

    public enum LogLevelType
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public static class Logger
    {

#if DEBUG
        static public LogLevelType LogLevel = LogLevelType.Debug;
        static public LogLevelType LogFileLevel = LogLevelType.Info;
#else
        static public LogLevelType LogLevel = LogLevelType.Info;
        static public LogLevelType LogFileLevel = LogLevelType.Warning;
#endif


        static private readonly object locker = new object();

        public static void Log(string message, ProgramType programType = ProgramType.Default)
        {
            InternalLog(message, LogLevelType.Info, programType);
        }

        public static void LogDebug(string message, ProgramType programType = ProgramType.Default)
        {
            InternalLog(message, LogLevelType.Debug, programType);
        }


        public static void LogWarning(string message, ProgramType programType = ProgramType.Default)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            InternalLog(message, LogLevelType.Warning, programType);
            Console.ResetColor();
        }

        public static void LogException(Exception exception, ProgramType programType = ProgramType.Default)
        {
            string errorMessage = $"{DateTime.Now}:{exception.GetType().ToString()} was thrown.\nValue: {exception.HResult}\nMessage: {exception.Message}\nSource: {exception.StackTrace}\n\n";
            LogError(errorMessage, programType);
        }
     
        public static void LogError(string errorMessage, ProgramType programType = ProgramType.Default)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            InternalLog(errorMessage, LogLevelType.Error, programType);
            Console.ResetColor();
        }

        #region internal methods

        private static void InternalLog(string message, LogLevelType messageLevel, ProgramType programType)
        {
            if (LogLevelEnough(messageLevel))
            {
                WriteToConsole(message);
            }

            if (LogFileLevelEnough(messageLevel))
            {
                WriteToFile(message, programType);
            }
        }

        private static void WriteToConsole(string message)
        {
            Console.WriteLine(FormatMessage(message));
        }

        private static void WriteToFile(string message, ProgramType callerType)
        {
            string filename = "";


            switch (callerType)
            {
                case ProgramType.Player:
                    filename = "AgentLog.txt";
                    break;

                case ProgramType.GameMaster:
                    filename = "GMLog.txt";
                    break;

                case ProgramType.CommunicationServer:
                    filename = "CSLog.txt";
                    break;

                case ProgramType.Default:
                    filename = "Log.txt";
                    break;

            }

            string[] loggedMessage = new string[]
            {
                FormatMessage(message)
            };

            lock (locker)
            {
                File.AppendAllLines(filename, loggedMessage);
            };
        }

        private static string FormatMessage(string message)
        {
            return $"{DateTime.Now}: {message}";
        }

        private static bool LogLevelEnough(LogLevelType messageLevel)
        {
            return messageLevel >= LogLevel;
        }
        private static bool LogFileLevelEnough(LogLevelType messageLevel)
        {
            return messageLevel >= LogFileLevel;
        }
        #endregion
    }
}
