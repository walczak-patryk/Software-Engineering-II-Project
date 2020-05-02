using CommunicationServerLibrary.Interfaces;
using System.Net.Sockets;

namespace CommunicationServerLibrary.Adapters
{
    public class TCPListenerAdapter : IConnectionListener
    {
        TcpListener listener;
        public IConnectionClient Accept()
        {
            return null;
        }
 
        public void StartListening(string IP, int port)
        {
        }
    
        public void StopListening()
        {

        }
    }
}
