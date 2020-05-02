using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class PickUpMsg : Message
    {
        public string playerGuid;
        public PickUpMsg(string playerGuid, string status) : base("pickup")
        {
            this.playerGuid = playerGuid;
        }
    }
}
