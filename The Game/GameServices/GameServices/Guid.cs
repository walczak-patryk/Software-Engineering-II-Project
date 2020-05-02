using System;

namespace GameMaster
{    public class PlayerGuid
    {
        public Guid g;

        public PlayerGuid()
        {
            g = Guid.NewGuid();
        }
    }
}
