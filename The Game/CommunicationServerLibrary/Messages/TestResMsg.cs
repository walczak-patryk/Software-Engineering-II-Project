using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class TestResMsg : Message
    {
        public string playerGuid;
        public string test;
        public string status;
        
        public TestResMsg(string playerGuid, string test, string status) : base("test")
        {
            this.playerGuid = playerGuid;
            this.test = test;
            this.status = status;
        }
    }
}
