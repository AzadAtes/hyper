﻿using System;

namespace ZWave
{
    public class ActionLinkedGroup : ActionGroup
    {
        private Action<ActionBase, ActionResult> OnSecondActionCompleted { get; set; }
        private ActionBase FirstAction { get; set; }
        private ActionBase SecondAction { get; set; }
        public ActionLinkedGroup(ActionBase firstAction, ActionBase secondAction, Action<ActionBase, ActionResult> onSecondActionCompleted)
        {
            FirstAction = firstAction;
            SecondAction = secondAction;
            OnSecondActionCompleted = onSecondActionCompleted;
        }

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0, FirstAction, SecondAction));
            ActionUnits.Add(new ActionCompletedUnit(FirstAction, OnFirstCompleted));
            ActionUnits.Add(new ActionCompletedUnit(SecondAction, OnSecondCompleted));
        }

        private void OnSecondCompleted(ActionCompletedUnit ou)
        {
            OnSecondActionCompleted(FirstAction, ou.Action.Result);
            Result.InnerResults.Add(ou.Action.Result);
            if (!FirstAction.Token.IsStateActive)
                SetStateCompleted(ou);
        }

        private void OnFirstCompleted(ActionCompletedUnit ou)
        {
            Result.InnerResults.Add(ou.Action.Result);
            if (!SecondAction.Token.IsStateActive)
                SetStateCompleted(ou);
        }

        protected override void CreateInstance()
        {
            FirstAction.ParentAction = this;
            SecondAction.ParentAction = this;
        }

        protected override ActionResult CreateOperationResult()
        {
            return new ActionResult();
        }
    }
}
