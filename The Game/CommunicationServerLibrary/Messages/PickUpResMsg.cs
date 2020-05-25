using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PickUpResMsg : PlayerMsg
    {
        public string status;
        public PickUpResMsg(PlayerGuid playerGuid, string status) : base(playerGuid, "pickup status")
        {
            this.status = status;
        }
    }
}
