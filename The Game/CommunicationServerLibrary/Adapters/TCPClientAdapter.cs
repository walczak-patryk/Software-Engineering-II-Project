using System;
using System.Net.Sockets;
using CommunicationServerLibrary.Messages;

namespace CommunicationLibraryProj.Adapters
{
    public class TCPClientAdapter: IConnectionClient
    {
        private readonly int messageSizeBitLength = 4;
        private readonly int messageIdBitLength = 4;

        TcpClient client = null;

        public TCPClientAdapter(){}

        public TCPClientAdapter(TcpClient tcpClient)
        {
            client = tcpClient;
        }     

        public event EventHandler ConnectionError;
        public bool Connect(string IP, int port)
        {
            return false;
        }

        public void Disconnect()
        {
        }

        public void SafeDisconnect()
        {          
        }

        public bool SendMessage(Message message)
        {
            return false;
        }
        public Message GetMessage()
        {
            return new Message("action");
        }
        private byte[] ReadBytesFromStream(TcpClient client, int size)
        {
            byte[] content = new byte[size];
            return content;
        }

        public bool IsConnected()
        {
            return client.Connected;
        }

    
    }
}
