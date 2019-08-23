﻿using Utils;
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    /// <summary>
    /// HOST->ZW: REQ | 0x61 | nodeID | funcID
    /// ZW->HOST: RES | 0x61 | retVal
    /// ZW->HOST: REQ | 0x61 | funcID | txStatus
    /// </summary>
    public class RemoveFailedNodeIdOperation : ApiOperation
    {
        public byte ReplacedNodeId { get; set; }
        public RemoveFailedNodeIdOperation(byte nodeId)
            : base(true, CommandTypes.CmdZWaveRemoveFailedNodeId, true)
        {
            ReplacedNodeId = nodeId;
        }

        public FailedNodeStatuses FailedNodeStatus { get; set; }
        public FailedNodeRetValues FailedNodeRetValues { get; set; }

        private ApiMessage messageStart;

        private ApiHandler handlerRemoveStarted;
        private ApiHandler handlerNotPrimary;
        private ApiHandler handlerNoCallback;
        private ApiHandler handlerNotFound;
        private ApiHandler handlerRemoveBusy;
        private ApiHandler handlerRemoveFail;

        private ApiHandler handlerNodeOk;
        private ApiHandler handlerRemoved;
        private ApiHandler handlerNotRemoved;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 30000, messageStart));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveStarted, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNotPrimary, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNoCallback, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerNotFound, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveBusy, OnRetValue));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoveFail, OnRetValue));

            ActionUnits.Add(new DataReceivedUnit(handlerNodeOk, OnCallback));
            ActionUnits.Add(new DataReceivedUnit(handlerRemoved, OnCallback));
            ActionUnits.Add(new DataReceivedUnit(handlerNotRemoved, OnCallback));
        }

        protected override void CreateInstance()
        {
            messageStart = new ApiMessage(SerialApiCommands[0], ReplacedNodeId);
            messageStart.SetSequenceNumber(SequenceNumber);

            handlerRemoveStarted = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveStarted.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED));

            handlerNotPrimary = new ApiHandler(SerialApiCommands[0]);
            handlerNotPrimary.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_NOT_PRIMARY_CONTROLLER));

            handlerNoCallback = new ApiHandler(SerialApiCommands[0]);
            handlerNoCallback.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_NO_CALLBACK_FUNCTION));

            handlerNotFound = new ApiHandler(SerialApiCommands[0]);
            handlerNotFound.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_NOT_FOUND));

            handlerRemoveBusy = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveBusy.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_PROCESS_BUSY));

            handlerRemoveFail = new ApiHandler(SerialApiCommands[0]);
            handlerRemoveFail.AddConditions(new ByteIndex((byte)FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_FAIL));


            handlerNodeOk = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNodeOk.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.ZW_NODE_OK));

            handlerRemoved = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerRemoved.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.ZW_FAILED_NODE_REMOVED));

            handlerNotRemoved = new ApiHandler(FrameTypes.Request, SerialApiCommands[0]);
            handlerNotRemoved.AddConditions(ByteIndex.AnyValue, new ByteIndex((byte)FailedNodeStatuses.ZW_FAILED_NODE_NOT_REMOVED));
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }

        private void OnRetValue(DataReceivedUnit ou)
        {
            FailedNodeRetValues = (FailedNodeRetValues)ou.DataFrame.Payload[0];
            if (FailedNodeRetValues != FailedNodeRetValues.ZW_FAILED_NODE_REMOVE_STARTED)
            {
                SetStateFailed(ou);
            }
        }

        private void OnCallback(DataReceivedUnit ou)
        {
            FailedNodeStatus = (FailedNodeStatuses)ou.DataFrame.Payload[1];
            if (FailedNodeStatus == FailedNodeStatuses.ZW_FAILED_NODE_REMOVED)
            {
                SetStateCompleted(ou);
            }
            else
            {
                SetStateFailed(ou);
            }
        }
    }
}
