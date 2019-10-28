﻿using System;
using System.Linq;
using ZWave.Enums;
using ZWave.Layers;
using ZWave.Layers.Frame;

namespace ZWave.TextApplication
{
    public class TextFrameClient : IFrameClient
    {
        public byte SessionId { get; set; }
        public Action<CustomDataFrame> ReceiveFrameCallback { get; set; }
        public Func<byte[], int> SendDataCallback { get; set; }
        private readonly Action<TextDataFrame> mTransmitCallback;
        public void ResetParser()
        { }

        public TextFrameClient(Action<TextDataFrame> transmitCallback)
        {
            mTransmitCallback = transmitCallback;
        }

        private byte[] CreateBuffer(CommandMessage frame)
        {
            byte[] tmp = CreateBufferInner(frame.SequenceNumber, frame.Data);
            return tmp;
        }

        private int WriteData(byte[] data)
        {
            if (mTransmitCallback != null)
            {
                TextDataFrame dataFrame = new TextDataFrame(SessionId, DataFrameTypes.Data, false, true, DateTime.Now);
                dataFrame.SetBuffer(data, 0, data.Length);
                mTransmitCallback(dataFrame);
            }
            if (SendDataCallback != null)
                return SendDataCallback(data);
            return -1;
        }

        public void HandleData(DataChunk dataChunk, bool isFromFile)
        {
            if (dataChunk.ApiType == ApiTypes.Text)
            {
                byte[] data = dataChunk.GetDataBuffer();
                ParseRawData(data, dataChunk.IsOutcome, dataChunk.TimeStamp, isFromFile);
            }
        }

        internal void ParseRawData(byte[] data, bool isOutcome, DateTime timeStamp, bool isFromFile)
        {
            if (data != null && data.Length > 0)
            {
                TextDataFrame frame = new TextDataFrame(SessionId, DataFrameTypes.Data, isFromFile, isOutcome, timeStamp);
                frame.SetBuffer(data, 0, data.Length);
                if (mTransmitCallback != null)
                    mTransmitCallback(frame);
                if (ReceiveFrameCallback != null)
                    ReceiveFrameCallback(frame);
            }
        }

        #region IFrameClient Members

        public bool SendFrames(ActionHandlerResult frameData)
        {
            bool ret = false;
            if (frameData != null && frameData.NextActions != null)
            {
                var sendFrames = frameData.NextActions.Where(x => x is TextApiMessage);
                if (sendFrames.Any())
                {
                    foreach (TextApiMessage frame in sendFrames)
                    {
                        byte[] tmp = CreateBuffer(frame);
                        int res = WriteData(tmp);
                        ret = res > 0;
                        frameData.Parent.AddTraceLogItem(DateTime.Now, tmp, true);
                    }
                    if (frameData.NextFramesCompletedCallback != null)
                        frameData.NextFramesCompletedCallback(ret);
                }
            }
            return ret;
        }

        #endregion

        private byte[] CreateBufferInner(byte seqNo, params byte[] parameters)
        {
            byte[] ret = null;
            ret = parameters;
            return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
