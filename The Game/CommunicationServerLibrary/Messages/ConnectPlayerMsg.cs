using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ConnectPlayerMsg : PlayerMsg
    {
        public string portNumber;
        public ConnectPlayerMsg(string portNumber, PlayerGuid playerGuid) : base(playerGuid,"connect")
        {
            this.portNumber = portNumber;
        }
    }
}
