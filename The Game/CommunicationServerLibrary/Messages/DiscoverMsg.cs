using System;
using System.Collections.Generic;
using System.Text;
using GameMaster.Positions;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class DiscoverMsg : Message
    {
        public PlayerGuid playerGuid;
        public Position position;
        public DiscoverMsg(PlayerGuid playerGuid, Position position) : base("discover")
        {
            this.playerGuid = playerGuid;
            this.position = position;
        }
    }
}
