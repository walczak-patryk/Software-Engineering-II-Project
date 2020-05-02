using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class TestResMsg : Message
    {
        public PlayerGuid playerGuid;
        public string test;
        public string status;
        
        public TestResMsg(PlayerGuid playerGuid, string test, string status) : base("test")
        {
            this.playerGuid = playerGuid;
            this.test = test;
            this.status = status;
        }
    }
}
