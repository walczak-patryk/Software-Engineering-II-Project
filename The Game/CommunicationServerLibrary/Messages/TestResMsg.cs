using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class TestResMsg : Message
    {
        public PlayerGuid playerGuid;
        public bool? test;
        public string status;
        
        public TestResMsg(PlayerGuid playerGuid, bool? test, string status) : base("test status")
        {
            this.playerGuid = playerGuid;
            this.test = test;
            this.status = status;
        }
    }
}
