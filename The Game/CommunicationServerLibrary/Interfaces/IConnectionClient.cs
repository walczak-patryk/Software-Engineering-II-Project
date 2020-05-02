using CommunicationServerLibrary.Messages;
using System;

namespace CommunicationServerLibrary.Interfaces
{
    public interface IConnectionClient
    {
        bool Connect(string IP, int port);

        void Disconnect();

        void SafeDisconnect();

        bool SendMessage(Message message);

        Message GetMessage();

        bool IsConnected();

        event EventHandler ConnectionError;

    }
}
