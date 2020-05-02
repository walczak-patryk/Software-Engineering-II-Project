using System;
using System.Collections.Generic;
using System.Text;
using GameMaster.Positions;

namespace CommunicationServerLibrary.Messages
{
    class DiscoverMsg : Message
    {
        public string playerGuid;
        public Position position;
        public DiscoverMsg(string playerGuid, Position position) : base("discover")
        {
            this.playerGuid = playerGuid;
            this.position = position;
        }
    }
}
