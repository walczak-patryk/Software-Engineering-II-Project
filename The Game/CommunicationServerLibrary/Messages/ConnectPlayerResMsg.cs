using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class ConnectPlayerResMsg : Message
    {
        public string portNumber;
        public string playerGuid;
        public string status;
        public ConnectPlayerResMsg(string portNumber, string playerGuid, string status) : base("connect")
        {
            this.portNumber = portNumber;
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
