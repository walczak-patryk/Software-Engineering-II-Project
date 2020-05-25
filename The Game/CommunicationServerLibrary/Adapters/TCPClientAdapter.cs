using System;
using System.Net.Sockets;
using CommunicationServerLibrary.Interfaces;
using CommunicationServerLibrary.Messages;
using System.Net;
using System.Text;

namespace CommunicationServerLibrary.Adapters

{
    public class TCPClientAdapter: IConnectionClient
    {
        private readonly int messageSizeBitLength = 4;

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
            try
            {
                client.Client.Shutdown(SocketShutdown.Send);
                // Error-prone part 
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool SendMessage(Message message)
        {
            try
            {
                string serializedMessage = MessageSerializer.Serialize(message);

                byte[] jsonbytes = Encoding.UTF8.GetBytes(serializedMessage);
                int size = IPAddress.HostToNetworkOrder(jsonbytes.Length);
                byte[] sizeByte = BitConverter.GetBytes(size);

                byte[] bytes = new byte[jsonbytes.Length + messageSizeBitLength];

                sizeByte.CopyTo(bytes, 0);
                jsonbytes.CopyTo(bytes, messageSizeBitLength);

                client.Client.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public Message GetMessage()
        {
            Message msg = null;
            while (msg == null)
            {
                msg = Listen();
            }
            return msg;
        }
        public Message Listen()
        {
            byte[] sizeBytes = null;
            byte[] idBytes = null;
            byte[] contentBytes = null;

            try
            {
                sizeBytes = ReadBytesFromStream(client, messageSizeBitLength);
                if (sizeBytes == null)
                    return null; // To handle

                int size = IPAddress.NetworkToHostOrder(
                    BitConverter.ToInt32(sizeBytes, 0));

                contentBytes = ReadBytesFromStream(client, size);
                if (contentBytes == null)
                    return null; // To handle

                string content = Encoding.UTF8.GetString(contentBytes);

                return MessageSerializer.Deserialize(content);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception \n"+e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
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
