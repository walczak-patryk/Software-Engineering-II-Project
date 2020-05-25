using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class GameStartMsg : PlayerMsg
    {
        public TeamColor team;
        public TeamRole teamRole;
        public int teamSize;
        public string[] teamGuids;
        public GameMaster.Positions.Position position;
        public GameMaster.Boards.Board board;
        public GameStartMsg(PlayerGuid playerGuid, TeamColor team, TeamRole teamRole, int teamSize, 
                            string[] teamGuids, GameMaster.Positions.Position position, GameMaster.Boards.Board board) : base(playerGuid,"start")
        {
            this.team = team;
            this.teamRole = teamRole;
            this.teamSize = teamSize;
            this.teamGuids = teamGuids;
            this.position = position;
            this.board = board;
        }
    }
}
