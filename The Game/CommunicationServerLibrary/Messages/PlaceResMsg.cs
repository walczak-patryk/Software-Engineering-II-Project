using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    class PlaceResMsg : Message
    {
        public PlayerGuid playerGuid;
        public string placementResult;
        public string status;

        public PlaceResMsg(PlayerGuid playerGuid, string placementResult, string status) : base("place")
        {
            this.playerGuid = playerGuid;
            this.placementResult = placementResult;
            this.status = status;
        }
    }
}
