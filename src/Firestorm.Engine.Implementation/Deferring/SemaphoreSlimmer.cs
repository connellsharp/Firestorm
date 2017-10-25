using System;
using System.Threading;
using System.Threading.Tasks;

namespace Firestorm.Engine
{
    /// <summary>
    /// Provides similar functionality to <see cref="SemaphoreSlim"/> but does not use an unmanged manual wait handle and therefore doesn't need disposing.
    /// </summary>
    internal class SemaphoreSlimmer
    {
        private int _currentCount;
        private readonly object _lock = new object();
        private TaskCompletionSource<bool> _tcs;

        internal SemaphoreSlimmer(int initialCount)
        {
            _currentCount = initialCount;
        }

        public Task WaitAsync()
        {
            lock (_lock)
            {
                _currentCount++;

                if (_currentCount == 1)
                    return Task.FromResult(true);

                if (_tcs == null)
                    _tcs = new TaskCompletionSource<bool>();

                return _tcs.Task;
            }
        }

        public void Release()
        {
            lock (_lock)
            {
                if (_currentCount == 0)
                    throw new Exception("No task waiting on this semaphore to release.");

                _currentCount--;

                if (_currentCount > 0)
                    return;

                if (_tcs != null)
                {
                    _tcs.SetResult(true);
                    _tcs = null;
                }
            }
        }
    }
}