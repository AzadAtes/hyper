using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public class COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL
    {
        public const byte ID = 0x29;
        public const byte VERSION = 1;
        public class SWITCH_TOGGLE_MULTILEVEL_SET
        {
            public const byte ID = 0x01;
            public static implicit operator SWITCH_TOGGLE_MULTILEVEL_SET(byte[] data)
            {
                SWITCH_TOGGLE_MULTILEVEL_SET ret = new SWITCH_TOGGLE_MULTILEVEL_SET();
                return ret;
            }
            public static implicit operator byte[](SWITCH_TOGGLE_MULTILEVEL_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class SWITCH_TOGGLE_MULTILEVEL_GET
        {
            public const byte ID = 0x02;
            public static implicit operator SWITCH_TOGGLE_MULTILEVEL_GET(byte[] data)
            {
                SWITCH_TOGGLE_MULTILEVEL_GET ret = new SWITCH_TOGGLE_MULTILEVEL_GET();
                return ret;
            }
            public static implicit operator byte[](SWITCH_TOGGLE_MULTILEVEL_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class SWITCH_TOGGLE_MULTILEVEL_REPORT
        {
            public const byte ID = 0x03;
            public byte value;
            public static implicit operator SWITCH_TOGGLE_MULTILEVEL_REPORT(byte[] data)
            {
                SWITCH_TOGGLE_MULTILEVEL_REPORT ret = new SWITCH_TOGGLE_MULTILEVEL_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.value = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_TOGGLE_MULTILEVEL_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL.ID);
                ret.Add(ID);
                ret.Add(command.value);
                return ret.ToArray();
            }
        }
        public class SWITCH_TOGGLE_MULTILEVEL_START_LEVEL_CHANGE
        {
            public const byte ID = 0x04;
            public struct Tproperties1
            {
                private byte _value;
                public byte reserved1
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte ignoreStartLevel
                {
                    get { return (byte)(_value >> 5 & 0x01); }
                    set { _value &= 0xFF - 0x20; _value += (byte)(value << 5 & 0x20); }
                }
                public byte reserved2
                {
                    get { return (byte)(_value >> 6 & 0x01); }
                    set { _value &= 0xFF - 0x40; _value += (byte)(value << 6 & 0x40); }
                }
                public byte rollOver
                {
                    get { return (byte)(_value >> 7 & 0x01); }
                    set { _value &= 0xFF - 0x80; _value += (byte)(value << 7 & 0x80); }
                }
                public static implicit operator Tproperties1(byte data)
                {
                    Tproperties1 ret = new Tproperties1();
                    ret._value = data;
                    return ret;
                }
                public static implicit operator byte(Tproperties1 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties1 properties1;
            public byte startLevel;
            public static implicit operator SWITCH_TOGGLE_MULTILEVEL_START_LEVEL_CHANGE(byte[] data)
            {
                SWITCH_TOGGLE_MULTILEVEL_START_LEVEL_CHANGE ret = new SWITCH_TOGGLE_MULTILEVEL_START_LEVEL_CHANGE();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.startLevel = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](SWITCH_TOGGLE_MULTILEVEL_START_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                ret.Add(command.startLevel);
                return ret.ToArray();
            }
        }
        public class SWITCH_TOGGLE_MULTILEVEL_STOP_LEVEL_CHANGE
        {
            public const byte ID = 0x05;
            public static implicit operator SWITCH_TOGGLE_MULTILEVEL_STOP_LEVEL_CHANGE(byte[] data)
            {
                SWITCH_TOGGLE_MULTILEVEL_STOP_LEVEL_CHANGE ret = new SWITCH_TOGGLE_MULTILEVEL_STOP_LEVEL_CHANGE();
                return ret;
            }
            public static implicit operator byte[](SWITCH_TOGGLE_MULTILEVEL_STOP_LEVEL_CHANGE command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_SWITCH_TOGGLE_MULTILEVEL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

