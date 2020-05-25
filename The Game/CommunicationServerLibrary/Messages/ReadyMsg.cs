using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class ReadyMsg : PlayerMsg
    {
        public ReadyMsg(PlayerGuid playerGuid) : base(playerGuid,"ready")
        {
        }
    }
}
