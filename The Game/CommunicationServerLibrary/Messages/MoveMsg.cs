using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class MoveMsg : Message
    {
        public PlayerGuid playerGuid;
        public GameMaster.Positions.Direction direction;
        public MoveMsg(PlayerGuid playerGuid, GameMaster.Positions.Direction direction) : base("move")
        {
            this.playerGuid = playerGuid;
            this.direction = direction;
        }
    }
}
