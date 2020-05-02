using CommunicationServerLibrary.Messages;
using System;
using System.Net;

namespace CommunicationServerLibrary.Interfaces
{
    public interface IConnectionClient
    {
        bool Connect(IPAddress IP, int port);

        void Disconnect();

        void SafeDisconnect();

        bool SendMessage(Message message);

        Message GetMessage();

        bool IsConnected();

        event EventHandler ConnectionError;

    }
}
