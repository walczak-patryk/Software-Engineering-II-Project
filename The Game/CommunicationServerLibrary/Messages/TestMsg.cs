using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class TestMsg : PlayerMsg
    {
        public TestMsg(PlayerGuid playerGuid) : base(playerGuid,"test")
        {
        }
    }
}
