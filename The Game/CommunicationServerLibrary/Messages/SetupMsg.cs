using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    public class SetupMsg : Message
    {
        public SetupMsg() : base("setup") 
        {

        }
    }
}
