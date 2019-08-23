﻿using System;
using System.Diagnostics;
using System.Threading;

namespace Utils.Threading
{
    public class Signal : IDisposable
    {
        private readonly AutoResetEvent _signal;
        public Signal()
            : this(false)
        {
        }

        public Signal(bool initialState)
        {
            _signal = new AutoResetEvent(initialState);
        }

        [DebuggerHidden]
        public void WaitOne()
        {
            if (!_isDisposed)
            {
                _signal.WaitOne();
            }
        }

        [DebuggerHidden]
        public bool WaitOne(int timeoutMs)
        {
            if (!_isDisposed)
            {
                return _signal.WaitOne(timeoutMs);
            }
            return true;
        }

        [DebuggerHidden]
        public void Set()
        {
            if (!_isDisposed)
            {
                _signal.Set();
            }
        }

        [DebuggerHidden]
        public void Reset()
        {
            if (!_isDisposed)
            {
                _signal.Reset();
            }
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Set();
                    _signal.Close();
                }
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
