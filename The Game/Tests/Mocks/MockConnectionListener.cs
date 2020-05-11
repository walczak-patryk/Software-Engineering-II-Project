using CommunicationServerLibrary.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Tests
{
    class MockConnectionListener : IConnectionListener, IDisposable
    {
        private BlockingCollection<IConnectionClient> connectionClients = new BlockingCollection<IConnectionClient>();
        public bool IsListening { get; private set; }

        public MockConnectionListener(IEnumerable<IConnectionClient> connectionClients)
        {
            foreach (var client in connectionClients)
            {
                this.connectionClients.Add(client);
            }
        }
        public MockConnectionListener()
        {
        }

        public IConnectionClient Accept()
        {
            if (!IsListening)
            {
                return null;
            }

            return connectionClients.Take();
        }

        public void StartListening(IPAddress IP = null, int port = -1)
        {
            IsListening = true;
        }

        public void StopListening()
        {
            IsListening = false;
            connectionClients.Add(null);
        }

        public void AddClient(IConnectionClient client)
        {
            connectionClients.Add(client);
        }

        public void Dispose()
        {
            connectionClients.Dispose();
        }
    }
}
