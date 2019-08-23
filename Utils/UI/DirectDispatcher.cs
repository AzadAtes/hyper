﻿using System;

namespace Utils.UI
{
    public class DirectDispatcher : IDispatch
    {
        public void BeginInvoke(Action action)
        {
            action();
        }

        public void Invoke(Action action)
        {
            action();
        }

        public bool InvokeBackground(Action action, int timeoutMs)
        {
            action();
            return true;
        }

        public bool InvokeBackground(Action action)
        {
            action();
            return true;
        }

        public bool CheckAccess()
        {
            return true;
        }
    }
}
