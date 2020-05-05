using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PickUpMsg : Message
    {
        public PlayerGuid playerGuid;
        public PickUpMsg(PlayerGuid playerGuid) : base("pickup")
        {
            this.playerGuid = playerGuid;
        }
    }
}
