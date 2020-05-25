using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class PlaceMsg : PlayerMsg
    {
        public PlaceMsg(PlayerGuid playerGuid) : base(playerGuid, "place")
        {
        }
    }
}
