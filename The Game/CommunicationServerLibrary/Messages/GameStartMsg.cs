using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerLibrary.Messages
{
    class GameStartMsg : Message
    {
        public string playerGuid;
        public GameMaster.TeamColor team;
        public GameMaster.TeamRole teamRole;
        public int teamSize;
        public string[] teamGuids;
        public GameMaster.Positions.Position position;
        public GameMaster.Boards.Board board;
        public GameStartMsg(string playerGuid, GameMaster.TeamColor team, GameMaster.TeamRole teamRole, int teamSize, 
                            string[] teamGuids, GameMaster.Positions.Position position, GameMaster.Boards.Board board) : base("start")
        {
            this.playerGuid = playerGuid;
            this.team = team;
            this.teamRole = teamRole;
            this.teamSize = teamSize;
            this.teamGuids = teamGuids;
            this.position = position;
            this.board = board;
        }
    }
}
