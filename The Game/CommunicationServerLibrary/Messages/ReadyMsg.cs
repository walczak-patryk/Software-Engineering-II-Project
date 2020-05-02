using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class ReadyMsg : Message
    {
        public string playerGuid;
        public ReadyMsg(string playerGuid) : base("ready")
        {
            this.playerGuid = playerGuid;
        }
    }
}
