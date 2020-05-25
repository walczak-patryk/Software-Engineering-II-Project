using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class TestResMsg : PlayerMsg
    {
        public bool? test;
        public string status;
        
        public TestResMsg(PlayerGuid playerGuid, bool? test, string status) : base(playerGuid,"test status")
        {
            this.test = test;
            this.status = status;
        }
    }
}
