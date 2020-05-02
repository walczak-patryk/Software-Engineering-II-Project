using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class PickUpResMsg : Message
    {
        public PlayerGuid playerGuid;
        public string status;
        public PickUpResMsg(PlayerGuid playerGuid, string status) : base("pickup")
        {
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
