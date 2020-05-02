using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class ConnectPlayerMsg : Message
    {
        public string portNumber;
        public PlayerGuid playerGuid;
        public ConnectPlayerMsg(string portNumber, PlayerGuid playerGuid) : base("connect")
        {
            this.portNumber = portNumber;
            this.playerGuid = playerGuid;
        }
    }
}
