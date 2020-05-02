using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class ConnectPlayerMsg : Message
    {
        public string portNumber;
        public string playerGuid;
        public ConnectPlayerMsg(string portNumber, string playerGuid) : base("connect")
        {
            this.portNumber = portNumber;
            this.playerGuid = playerGuid;
        }
    }
}
