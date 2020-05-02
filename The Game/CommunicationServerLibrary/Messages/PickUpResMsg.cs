using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class PickUpResMsg : Message
    {
        public string playerGuid;
        public string status;
        public PickUpResMsg(string playerGuid, string status) : base("pickup")
        {
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
