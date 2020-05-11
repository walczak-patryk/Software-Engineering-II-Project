using System;
using System.Collections.Generic;
using System.Text;
using CommunicationServerLibrary.Messages;
using Newtonsoft.Json;

namespace CommunicationServerLibrary
{
    public static class MessageSerializer
    {
        public static string Serialize(Message message)
        {
            try
            {
                return JsonConvert.SerializeObject(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Message Deserialize(string message)
        {
            Message msg= JsonConvert.DeserializeObject<Message>(message);
            try
            {
                switch (msg.action)
                {
                    case "setup":
                        return JsonConvert.DeserializeObject<SetupMsg>(message);
                    case "setup status":
                        return JsonConvert.DeserializeObject<SetupResMsg>(message);
                    case "connect":
                        return JsonConvert.DeserializeObject<ConnectPlayerMsg>(message);
                    case "connect status":
                        return JsonConvert.DeserializeObject<ConnectPlayerResMsg>(message);
                    case "ready":
                        return JsonConvert.DeserializeObject<ReadyMsg>(message);
                    case "ready status":
                        return JsonConvert.DeserializeObject<ReadyResMsg>(message);
                    case "start":
                        return JsonConvert.DeserializeObject<GameStartMsg>(message);
                    case "move":
                        return JsonConvert.DeserializeObject<MoveMsg>(message);
                    case "move status":
                        return JsonConvert.DeserializeObject<MoveResMsg>(message);
                    case "pickup":
                        return JsonConvert.DeserializeObject<PickUpMsg>(message);
                    case "pickup status":
                        return JsonConvert.DeserializeObject<PickUpResMsg>(message);
                    case "test":
                        return JsonConvert.DeserializeObject<TestMsg>(message);
                    case "test status":
                        return JsonConvert.DeserializeObject<TestResMsg>(message);
                    case "place":
                        return JsonConvert.DeserializeObject<PlaceMsg>(message);
                    case "place status":
                        return JsonConvert.DeserializeObject<PlaceResMsg>(message);
                    case "discover":
                        return JsonConvert.DeserializeObject<DiscoverMsg>(message);
                    case "discover status":
                        return JsonConvert.DeserializeObject<DiscoverResMsg>(message);
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
