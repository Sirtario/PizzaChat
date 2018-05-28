using System;
using System.Threading;

namespace ACX.ViciOne.TCPLibrary
{
    internal class ThreadCancelationToken
    {
        private ManualResetEventSlim _manualResetEventThreadEnding = new ManualResetEventSlim();

        public bool IsCancelationRequested { get; private set; }

        public void Cancel()
        {
            IsCancelationRequested = true;
        }

        public void AwaitCancel(int timeOut)
        {
            Cancel();

            if (!_manualResetEventThreadEnding.Wait(timeOut))
            {
                throw new TimeoutException($"Threadcancelation exceedet timeout: {timeOut}");
            }
        }

        internal void Canceled()
        {
            _manualResetEventThreadEnding.Set();
        }
    }
}