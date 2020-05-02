using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class TestMsg : Message
    {
        public string playerGuid;
        public TestMsg(string playerGuid, string status) : base("test")
        {
            this.playerGuid = playerGuid;
        }
    }
}
