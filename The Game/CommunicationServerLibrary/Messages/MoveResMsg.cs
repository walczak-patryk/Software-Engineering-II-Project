using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class MoveResMsg : Message
    {
        public string playerGuid;
        public GameMaster.Positions.Direction direction;
        public string status;
        public GameMaster.Positions.Position position;
        public MoveResMsg(string playerGuid, GameMaster.Positions.Direction direction, string status, GameMaster.Positions.Position position) : base("move")
        {
            this.playerGuid = playerGuid;
            this.direction = direction;
            this.status = status;
            this.position = position;
        }
    }
}
