using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class MoveResMsg : PlayerMsg
    {
        public GameMaster.Positions.Direction direction;
        public string status;
        public GameMaster.Positions.Position position;
        public MoveResMsg(PlayerGuid playerGuid, GameMaster.Positions.Direction direction, string status, GameMaster.Positions.Position position) : base(playerGuid,"move status")
        {
            this.direction = direction;
            this.status = status;
            this.position = position;
        }
    }
}
