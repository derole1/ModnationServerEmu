using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombServerEmu_MNR.Src.Protocols.Clients
{
    // These codes are shared between TCP and RDUP, so some are used in TCP and not in RUDP and vice versa
    // 
    // RUDP seems to be a custom implementation. Its intresting cause it seems to be a hybrid of regular UDP and RUDP.
    // The sender can choose if they want an acknowledement back or not, I imagine this is done for performance reasons.
    // As an example, you dont want to wait for an acknowledgement when broadcasting player positions, it wastes time.
    // 
    // A typical connection init is as follows:
    // Client->Server: SYN
    // Server->Client: ACK-SYN
    // Server->Client: SYN
    // Client->Server: ACK-SYN
    // 
    // The rest of the protocol follows this similarly. For example, to send NetcodeData:
    // Client->Server: NetcodeData(SEQ 0)
    // Client->Server: NetcodeData(SEQ 1)
    // Server->Client: ACK-NetcodeData(SEQ 0)
    // Server->Client: ACK-NetcodeData(SEQ 1)
    // 
    // As sequence numbers are involved, I assume the packets need to be re-ordered once theyre all recieved,
    // ACKs are only sent out once all packets have been recieved. I need to figure out how to tell how many to expect.
    // 
    // You get the idea, its similar to TCP but with the flexibility and performance of UDP when its required
    // 
    // PS: Assuming 0x67 is FIN, a completely wild guess, but I dont know what else it could be

    public enum EBombPacketType : byte
    {
        Acknowledge = 0x63,
        KeepAlive = 0x61,
        NetcodeData = 0x64,
        ReliableGameData = 0x66,
        UnreliableGameData = 0x65,
        Finish = 0x67, //This never seems to be used
        Reset = 0x60,
        Sync = 0x62
    }
}
