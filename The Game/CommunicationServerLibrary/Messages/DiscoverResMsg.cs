using GameMaster.Fields;
using GameMaster.Positions;
using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class DiscoverResMsg : Message
    {
        public PlayerGuid playerGuid;
        public Position position;
        public List<Field> fields;
        public string status;
        public DiscoverResMsg(PlayerGuid playerGuid, Position position, List<Field> fields, string status) : base("discover")
        {
            this.playerGuid = playerGuid;
            this.position = position;
            this.fields = fields;
            this.status = status;
        }
    }
}
