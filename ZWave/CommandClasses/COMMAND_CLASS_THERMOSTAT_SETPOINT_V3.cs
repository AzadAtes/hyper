using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public class COMMAND_CLASS_THERMOSTAT_SETPOINT_V3
    {
        public const byte ID = 0x43;
        public const byte VERSION = 3;
        public class THERMOSTAT_SETPOINT_GET
        {
            public const byte ID = 0x02;
            public struct Tproperties1
            {
                private byte _value;
                public byte setpointType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public static implicit operator THERMOSTAT_SETPOINT_GET(byte[] data)
            {
                THERMOSTAT_SETPOINT_GET ret = new THERMOSTAT_SETPOINT_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public byte setpointType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public struct Tproperties2
            {
                private byte _value;
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2;
            public IList<byte> value = new List<byte>();
            public static implicit operator THERMOSTAT_SETPOINT_REPORT(byte[] data)
            {
                THERMOSTAT_SETPOINT_REPORT ret = new THERMOSTAT_SETPOINT_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.properties2 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.value = new List<byte>();
                    for (int i = 0; i < ret.properties2.size; i++)
                    {
                        ret.value.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                ret.Add(command.properties2);
                if (command.value != null)
                {
                    foreach (var tmp in command.value)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public byte setpointType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public struct Tproperties2
            {
                private byte _value;
                public byte size
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2;
            public IList<byte> value = new List<byte>();
            public static implicit operator THERMOSTAT_SETPOINT_SET(byte[] data)
            {
                THERMOSTAT_SETPOINT_SET ret = new THERMOSTAT_SETPOINT_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.properties2 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.value = new List<byte>();
                    for (int i = 0; i < ret.properties2.size; i++)
                    {
                        ret.value.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                ret.Add(command.properties2);
                if (command.value != null)
                {
                    foreach (var tmp in command.value)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_SUPPORTED_GET
        {
            public const byte ID = 0x04;
            public static implicit operator THERMOSTAT_SETPOINT_SUPPORTED_GET(byte[] data)
            {
                THERMOSTAT_SETPOINT_SUPPORTED_GET ret = new THERMOSTAT_SETPOINT_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_SUPPORTED_REPORT
        {
            public const byte ID = 0x05;
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator THERMOSTAT_SETPOINT_SUPPORTED_REPORT(byte[] data)
            {
                THERMOSTAT_SETPOINT_SUPPORTED_REPORT ret = new THERMOSTAT_SETPOINT_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        ret.bitMask.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                if (command.bitMask != null)
                {
                    foreach (var tmp in command.bitMask)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_CAPABILITIES_GET
        {
            public const byte ID = 0x09;
            public struct Tproperties1
            {
                private byte _value;
                public byte setpointType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public static implicit operator THERMOSTAT_SETPOINT_CAPABILITIES_GET(byte[] data)
            {
                THERMOSTAT_SETPOINT_CAPABILITIES_GET ret = new THERMOSTAT_SETPOINT_CAPABILITIES_GET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_CAPABILITIES_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public class THERMOSTAT_SETPOINT_CAPABILITIES_REPORT
        {
            public const byte ID = 0x0A;
            public struct Tproperties1
            {
                private byte _value;
                public byte setpointType
                {
                    get { return (byte)(_value >> 0 & 0x0F); }
                    set { _value &= 0xFF - 0x0F; _value += (byte)(value << 0 & 0x0F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 4 & 0x0F); }
                    set { _value &= 0xFF - 0xF0; _value += (byte)(value << 4 & 0xF0); }
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
            public struct Tproperties2
            {
                private byte _value;
                public byte size1
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale1
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision1
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties2(byte data)
                {
                    Tproperties2 ret = new Tproperties2();
                    ret._value = data;
                    return ret;
                }
                public static implicit operator byte(Tproperties2 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties2 properties2;
            public IList<byte> minValue = new List<byte>();
            public struct Tproperties3
            {
                private byte _value;
                public byte size2
                {
                    get { return (byte)(_value >> 0 & 0x07); }
                    set { _value &= 0xFF - 0x07; _value += (byte)(value << 0 & 0x07); }
                }
                public byte scale2
                {
                    get { return (byte)(_value >> 3 & 0x03); }
                    set { _value &= 0xFF - 0x18; _value += (byte)(value << 3 & 0x18); }
                }
                public byte precision2
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
                }
                public static implicit operator Tproperties3(byte data)
                {
                    Tproperties3 ret = new Tproperties3();
                    ret._value = data;
                    return ret;
                }
                public static implicit operator byte(Tproperties3 prm)
                {
                    return prm._value;
                }
            }
            public Tproperties3 properties3;
            public IList<byte> maxvalue = new List<byte>();
            public static implicit operator THERMOSTAT_SETPOINT_CAPABILITIES_REPORT(byte[] data)
            {
                THERMOSTAT_SETPOINT_CAPABILITIES_REPORT ret = new THERMOSTAT_SETPOINT_CAPABILITIES_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.properties2 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.minValue = new List<byte>();
                    for (int i = 0; i < ret.properties2.size1; i++)
                    {
                        ret.minValue.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                    ret.properties3 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.maxvalue = new List<byte>();
                    for (int i = 0; i < ret.properties3.size2; i++)
                    {
                        ret.maxvalue.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](THERMOSTAT_SETPOINT_CAPABILITIES_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_THERMOSTAT_SETPOINT_V3.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                ret.Add(command.properties2);
                if (command.minValue != null)
                {
                    foreach (var tmp in command.minValue)
                    {
                        ret.Add(tmp);
                    }
                }
                ret.Add(command.properties3);
                if (command.maxvalue != null)
                {
                    foreach (var tmp in command.maxvalue)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}

