﻿using System;
using Utils;
using ZWave.Enums;
using ZWave.Security;

namespace ZWave.Layers.Frame
{
    public abstract class CustomDataFrame : IActionCase, IDataFrame
    {
        public bool IsSkippedSecurity { get; set; }
        public CustomDataFrame Parent { get; set; }
        protected abstract int GetMaxLength();
        protected abstract byte[] RefreshData();
        protected abstract byte[] RefreshPayload();
        public SubstituteIncomingFlags SubstituteIncomingFlags { get; set; }
        public byte SessionId { get; set; }
        public bool IsSubstituted { get; set; }
        public bool IsHandled { get; set; }
        public ApiTypes ApiType { get; set; }
        public bool IsOutcome { get; private set; }
        public bool IsInvalid { get; set; }
        public DataFrameTypes DataFrameType { get; set; }
        public DateTime SystemTimeStamp { get; private set; }
        public int CmdLength { get; private set; }
        public Extensions Extensions { get; set; }
        public CustomDataFrame(byte sessionId, DataFrameTypes type, bool isHandled, bool isOutcome, DateTime timeStamp)
        {
            SessionId = sessionId;
            DataFrameType = type;
            SystemTimeStamp = timeStamp;
            IsHandled = isHandled;
            IsOutcome = isOutcome;
        }

        private byte[] _buffer;
        public byte[] Buffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new byte[0];
                return _buffer;
            }
        }

        protected byte[] _data;
        public byte[] Data
        {
            get
            {
                if (_data == null)
                    _data = new byte[0];
                return _data;
            }
        }

        protected byte[] _payload;
        public byte[] Payload
        {
            get
            {
                if (_payload == null)
                    _payload = new byte[0];
                return _payload;
            }
        }

        public void SetBuffer(byte[] data, int offset, int length)
        {
            byte[] buffer = new byte[length];
            Array.Copy(data, offset, buffer, 0, length);
            _buffer = buffer;
            _data = RefreshData();
            _payload = RefreshPayload();
        }

        // workaround to set command length more than byte.MaxValue
        public void SetBuffer(byte[] data, int cmdLength)
        {
            SetBuffer(data, 0, data.Length);
            if (_payload != null)
            {
                CmdLength = cmdLength;
            }
        }

        public override string ToString()
        {
            return Tools.GetHex(Buffer);
        }
    }
}
