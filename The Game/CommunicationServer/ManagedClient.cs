using CommunicationLibraryProj;
using CommunicationServerLibrary.Messages;

namespace CommunicationServerProj
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
            return false;
        }

        public void Disconnect()
        {
        }

        public void SafeDisconnect()
        {
        }

        public bool SendMessage(Message msg)
        {
            return false;
        }

        public Message GetMessage()
        {
            return new Message("action");
        }
    }
}
