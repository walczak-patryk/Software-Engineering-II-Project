using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ConnectPlayerResMsg : Message
    {
        public string portNumber;
        public PlayerGuid playerGuid;
        public string status;
        public ConnectPlayerResMsg(string portNumber, PlayerGuid playerGuid, string status) : base("connect")
        {
            this.portNumber = portNumber;
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
