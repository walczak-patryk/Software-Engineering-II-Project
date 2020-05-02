using System.Net;
﻿namespace CommunicationServerLibrary.Interfaces
{
    public interface IConnectionListener
    { 
        void StartListening(IPAddress IP, int port);
        void StopListening();     
        IConnectionClient Accept();
    }
}
