using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ReadyResMsg : PlayerMsg
    {
        public string status;
        public ReadyResMsg(PlayerGuid playerGuid, string status) : base(playerGuid,"ready status")
        {
            this.status = status;
        }
    }
}
