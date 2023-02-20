using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FFA.Empty.Empty.ServerAndNetwork
{


    public struct ScafholdEntity
    {
        public byte scafholdEntityID;
        public byte scafholdClientID;
        public string name;
    }
    public struct PacketPlusTiming
    {
        public short packet;

        public float timing;
    }
    public struct VariableSyncPacket
    {
        public byte xCoord;
        public byte yCoord;

        public short hp;
        public byte item;

        public byte blunder;
    }
}