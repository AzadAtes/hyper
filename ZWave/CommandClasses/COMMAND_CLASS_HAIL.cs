using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public class COMMAND_CLASS_HAIL
    {
        public const byte ID = 0x82;
        public const byte VERSION = 1;
        public class HAIL
        {
            public const byte ID = 0x01;
            public static implicit operator HAIL(byte[] data)
            {
                HAIL ret = new HAIL();
                return ret;
            }
            public static implicit operator byte[](HAIL command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HAIL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

