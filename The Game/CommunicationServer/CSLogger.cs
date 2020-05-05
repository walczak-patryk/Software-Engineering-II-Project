using CommunicationServerLibrary;
using CommunicationServerLibrary.Messages;
using System;

namespace CommunicationServer
{
    internal static class CSLogger
    {
        internal static void Log(string text)
        {
            Logger.Log(text, ProgramType.CommunicationServer);
        }

        internal static void LogDebug(string text)
        {
            Logger.LogDebug(text, ProgramType.CommunicationServer);
        }

        internal static void LogError(string text)
        {
            Logger.LogError(text, ProgramType.CommunicationServer);
        }

        internal static void LogMessage(Message message, CSState value)
        {
            string text;
            text = $"\nServer received this message:\n";
            text += message.ToString();
            text += $"It was unexpected during {value} server state.\n";
            Logger.LogError(text, ProgramType.CommunicationServer);
        }
    }
}
