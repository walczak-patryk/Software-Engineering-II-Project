using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class MoveMsg : PlayerMsg
    {
        public GameMaster.Positions.Direction direction;
        public MoveMsg(PlayerGuid playerGuid, GameMaster.Positions.Direction direction) : base(playerGuid, "move")
        {
            this.direction = direction;
        }
    }
}
