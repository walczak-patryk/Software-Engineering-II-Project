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
                    case "setup":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupMsg>(message);
                    case "setup status":
                        return System.Text.Json.JsonSerializer.Deserialize<SetupResMsg>(message);
                    case "connect":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerMsg>(message);
                    case "connect status":
                        return System.Text.Json.JsonSerializer.Deserialize<ConnectPlayerResMsg>(message);
                    case "ready":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyMsg>(message);
                    case "ready status":
                        return System.Text.Json.JsonSerializer.Deserialize<ReadyResMsg>(message);
                    case "start":
                        return System.Text.Json.JsonSerializer.Deserialize<GameStartMsg>(message);
                    case "move":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveMsg>(message);
                    case "move status":
                        return System.Text.Json.JsonSerializer.Deserialize<MoveResMsg>(message);
                    case "pickup":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpMsg>(message);
                    case "pickup status":
                        return System.Text.Json.JsonSerializer.Deserialize<PickUpResMsg>(message);
                    case "test":
                        return System.Text.Json.JsonSerializer.Deserialize<TestMsg>(message);
                    case "test status":
                        return System.Text.Json.JsonSerializer.Deserialize<TestResMsg>(message);
                    case "place":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceMsg>(message);
                    case "place status":
                        return System.Text.Json.JsonSerializer.Deserialize<PlaceResMsg>(message);
                    case "discover":
                        return System.Text.Json.JsonSerializer.Deserialize<DiscoverMsg>(message);
                    case "discover status":
                        return System.Text.Json.JsonSerializer.Deserialize<DiscoverResMsg>(message);
                    default: 
                        return new Message("unknown");
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
