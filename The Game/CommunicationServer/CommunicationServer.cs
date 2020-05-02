using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using CommunicationServerLibrary.Messages;
using System.Net;
using CommunicationServerLibrary.Interfaces;

namespace CommunicationServer
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

        public CommunicationServer(IConnectionListener listener, string ipAddress, int portNumber)
        {
            this.server = listener ?? throw new ArgumentNullException(nameof(listener));
            this.portNumber = portNumber;
            this.ipAddress = IPAddress.Parse(ipAddress);

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
            if (!TryStartListening())
                return;
            while (true)
            {
                try
                {
                    IConnectionClient client = server.Accept();
                    if (client == null) break;
                    ManagedClient managedClient = new ManagedClient(client, nextId++);
                    lock (clientsLocker)
                    {
                        if (state.Value == CSState.Listening || state.Value == CSState.AgentsAccepting)
                        {
                            clients.Add(managedClient);
                            tasks.Add(Task.Run(() => HandleCommunication(managedClient)));
                        }
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Some error with TCPListener occured.\n");
                    Kill();
                }
            }
            Console.WriteLine($"Communication Server stopped listening for clients.\n");
            Task.WaitAll(tasks.ToArray());
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
                state.Value = CSState.Listening;
                Console.WriteLine($"CS started listening for clients on {ipAddress}:{portNumber}\n");
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Given port is out of range.\n");
            }
            catch (Exception)
            {
                Console.WriteLine("Some error occured while tried to start listening.\n");
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