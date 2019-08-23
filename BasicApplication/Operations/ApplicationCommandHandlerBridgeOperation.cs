﻿using System;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    [Obsolete]
    public class ApplicationCommandHandlerBridgeOperation : ApiOperation
    {
        public Action<byte[]> ReceivedCallback { get; set; }
        public ApplicationCommandHandlerBridgeOperation()
            : base(false, CommandTypes.CmdApplicationCommandHandler_Bridge, false)
        {
        }

        private ApiHandler applicationCommandHandler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(applicationCommandHandler, OnReceived));
        }

        protected override void CreateInstance()
        {
            applicationCommandHandler = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            ReceivedCallback(ou.DataFrame.Payload);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
