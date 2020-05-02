namespace CommunicationServerLibrary.Interfaces
{
    public interface IConnectionListener
    { 
        void StartListening(string IP, int port);
        void StopListening();     
        IConnectionClient Accept();
    }
}
