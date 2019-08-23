﻿using System.Linq;
using System.Threading;
using ZWave.BasicApplication.Security;
using ZWave.CommandClasses;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class SendDataFragmentedSecureTask : ApiOperation
    {
        #region Timeouts
        /// <summary>
        /// Nonce Request Timer
        /// </summary>
        const int NONCE_REQUEST_INCLUSION_TIMER = 10000;
        /// <summary>
        /// Nonce Request Timer
        /// </summary>
        const int NONCE_REQUEST_TIMER = 20000;
        #endregion

        protected TransmitOptions TxOptions { get; set; }
        internal byte[] CommandToSecureSend { get; private set; }
        internal int DataDelay { get; set; }
        internal byte NodeId { get; private set; }
        internal int MaxBytesPerFrameSize { get; set; }
        internal byte SequenceCounter { get; set; }
        //public Action<ActionUnit> OnHandledCallback { get; set; }

        private SecurityManagerInfo _securityManagerInfo;
        private SecurityS0CryptoProvider _securityS0CryptoProvider;

        internal SendDataFragmentedSecureTask(SecurityManagerInfo securityManagerInfo, SecurityS0CryptoProvider securityS0CryptoProvider, byte nodeId, byte[] data, TransmitOptions txOptions)
            : base(false, null, false)
        {
            _securityManagerInfo = securityManagerInfo;
            _securityS0CryptoProvider = securityS0CryptoProvider;
            SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);
            CommandToSecureSend = data;
            NodeId = nodeId;
            TxOptions = txOptions;
        }

        RequestDataOperation requestNonce;
        RequestDataOperation sendFirstEncData;
        SendDataOperation sendSecondEncData;

        protected override void CreateWorkflow()
        {
            if (_securityManagerInfo.TestFirstFragmentedPartDisabled)
            {
                ActionUnits.Add(new StartActionUnit(OnStart, 0, requestNonce));
                ActionUnits.Add(new ActionCompletedUnit(requestNonce, OnNonceReportSecond, sendSecondEncData));
                ActionUnits.Add(new ActionCompletedUnit(sendSecondEncData, OnSendCompleted));
            }
            else
            {
                ActionUnits.Add(new StartActionUnit(OnStart, 0, requestNonce));
                ActionUnits.Add(new ActionCompletedUnit(requestNonce, OnNonceReport, sendFirstEncData));

                if (_securityManagerInfo.TestSecondFragmentedPartDisabled)
                {
                    ActionUnits.Add(new ActionCompletedUnit(sendFirstEncData, OnSendOnlyFirstCompleted));
                }
                else
                {
                    ActionUnits.Add(new ActionCompletedUnit(sendFirstEncData, OnRequestFirstEncData, sendSecondEncData));
                    ActionUnits.Add(new ActionCompletedUnit(sendSecondEncData, OnSendCompleted));
                }
            }
        }

        protected override void CreateInstance()
        {
            requestNonce = new RequestDataOperation(0, NodeId,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_GET(), TxOptions,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT(), 2, NONCE_REQUEST_TIMER);
            if (_securityManagerInfo.IsInclusion)
                requestNonce.Token.Reset(NONCE_REQUEST_INCLUSION_TIMER);
            requestNonce.SubstituteSettings.SetFlag(SubstituteFlags.DenySecurity);

            sendFirstEncData = new RequestDataOperation(0, NodeId,
                null, TxOptions,
                new COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT(), 2, NONCE_REQUEST_TIMER);
            sendFirstEncData.SubstituteSettings.SetFlag(SubstituteFlags.DenyTransportService | SubstituteFlags.DenySecurity);

            sendSecondEncData = new SendDataOperation(NodeId, null, TxOptions);
            sendSecondEncData.SubstituteSettings.SetFlag(SubstituteFlags.DenyTransportService | SubstituteFlags.DenySecurity);
        }

        private void OnStart(StartActionUnit ou)
        {
            if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.NonceGet))
            {
                Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.NonceGet]);
            }
        }

        private void OnNonceReport(ActionCompletedUnit ou)
        {
            AddTraceLogItems(requestNonce.SpecificResult.TraceLog);
            if (requestNonce.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = requestNonce.SpecificResult.Command;
                if (cmd.nonceByte != null && cmd.nonceByte.Count == 8)
                {
                    byte[] firstFragmentCmd = CommandToSecureSend.Take(MaxBytesPerFrameSize).ToArray();
                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET.Tproperties1 property = 0;
                    property.sequenceCounter = SequenceCounter;
                    property.sequenced = 1;
                    property.secondFrame = 0;
                    byte[] msg = _securityS0CryptoProvider.Encrypt(property, firstFragmentCmd, _securityManagerInfo.Network.NodeId, NodeId, cmd.nonceByte.ToArray());

                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION_NONCE_GET msgCmd = msg;
                    sendFirstEncData.Data = msgCmd;
                    if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.Command))
                    {
                        Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.Command]);
                    }
                }
                else
                    SetStateFailed(ou);
            }
            else
                SetStateFailed(ou);
        }

        private void OnNonceReportSecond(ActionCompletedUnit ou)
        {
            AddTraceLogItems(requestNonce.SpecificResult.TraceLog);
            if (requestNonce.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = requestNonce.SpecificResult.Command;
                if (cmd.nonceByte != null && cmd.nonceByte.Count == 8)
                {
                    byte[] secondFragmentCmd = CommandToSecureSend.Skip(MaxBytesPerFrameSize).Take(MaxBytesPerFrameSize).ToArray();
                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.Tproperties1 property = 0;
                    property.sequenceCounter = SequenceCounter;
                    property.sequenced = 1;
                    property.secondFrame = 1;
                    byte[] msg = _securityS0CryptoProvider.Encrypt(property, secondFragmentCmd, _securityManagerInfo.Network.NodeId, NodeId, cmd.nonceByte.ToArray());

                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION msgCmd = msg;
                    sendSecondEncData.Data = msgCmd;
                    //sendSecondEncData.OnHandledCallback = OnHandledCallback;
                    if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.Command))
                    {
                        Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.Command]);
                    }
                }
                else
                    SetStateFailed(ou);
            }
            else
                SetStateFailed(ou);
        }

        private void OnRequestFirstEncData(ActionCompletedUnit ou)
        {
            AddTraceLogItems(sendFirstEncData.SpecificResult.TraceLog);
            if (sendFirstEncData.Result.State == ActionStates.Completed)
            {
                COMMAND_CLASS_SECURITY.SECURITY_NONCE_REPORT cmd = sendFirstEncData.SpecificResult.Command;
                if (cmd.nonceByte != null && cmd.nonceByte.Count == 8)
                {
                    byte[] secondFragmentCmd = CommandToSecureSend.Skip(MaxBytesPerFrameSize).Take(MaxBytesPerFrameSize).ToArray();
                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION.Tproperties1 property = 0;
                    property.sequenceCounter = SequenceCounter;
                    property.sequenced = 1;
                    property.secondFrame = 1;
                    byte[] msg = _securityS0CryptoProvider.Encrypt(property, secondFragmentCmd, _securityManagerInfo.Network.NodeId, NodeId, cmd.nonceByte.ToArray());

                    COMMAND_CLASS_SECURITY.SECURITY_MESSAGE_ENCAPSULATION msgCmd = msg;
                    sendSecondEncData.Data = msgCmd;
                    //sendSecondEncData.OnHandledCallback = OnHandledCallback;
                    if (_securityManagerInfo.DelaysS0.ContainsKey(SecurityS0Delays.Command))
                    {
                        Thread.Sleep(_securityManagerInfo.DelaysS0[SecurityS0Delays.Command]);
                    }
                }
                else
                    SetStateFailed(ou);
            }
            else
                SetStateFailed(ou);
        }

        private void OnSendCompleted(ActionCompletedUnit ou)
        {
            AddTraceLogItems(ou.Action.Result.TraceLog);
            if (ou.Action.Result.State == ActionStates.Completed)
            {
                SetStateCompleted(ou);
            }
            else
                SetStateFailed(ou);
        }

        private void OnSendOnlyFirstCompleted(ActionCompletedUnit ou)
        {
            //OnHandledCallback(ou);
            AddTraceLogItems(ou.Action.Result.TraceLog);
            if (ou.Action.Result.State == ActionStates.Completed)
            {
                SetStateCompleting(ou);
            }
            else
                SetStateFailed(ou);
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
