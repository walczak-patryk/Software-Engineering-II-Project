using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class SetupResMsg : Message
    {
        public string status;
        public SetupResMsg(string status) : base("setup")
        {
            this.status = status;
        }
    }
}
