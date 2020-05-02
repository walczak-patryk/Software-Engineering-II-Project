using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class ReadyResMsg : Message
    {
        public string playerGuid;
        public string status;
        public ReadyResMsg(string playerGuid, string status) : base("ready")
        {
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
