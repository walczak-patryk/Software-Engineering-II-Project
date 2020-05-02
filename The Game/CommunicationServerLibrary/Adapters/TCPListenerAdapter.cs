using System.Net.Sockets;
using System.Net;
using System;

namespace CommunicationLibraryProj.Adapters
{
    public class TCPListenerAdapter : IConnectionListener
    {
        TcpListener listener;
        public IConnectionClient Accept()
        {
            TcpClient acceptedClient = null;

            try
            {
                acceptedClient = listener.AcceptTcpClient();
            }
            catch (InvalidOperationException e)
            {

                Console.WriteLine("Listener Accept: " + e.Message);

                return null;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.Interrupted)
                    return null;
                else
                {
                    Console.WriteLine("Listener Accept: " + e.Message);
                    throw;
                }
            }

            return new TCPClientAdapter(acceptedClient);
        }
 
        public void StartListening(IPAddress IP, int port)
        {
            try
            {
                listener = new TcpListener(IP, port);
                listener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Listener StartListening: "+e.Message);
                throw;
            }
        }
    
        public void StopListening()
        {
            try
            {
                listener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Listener StopListening: "+e.Message);
            }

        }
    }
}
