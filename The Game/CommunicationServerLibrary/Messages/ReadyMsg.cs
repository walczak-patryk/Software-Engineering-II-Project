using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ReadyMsg : Message
    {
        public PlayerGuid playerGuid;
        public ReadyMsg(PlayerGuid playerGuid) : base("ready")
        {
            this.playerGuid = playerGuid;
        }
    }
}
