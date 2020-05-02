using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerProj
{
    internal enum CSState
    {
        Listening,
        AgentsAccepting,
        GameInProgress,
        GameFinished 
    }
}
