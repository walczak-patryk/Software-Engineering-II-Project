using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class TestMsg : Message
    {
        public PlayerGuid playerGuid;
        public TestMsg(PlayerGuid playerGuid) : base("test")
        {
            this.playerGuid = playerGuid;
        }
    }
}
