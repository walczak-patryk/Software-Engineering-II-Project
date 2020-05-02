using CommunicationServerLibrary.Messages;
using System;

namespace CommunicationLibraryProj
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
