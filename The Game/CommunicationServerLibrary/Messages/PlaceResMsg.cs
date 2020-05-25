using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PlaceResMsg : PlayerMsg
    {
        public string placementResult;
        public string status;

        public PlaceResMsg(PlayerGuid playerGuid, string placementResult, string status) : base(playerGuid,"place status")
        {
            this.placementResult = placementResult;
            this.status = status;
        }
    }
}
