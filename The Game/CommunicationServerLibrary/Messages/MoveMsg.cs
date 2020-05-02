using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class MoveMsg : Message
    {
        public string playerGuid;
        public GameMaster.Positions.Direction direction;
        public MoveMsg(string playerGuid, GameMaster.Positions.Direction direction) : base("move")
        {
            this.playerGuid = playerGuid;
            this.direction = direction;
        }
    }
}
