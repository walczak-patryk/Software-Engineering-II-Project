using GameMaster;

namespace CommunicationServerLibrary.Messages
{
    public class GameEndMsg : Message
    {
        public TeamColor result;
        public GameEndMsg(TeamColor result) : base("end")
        {
            this.result = result;
        }
    }
}
