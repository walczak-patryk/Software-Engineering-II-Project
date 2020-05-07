using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class MoveResMsg : Message
    {
        public PlayerGuid playerGuid;
        public GameMaster.Positions.Direction direction;
        public string status;
        public GameMaster.Positions.Position position;
        public MoveResMsg(PlayerGuid playerGuid, GameMaster.Positions.Direction direction, string status, GameMaster.Positions.Position position) : base("move status")
        {
            this.playerGuid = playerGuid;
            this.direction = direction;
            this.status = status;
            this.position = position;
        }
    }
}
