using System;
using System.Net.Sockets;
using CommunicationServerLibrary.Messages;
using System.Net;

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
        public bool Connect(IPAddress IP, int port)
        {
            try
            {
                client = new TcpClient();
                IPEndPoint ipEndPoint = new IPEndPoint(IP, port);
                client.Connect(ipEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(" Client Connect: "+e.Message);
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            try
            {
                client.Client.Shutdown(SocketShutdown.Send);
                if (client.Client.Available != 0)
                    client.Client.Receive(new byte[client.Client.Available]);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Client Disconnect: " + e.Message); ;
            }
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
            int remainingSize = size;
            try
            {
                while (remainingSize > 0)
                {
                    int offset = (size - remainingSize);
                    int receivedBytes = client.Client.Receive(content, offset, size - offset, SocketFlags.None);
                    if (receivedBytes == 0)
                        return null;
                    remainingSize -= receivedBytes;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client ReadBytesFromStream: "+e.Message);
            }
            return content;
        }

        public bool IsConnected()
        {
            return client.Connected;
        }

    
    }
}
