using CommunicationServerLibrary.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibraryProj.Mocks
{
    public class MockConnectionClient : IDisposable
    {
        protected BlockingCollection<Message> messagesToClient = new BlockingCollection<Message>();
        protected BlockingCollection<Message> messagesFromClient = new BlockingCollection<Message>();
        public InjectableMockClient InjectableClient { get; protected set; }

        public MockConnectionClient()
        {
            InjectableClient = new InjectableMockClient(messagesToClient, messagesFromClient);
        }

        public void SendMessageFromClient(Message message)
        {
            messagesToClient.Add(message);
        }

        public void AddRangeOfMessagesToClient(IEnumerable<Message> messages)
        {
            foreach (var m in messages)
                messagesToClient.Add(m);
        }

        public Message TakeMessageSentToClient()
        {
            return messagesFromClient.Take();
        }

        public bool TryTakeMessageSentToClient(out Message message)
        {
            return messagesFromClient.TryTake(out message);
        }

        public Message SendMessageAndGetResponse(Message message)
        {
            messagesToClient.Add(message);
            return messagesFromClient.Take();
        }

        public void DisconnectClient()
        {
            InjectableClient.Disconnect();
        }

        public void Dispose()
        {
            messagesFromClient.Dispose();
            messagesToClient.Dispose();
        }
    }
}
