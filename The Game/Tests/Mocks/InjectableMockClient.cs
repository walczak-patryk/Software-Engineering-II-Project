using CommunicationServerLibrary.Interfaces;
using CommunicationServerLibrary.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CommunicationLibraryProj.Mocks
{
    public class InjectableMockClient : IConnectionClient
    {
        public bool isConnected = true;
        private BlockingCollection<Message> input;
        private BlockingCollection<Message> output;

        public InjectableMockClient(BlockingCollection<Message> input, BlockingCollection<Message> output)
        {
            this.input = input;
            this.output = output;
        }

        #region IConnectionClient implementation
        public bool Connect(IPAddress IP, int port)
        {
            return isConnected = true;
        }

        public void Disconnect()
        {
            isConnected = false;
        }

        public void SafeDisconnect()
        {
            Disconnect();
        }

        public Message GetMessage()
        {
            return input.Take();
        }

        public bool SendMessage(Message message)
        {
            if (!isConnected)
                return false;

            output.Add(message);
            return true;
        }

        public bool IsConnected()
        {
            return isConnected;
        }
        public event EventHandler ConnectionError;
        #endregion
    }
}
