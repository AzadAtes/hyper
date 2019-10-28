﻿namespace ZWave.Operations
{
    public class SendOperation : ActionBase
    {
        private readonly byte[] _data;
        public SendOperation(byte[] data)
            : base(false)
        {
            _data = data;
        }

        private CommandMessage _sendData;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, _sendData));
        }

        protected override void CreateInstance()
        {
            _sendData = new CommandMessage();
            _sendData.Data = _data;
        }

        public SendResult SpecificResult
        {
            get { return (SendResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SendResult();
        }
    }

    public class SendResult : ActionResult
    {
    }
}