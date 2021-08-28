using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GorgaTech {
    interface PacketHandler {

        void OnClientPacket(ref Packet packet);
        void OnServerPacket(ref Packet packet);

    }
}
