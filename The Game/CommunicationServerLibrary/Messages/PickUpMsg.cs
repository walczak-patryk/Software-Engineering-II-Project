using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PickUpMsg : PlayerMsg
    {
        public PickUpMsg(PlayerGuid playerGuid) : base(playerGuid, "pickup")
        {
            this.playerGuid = playerGuid;
        }
    }
}
