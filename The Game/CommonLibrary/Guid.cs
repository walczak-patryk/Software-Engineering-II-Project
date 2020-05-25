using System;
using GameMaster.Positions;

namespace GameMaster
{    public class PlayerGuid
    {
        public Guid g;

        public PlayerGuid()
        {
            g = Guid.NewGuid();
        }
    }

    public class PlayerDTO
    {
        public Guid playerGuid;
        public Position playerPosition;
        public TeamRole playerTeamRole;
        public TeamColor playerTeamColor;
        public ActionType playerAction;
    }

    public enum ActionType
    {
        Move,
        Pickup,
        Test,
        Place,
        Discover,
        Destroy,
        Send
    }
}
