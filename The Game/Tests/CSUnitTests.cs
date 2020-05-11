using CommunicationLibraryProj.Mocks;
using CommunicationServer;
using CommunicationServerLibrary.Messages;
using GameMaster;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Tests
{
    class CSUnitTests
    {
        private int portNumber = 13000;
        private string ipAddress = "127.0.0.1";

        private MockConnectionListener mockListener;
        private List<MockConnectionClient> mockClients;
        private CommunicationServer.CommunicationServer communicationServer;
        private Task csTask;
        private List<PlayerGuid> playersGuids = new List<PlayerGuid>();

        private void InitMocksAndStartCommunicationServer(int clientsCount)
        {
            mockClients = new List<MockConnectionClient>();
            for (int i = 0; i < clientsCount; i++)
                mockClients.Add(new MockConnectionClient());

            mockListener = new MockConnectionListener();
            mockClients.ForEach(c => mockListener.AddClient(c.InjectableClient));

            communicationServer = new CommunicationServer.CommunicationServer(mockListener, ipAddress, portNumber);
            csTask = Task.Run(() => communicationServer.Run());
        }

        private void DisposeMocksAndStopCommunicationServer()
        {
            communicationServer.Kill();
            csTask.Wait();

            mockListener.Dispose();
            mockClients.ForEach(e => e.Dispose());
        }

        private void ConnectTestPlayers()
        {
            playersGuids.Clear();
            for (int i = 1; i < mockClients.Count; i++)
            {
                mockClients[i].SendMessageFromClient(new ConnectPlayerMsg(portNumber.ToString(), new PlayerGuid()));

                var request = mockClients[0].TakeMessageSentToClient();

                PlayerGuid playerGuid = (request as ConnectPlayerMsg).playerGuid;
                playersGuids.Add(playerGuid);

                mockClients[0].SendMessageFromClient(new ConnectPlayerResMsg(portNumber.ToString(), playerGuid, GameMasterStatus.Active.ToString()));

                mockClients[i].TakeMessageSentToClient();
            }
        }

        private void StopClientsAndCommunicationServer()
        {
            communicationServer.Kill();
            csTask.Wait();

            mockListener.Dispose();
            mockClients.ForEach(e => e.Dispose());
        }

      [Test]
      public void OnGmConnectionToServerSendValidGmConnectResponse()
        {
            InitMocksAndStartCommunicationServer(1);

            var result = mockClients[0].SendMessageAndGetResponse(new ConnectGMMsg(portNumber.ToString()));
            Assert.IsInstanceOf<ConnectGMResMsg>(result);
            Assert.IsTrue((result as ConnectGMResMsg).status == "connected");

            StopClientsAndCommunicationServer();
        }

        [Test]
        public void CommunicationServerAcceptsOnlyOneGm()
        {
            InitMocksAndStartCommunicationServer(2);

            var result1 = mockClients[0].SendMessageAndGetResponse(new ConnectGMMsg(portNumber.ToString()));
            var result2 = mockClients[1].SendMessageAndGetResponse(new ConnectGMMsg(portNumber.ToString()));

            Assert.IsInstanceOf<ConnectGMResMsg>(result2);
            Assert.IsTrue((result2 as ConnectGMResMsg).status == "not connected");

            StopClientsAndCommunicationServer();
        }

        [Test]
        public void PlayerCanNotConnectWithoutGm()
        {
            InitMocksAndStartCommunicationServer(1);

            var result = mockClients[0].SendMessageAndGetResponse(new ConnectPlayerMsg(portNumber.ToString(), new PlayerGuid()));

            Assert.IsInstanceOf<ConnectPlayerResMsg>(result);
            Assert.IsTrue((result as ConnectPlayerResMsg).status == "connected");

            StopClientsAndCommunicationServer();
        }
    }
}
