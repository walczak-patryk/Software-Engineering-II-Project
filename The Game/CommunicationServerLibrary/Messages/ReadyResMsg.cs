using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class ReadyResMsg : Message
    {
        public PlayerGuid playerGuid;
        public string status;
        public ReadyResMsg(PlayerGuid playerGuid, string status) : base("ready")
        {
            this.playerGuid = playerGuid;
            this.status = status;
        }
    }
}
