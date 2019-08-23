using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public class COMMAND_CLASS_HRV_CONTROL
    {
        public const byte ID = 0x39;
        public const byte VERSION = 1;
        public class HRV_CONTROL_BYPASS_GET
        {
            public const byte ID = 0x05;
            public static implicit operator HRV_CONTROL_BYPASS_GET(byte[] data)
            {
                HRV_CONTROL_BYPASS_GET ret = new HRV_CONTROL_BYPASS_GET();
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_BYPASS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_BYPASS_REPORT
        {
            public const byte ID = 0x06;
            public byte bypass;
            public static implicit operator HRV_CONTROL_BYPASS_REPORT(byte[] data)
            {
                HRV_CONTROL_BYPASS_REPORT ret = new HRV_CONTROL_BYPASS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.bypass = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_BYPASS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.bypass);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_BYPASS_SET
        {
            public const byte ID = 0x04;
            public byte bypass;
            public static implicit operator HRV_CONTROL_BYPASS_SET(byte[] data)
            {
                HRV_CONTROL_BYPASS_SET ret = new HRV_CONTROL_BYPASS_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.bypass = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_BYPASS_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.bypass);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_MODE_GET
        {
            public const byte ID = 0x02;
            public static implicit operator HRV_CONTROL_MODE_GET(byte[] data)
            {
                HRV_CONTROL_MODE_GET ret = new HRV_CONTROL_MODE_GET();
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_MODE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_MODE_REPORT
        {
            public const byte ID = 0x03;
            public struct Tproperties1
            {
                private byte _value;
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public static implicit operator HRV_CONTROL_MODE_REPORT(byte[] data)
            {
                HRV_CONTROL_MODE_REPORT ret = new HRV_CONTROL_MODE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_MODE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_MODE_SET
        {
            public const byte ID = 0x01;
            public struct Tproperties1
            {
                private byte _value;
                public byte mode
                {
                    get { return (byte)(_value >> 0 & 0x1F); }
                    set { _value &= 0xFF - 0x1F; _value += (byte)(value << 0 & 0x1F); }
                }
                public byte reserved
                {
                    get { return (byte)(_value >> 5 & 0x07); }
                    set { _value &= 0xFF - 0xE0; _value += (byte)(value << 5 & 0xE0); }
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
            public static implicit operator HRV_CONTROL_MODE_SET(byte[] data)
            {
                HRV_CONTROL_MODE_SET ret = new HRV_CONTROL_MODE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_MODE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_MODE_SUPPORTED_GET
        {
            public const byte ID = 0x0A;
            public static implicit operator HRV_CONTROL_MODE_SUPPORTED_GET(byte[] data)
            {
                HRV_CONTROL_MODE_SUPPORTED_GET ret = new HRV_CONTROL_MODE_SUPPORTED_GET();
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_MODE_SUPPORTED_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_MODE_SUPPORTED_REPORT
        {
            public const byte ID = 0x0B;
            public struct Tproperties1
            {
                private byte _value;
                public byte manualControlSupported
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
            public IList<byte> bitMask = new List<byte>();
            public static implicit operator HRV_CONTROL_MODE_SUPPORTED_REPORT(byte[] data)
            {
                HRV_CONTROL_MODE_SUPPORTED_REPORT ret = new HRV_CONTROL_MODE_SUPPORTED_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.properties1 = data.Length > index ? data[index++] : (byte)0x00;
                    ret.bitMask = new List<byte>();
                    while (data.Length - 0 > index)
                    {
                        ret.bitMask.Add(data.Length > index ? data[index++] : (byte)0x00);
                    }
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_MODE_SUPPORTED_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.properties1);
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
        public class HRV_CONTROL_VENTILATION_RATE_GET
        {
            public const byte ID = 0x08;
            public static implicit operator HRV_CONTROL_VENTILATION_RATE_GET(byte[] data)
            {
                HRV_CONTROL_VENTILATION_RATE_GET ret = new HRV_CONTROL_VENTILATION_RATE_GET();
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_VENTILATION_RATE_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_VENTILATION_RATE_REPORT
        {
            public const byte ID = 0x09;
            public byte ventilationRate;
            public static implicit operator HRV_CONTROL_VENTILATION_RATE_REPORT(byte[] data)
            {
                HRV_CONTROL_VENTILATION_RATE_REPORT ret = new HRV_CONTROL_VENTILATION_RATE_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.ventilationRate = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_VENTILATION_RATE_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.ventilationRate);
                return ret.ToArray();
            }
        }
        public class HRV_CONTROL_VENTILATION_RATE_SET
        {
            public const byte ID = 0x07;
            public byte ventilationRate;
            public static implicit operator HRV_CONTROL_VENTILATION_RATE_SET(byte[] data)
            {
                HRV_CONTROL_VENTILATION_RATE_SET ret = new HRV_CONTROL_VENTILATION_RATE_SET();
                if (data != null)
                {
                    int index = 2;
                    ret.ventilationRate = data.Length > index ? data[index++] : (byte)0x00;
                }
                return ret;
            }
            public static implicit operator byte[](HRV_CONTROL_VENTILATION_RATE_SET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_HRV_CONTROL.ID);
                ret.Add(ID);
                ret.Add(command.ventilationRate);
                return ret.ToArray();
            }
        }
    }
}

