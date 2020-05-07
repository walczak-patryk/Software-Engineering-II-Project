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

        public static Message Deserialize(string message)
        {
            Message msg= System.Text.Json.JsonSerializer.Deserialize<Message>(message);
            try
            {
                switch (msg.action)
                {
                    case "GameSetup":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupMsg>(message);
                    case "GameSetup status":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupResMsg>(message);
                    case "Connect player":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerMsg>(message);
                    case "Connect player status":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerResMsg>(message);
                    case "Ready":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyMsg>(message);
                    case "Ready status":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyResMsg>(message);
                    case "Game start":
                        return System.Text.Json.JsonSerializer.Deserialize<GameStartMsg>(message);
                    case "Move":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveMsg>(message);
                    case "MoveStatus":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveResMsg>(message);
                    case "PickUp":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpMsg>(message);
                    case "PickUpStatus":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpResMsg>(message);
                    case "Test":
                        return System.Text.Json.JsonSerializer.Deserialize<TestMsg>(message);
                    case "TestStatus":
                        return System.Text.Json.JsonSerializer.Deserialize<TestResMsg>(message);
                    case "Place":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceMsg>(message);
                    case "PlaceStatus":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceResMsg>(message);
                    case "Discover":
                        return System.Text.Json.JsonSerializer.Deserialize<DiscoverMsg>(message);
                    case "DiscoverStatus":
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
