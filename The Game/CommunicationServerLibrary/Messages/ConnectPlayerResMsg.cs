using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ConnectPlayerResMsg : PlayerMsg
    {
        public string portNumber;
        public string status;
        public ConnectPlayerResMsg(string portNumber, PlayerGuid playerGuid, string status) : base(playerGuid,"connect status")
        {
            this.portNumber = portNumber;
            this.status = status;
        }
    }
}
