using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class ConnectGMMsg : Message
    {
        public string portNumber;

        public ConnectGMMsg(string portNumber) : base("connect GM")
        {
            this.portNumber = portNumber;
        }
    }
}
