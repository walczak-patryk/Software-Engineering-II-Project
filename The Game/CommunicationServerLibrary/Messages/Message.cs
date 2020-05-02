using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    public class Message
    {
        public string action;

        public Message(string action)
        {
            this.action = action;
        }
    }
}
