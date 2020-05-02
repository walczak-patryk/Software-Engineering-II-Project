using System.Net;
namespace CommunicationLibraryProj
{
    public interface IConnectionListener
    { 
        void StartListening(IPAddress IP, int port);
        void StopListening();     
        IConnectionClient Accept();
    }
}
