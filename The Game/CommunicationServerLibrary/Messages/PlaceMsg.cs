using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class PlaceMsg : Message
    {
        public string playerGuid;
        public PlaceMsg(string playerGuid) : base("place")
        {
            this.playerGuid = playerGuid;
        }
    }
}
