using System;
using System.Collections.Generic;
using System.Text;
using CommunicationServerLibrary.Messages;

namespace CommunicationServerLibrary
{
    public static class MessageSerializer
    {
        public static string Serialize(Message message)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Message Deserialize(string message, string header)
        {
            try
            {
                switch (header)
                {
                    case "GameSetup message":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupMsg>(message);
                    case "GameSetup status message":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupResMsg>(message);
                    case "Connect player message":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerMsg>(message);
                    case "Connect player status message":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerResMsg>(message);
                    case "Ready message":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyMsg>(message);
                    case "Ready status message":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyResMsg>(message);
                    case "Game start message":
                        return System.Text.Json.JsonSerializer.Deserialize<GameStartMsg>(message);
                    case "Move message":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveMsg>(message);
                    case "MoveStatus message":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveResMsg>(message);
                    case "PickUp message":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpMsg>(message);
                    case "PickUpStatus message":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpResMsg>(message);
                    case "Test message":
                        return System.Text.Json.JsonSerializer.Deserialize<TestMsg>(message);
                    case "TestStatus message":
                        return System.Text.Json.JsonSerializer.Deserialize<TestResMsg>(message);
                    case "Place message":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceMsg>(message);
                    case "PlaceStatus message":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceResMsg>(message);
                    case "Discover messages":
                        return System.Text.Json.JsonSerializer.Deserialize<DiscoverMsg>(message);
                    case "DiscoverStatus messages":
                        return System.Text.Json.JsonSerializer.Deserialize<DiscoverResMsg>(message);
                    default: 
                        return new Message("Unknown");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
