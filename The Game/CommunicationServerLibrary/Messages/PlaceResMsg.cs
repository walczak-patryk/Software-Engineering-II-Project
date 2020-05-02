using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class PlaceResMsg : Message
    {
        public string playerGuid;
        public string placementResult;
        public string status;

        public PlaceResMsg(string playerGuid, string placementResult, string status) : base("place")
        {
            this.playerGuid = playerGuid;
            this.placementResult = placementResult;
            this.status = status;
        }
    }
}
