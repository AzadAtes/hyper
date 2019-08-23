﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class GetControllerCapabilitiesOperation : RequestApiOperation
    {
        public GetControllerCapabilitiesOperation()
            : base(CommandTypes.CmdZWaveGetControllerCapabilities, false)
        {
        }

        protected override byte[] CreateInputParameters()
        {
            return null;
        }

        protected override void SetStateCompleted(ActionUnit ou)
        {
            byte[] res = ((DataReceivedUnit)ou).DataFrame.Payload;
            SpecificResult.ControllerCapability = res[0];
            base.SetStateCompleted(ou);
        }

        public GetControllerCapabilitiesResult SpecificResult
        {
            get { return (GetControllerCapabilitiesResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new GetControllerCapabilitiesResult();
        }
    }

    public class GetControllerCapabilitiesResult : ActionResult
    {
        public byte ControllerCapability { get; set; }
    }
}
