using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    public class SetupResMsg : Message
    {
        public string status;
        public SetupResMsg(string status) : base("setup")
        {
            this.status = status;
        }
    }
}
