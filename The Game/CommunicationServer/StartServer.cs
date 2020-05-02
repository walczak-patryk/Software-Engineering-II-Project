using CommunicationServerLibrary.Adapters;

namespace CommunicationServer
{
    class StartServer
    {
        static void Main(string[] args)
        {
            new CommunicationServer(new TCPListenerAdapter(), "127.0.0.1", 13000).Run();
        }
    }
}
