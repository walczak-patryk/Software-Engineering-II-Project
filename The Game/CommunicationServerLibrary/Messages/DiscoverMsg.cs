using System;
using System.Collections.Generic;
using System.Text;
using GameMaster.Positions;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class DiscoverMsg : PlayerMsg
    {
        public Position position;
        public DiscoverMsg(PlayerGuid playerGuid, Position position) : base(playerGuid,"discover")
        {
            this.position = position;
        }
    }
}
