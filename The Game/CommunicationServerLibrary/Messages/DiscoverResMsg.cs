using GameMaster.Fields;
using GameMaster.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class DiscoverResMsg : Message
    {
        public string playerGuid;
        public Position position;
        public Field[] fields;
        public string status;
        public DiscoverResMsg(string playerGuid, Position position, Field[] fields, string status) : base("discover")
        {
            this.playerGuid = playerGuid;
            this.position = position;
            this.fields = fields;
            this.status = status;
        }
    }
}
