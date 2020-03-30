using GameMaster.Boards;
using System.Collections.Generic;
using System.Net;

namespace GameMaster
{
    public class GameMaster
    {
        private int portNumber;
        private IPAddress IPAddress;
        private GameMasterBoard board;
        private GameMasterStatus status;
        private GameMasterConfiguration configuration;
        private List<Guid> teamRedGuids;
        private List<Guid> teamBlueGuids;

        public void StartGame()
        {
            this.configuration = new GameMasterConfiguration();
            this.board = new GameMasterBoard(this.configuration.boardGoalHeight, this.configuration.boardGoalHeight, this.configuration.boardTaskHeight);
            this.status = GameMasterStatus.Active;
            teamBlueGuids = new List<Guid> { new Guid(), new Guid(), new Guid() };
            teamRedGuids = new List<Guid> { new Guid(), new Guid(), new Guid() };
        }

        private void listen()
        {

        }

        public GameMasterConfiguration LoadConfigurationFromJSON(string path)
        {
            return null;
        }

        public void SaveConfigurationToJSON(string path)
        {

        }

        private void PutNewPiece()
        {

        }

        private void PrintBoard()
        {

        }

        public void MessageHandler(string message)
        {

        }
    }
}
