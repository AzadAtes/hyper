﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace ZWave.Xml.FrameHeader
{
    [Serializable]
    public class HeaderValue
    {
        public const string MsgLength = "MSG_LENGTH";
        public const string MsgMarker = "MSG_MARKER";
        protected HeaderValue()
        {
        }
        public bool HasParseError { get; set; }
        public Header HeaderDefinition { get; set; }
        public byte[] Data { get; set; }
        public IList<ParamValue> ParamValues { get; set; }

        public static HeaderValue CreateHeaderValue(byte[] data, Header header)
        {
            HeaderValue ret = new HeaderValue
            {
                HeaderDefinition = header,
                Data = data,
                ParamValues = new List<ParamValue>()
            };
            return ret;
        }

        private readonly object _lockObj = new object();

        public void ProcessParameters()
        {
            lock (_lockObj)
            {
                ParamValues.Clear();
                if (HeaderDefinition.BaseHeaderDefinition != null)
                {
                    ProcessParameters(HeaderDefinition.BaseHeaderDefinition, Data);
                }
                ProcessParameters(HeaderDefinition, Data);
            }
        }
        public byte IndexInData;

        internal void ProcessParameters(BaseHeader header, byte[] data)
        {

            if (header.Param == null || header.Param.Count == 0)
                return;
            foreach (var item in header.Param)
            {
                ParamValues.Add(ProcessParameter(data, ref IndexInData, item, null));
            }
        }

        internal void ProcessParameters(Header header, byte[] data)
        {

            if (header.Param == null || header.Param.Count == 0)
                return;
            foreach (var item in header.Param)
            {
                ParamValues.Add(ProcessParameter(data, ref IndexInData, item, null));
            }
        }

        private ParamValue ProcessParameter(byte[] data, ref byte index, Param item, IList<ParamValue> vgParams)
        {
            ParamValue ret = ParamValue.CreateParamValue(item);
            if (string.IsNullOrEmpty(item.OptRef) || IsIncluded(item.OptRef))
            {
                if (item.Param1 != null && item.Param1.Count > 0 && (item.Size > 1 || item.SizeRef != null))
                {
                    ret.InnerValues = new List<ParamValue>();
                    if (item.Size > 1)
                    {
                        for (int i = 0; i < item.Size; i++)
                        {
                            ParamValue vg = ParamValue.CreateParamValue(item);
                            vg.InnerValues = new List<ParamValue>();
                            foreach (var prm1 in item.Param1)
                            {
                                vg.InnerValues.Add(ProcessParameter(data, ref index, prm1, vg.InnerValues));
                            }
                            ret.InnerValues.Add(vg);
                        }
                    }
                    else if (item.SizeRef != null && item.SizeRef == MsgLength)
                    {
                        while (data.Length - GetTailSize(HeaderDefinition) > index)
                        {
                            ParamValue vg = ParamValue.CreateParamValue(item);
                            vg.InnerValues = new List<ParamValue>();
                            foreach (var prm1 in item.Param1)
                            {
                                vg.InnerValues.Add(ProcessParameter(data, ref index, prm1, vg.InnerValues));
                            }
                            ret.InnerValues.Add(vg);
                        }
                    }
                    else if (item.SizeRef != null)
                    {
                        for (int i = 0; i < GetSize(item.SizeRef, vgParams); i++)
                        {
                            ParamValue vg = ParamValue.CreateParamValue(item);
                            vg.InnerValues = new List<ParamValue>();
                            foreach (var prm1 in item.Param1)
                            {
                                vg.InnerValues.Add(ProcessParameter(data, ref index, prm1, vg.InnerValues));
                            }
                            ret.InnerValues.Add(vg);
                        }
                    }
                }
                else if (item.Param1 != null && item.Param1.Count > 0 && item.Bits == 8)
                {
                    ret.InnerValues = new List<ParamValue>();
                    byte bvalue = data.Length > index ? data[index++] : (byte)0;
                    byte totalbits = 0;
                    foreach (var prm1 in item.Param1)
                    {
                        ParamValue sp = ParamValue.CreateParamValue(prm1);
                        sp.ByteValueList.Add((byte)(bvalue >> totalbits & Tools.GetMaskFromBits(prm1.Bits, 0)));
                        ret.InnerValues.Add(sp);
                        totalbits += prm1.Bits;
                    }
                }
                else
                {
                    byte bytesCount = GetParamValueBytes(item);
                    if (item.Size > 1)
                    {
                        for (int i = 0; i < item.Size; i++)
                        {
                            for (int j = 0; j < bytesCount; j++)
                            {
                                ret.ByteValueList.Add(data.Length > index ? data[index++] : (byte)0);
                            }
                        }
                    }
                    else if (item.SizeRef != null && item.SizeRef == MsgLength)
                    {
                        while (data.Length - GetTailSize(HeaderDefinition) > index)
                        {
                            for (int j = 0; j < bytesCount; j++)
                            {
                                ret.ByteValueList.Add(data.Length > index ? data[index++] : (byte)0);
                            }
                        }
                    }
                    else if (item.SizeRef != null)
                    {
                        for (int i = 0; i < GetSize(item.SizeRef, vgParams); i++)
                        {
                            for (int j = 0; j < bytesCount; j++)
                            {
                                ret.ByteValueList.Add(data.Length > index ? data[index++] : (byte)0);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < bytesCount; j++)
                        {
                            ret.ByteValueList.Add(data.Length > index ? data[index++] : (byte)0);
                        }
                    }
                }
            }
            return ret;
        }

        private static byte GetParamValueBytes(Param item)
        {
            return (byte)(item.Bits / 8);
        }

        private bool IsIncluded(string optRef)
        {
            bool ret = false;
            string[] tokens = optRef.Split('.');
            ParamValue p = ParamValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            if (p != null)
            {
                if (tokens.Length == 2)
                {
                    ParamValue s = p.InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[1]);
                    if (s != null)
                    {
                        ret = s.ByteValueList[0] > 0;
                    }
                }
                else
                {
                    ret = p.ByteValueList[0] > 0;
                }
            }
            return ret;
        }

        private byte GetSize(string sizeRef, IList<ParamValue> vgParams)
        {
            byte ret = 0;
            string[] tokens = sizeRef.Split('.');
            bool inParent = false;
            if (sizeRef.StartsWith("Parent."))
            {
                tokens = sizeRef.Substring(7).Split('.');
                inParent = true;
            }
            ParamValue p;
            if (vgParams == null || inParent)
            {
                p = ParamValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            }
            else
            {
                p = vgParams.FirstOrDefault(x => x.ParamDefinition.Name == tokens[0]);
            }

            if (p != null)
            {
                if (tokens.Length == 2)
                {
                    ParamValue s = p.InnerValues.FirstOrDefault(x => x.ParamDefinition.Name == tokens[1]);
                    if (s != null)
                    {
                        ret = s.ByteValueList[0];
                    }
                }
                else
                {
                    ret = p.ByteValueList[0];
                }
            }
            return ret;
        }

        private static byte GetParamBits(Param prm)
        {
            byte ret = prm.Bits;
            if (ret == 0)
                ret = 8;
            if (prm.Size > 1)
                ret *= prm.Size;
            return ret;
        }

        private static byte GetTailSize(Header cmd)
        {
            byte ret = 0;
            if (cmd.Param != null && cmd.Param.Count > 0)
            {
                for (int i = cmd.Param.Count - 1; i >= 0; i--)
                {
                    Param item = cmd.Param[i];
                    if (item.SizeRef != null || item.Param1 != null && item.Param1.Count > 0)
                        break;
                    ret += GetParamBits(item);
                }
            }
            return (byte)(ret / 8);
        }
    }
}
