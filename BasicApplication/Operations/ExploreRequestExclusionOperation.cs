﻿using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ExploreRequestExclusionOperation : ApiOperation
    {
        byte FuncId;
        public ExploreRequestExclusionOperation(byte funcId)
            : base(true, CommandTypes.CmdZWaveExploreRequestExclusion, true)
        {
            FuncId = funcId;
        }
        ApiMessage message;
        ApiHandler handlerOk;
        ApiHandler handlerFail;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 1000, message));
            ActionUnits.Add(new DataReceivedUnit(handlerOk, SetStateCompleted));
            ActionUnits.Add(new DataReceivedUnit(handlerFail, SetStateFailed));
        }

        protected override void CreateInstance()
        {
            message = new ApiMessage(SerialApiCommands[0]);
            if (FuncId > 0)
                message.SetSequenceNumber(FuncId);
            handlerOk = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handlerOk.AddConditions(new ByteIndex(0x01));
            handlerFail = new ApiHandler(FrameTypes.Response, SerialApiCommands[0]);
            handlerFail.AddConditions(new ByteIndex(0x00));
        }

    }
}
