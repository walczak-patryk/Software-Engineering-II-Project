using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PlayerMsg : Message
    {
        public PlayerGuid playerGuid;

        public PlayerMsg(PlayerGuid playerGuid, string action) : base(action)
        {
            this.playerGuid = playerGuid;
        }
    }
}
