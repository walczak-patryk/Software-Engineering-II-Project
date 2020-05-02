using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using CommunicationLibraryProj;
using CommunicationServerLibrary.Messages;
using System.Net;

namespace CommunicationServerProj
{
    public class CommunicationServer
    {
        private IConnectionListener server;
        private int portNumber;
        private IPAddress ipAddress;

        private void Listen() { }

        private ThreadSafeVariable<CSState> state;
        private ThreadSafeVariable<int> gmId;

        private List<Task> tasks;
        private List<ManagedClient> clients;
        private readonly object clientsLocker;

        private readonly object gmRegisteringLocker;
        private int nextId;

        public CommunicationServer(IConnectionListener listener, IPAddress ipAddress, int portNumber)
        {
            this.server = listener ?? throw new ArgumentNullException(nameof(listener));
            this.portNumber = portNumber;
            this.ipAddress = ipAddress;

            this.state = new ThreadSafeVariable<CSState>();
            this.gmId = new ThreadSafeVariable<int> { Value = -1 };

            this.tasks = new List<Task>();
            this.clients = new List<ManagedClient>();
            this.clientsLocker = new object();

            this.gmRegisteringLocker = new object();
            this.nextId = 0;
        }


        public void Run()
        {
            
        }


        private void HandleCommunication(ManagedClient client)
        {
           
        }

        private void HandleGmCommunication(ManagedClient client)
        {
           
        }


        #region MessagesProcessing
        private void ProcessClientMessageDuringListening(Message message, ManagedClient client)
        {
            
        }


        private void ProcessAgentMessageDuringAgentsAccepting(Message message, ManagedClient client)
        {

        }

        private void ProcessAgentMessageDuringGameInProgress(Message message, ManagedClient client)
        {
         
        }

        private void ProcessGmMessageDuringAgentsAccepting(Message message, ManagedClient client)
        {
           
        }

        private void ProcessGmMessageDuringGameInProgress(Message message, ManagedClient client)
        {
            
        }

        private void ProcessGmMessageDuringGameFinished(Message message, ManagedClient client)
        {
            
        }
        #endregion

        private bool TryStartListening()
        {
            try
            {
                server.StartListening(this.ipAddress, this.portNumber);
                state.Value = CSState.Listening;
                return true;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("TryStartListening: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("TryStartListening: "+e.Message);
            }
            return false;
        }

        private bool RegisterGM(ManagedClient client)
        {
            return false;
        }

        private void StartGame()
        {
          
        }

        private void ForwardMessageFromGM(Message msg)
        {
           
        }

        private void HandleAgentDisconnected(ManagedClient client)
        {
            
        }

        private void HandleGMDisconnected()
        {
           
        }

        public void Kill()
        {
            
        }
    }
}