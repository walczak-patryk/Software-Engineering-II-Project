using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ConnectGMResMsg : Message
    {
        public string portNumber;
        public string status;

        public ConnectGMResMsg(string portNumber, string status) : base("connect GM result")
        {
            this.portNumber = portNumber;
            this.status = status;
        }
    }
}
