﻿using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0xD6 | bInclRequestIntervals
    /// ZW->HOST: RES | 0xD6 | RetVal
    /// </summary>
    public class SetMaxInclusionRequestIntervalsOperation : ControlApiOperation
    {
        private byte RequestIntervals { get; set; }
        public SetMaxInclusionRequestIntervalsOperation(byte requestIntervals)
            : base(CommandTypes.CmdZWaveSetMaxInclusionRequestIntervals, false)
        {
            RequestIntervals = requestIntervals;
        }

        protected override byte[] CreateInputParameters()
        {
            return new byte[] { RequestIntervals };
        }
    }
}
