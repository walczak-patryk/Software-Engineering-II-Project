using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PlaceMsg : Message
    {
        public PlayerGuid playerGuid;
        public PlaceMsg(PlayerGuid playerGuid) : base("place")
        {
            this.playerGuid = playerGuid;
        }
    }
}
