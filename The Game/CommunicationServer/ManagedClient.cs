using CommunicationServerLibrary.Interfaces;
using CommunicationServerLibrary.Messages;

namespace CommunicationServer
{
    internal class ManagedClient
    {
        private readonly object locker = new object();
        private ThreadSafeVariable<bool> isInGame = new ThreadSafeVariable<bool>();
        private IConnectionClient client;

        public ManagedClient(IConnectionClient client, int id)
        {
            this.client = client;
            this.Id = id;
        }

        public int Id { get; }

        public bool IsInGame
        {
            get { return isInGame.Value; }

            set { isInGame.Value = value; }
        }

        public bool IsConnected()
        {
            lock (locker)
            {
                return client.IsConnected();
            }
        }

        public void Disconnect()
        {
            lock (locker)
            {
                if (client.IsConnected())
                {
                    client.Disconnect();
                    System.Console.WriteLine($" client {Id} marked to be disconnected ");
                }
            }
        }

        public void SafeDisconnect()
        {
            lock (locker)
            {
                if (client.IsConnected())
                {
                    client.SafeDisconnect();
                    System.Console.WriteLine($"### client {Id} marked to be safe disconnected");
                }
            }
        }

        public bool SendMessage(Message msg)
        {
            lock (locker)
            {
                bool sent = client.SendMessage(msg);

                if (sent)
                    System.Console.WriteLine($"[client {Id}] sent message {msg.GetType().Name}");

                return sent;
            }
        }

        public Message GetMessage()
        {
            Message msg = client.GetMessage();

            System.Console.WriteLine($"[client {Id}] received message {msg.GetType().Name}");

            return msg;
        }
    }
}
