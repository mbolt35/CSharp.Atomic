////////////////////////////////////////////////////////////////////////////////
//
//  MATTBOLT.BLOGSPOT.COM
//  Copyright(C) 2013 Matt Bolt
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at:
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

namespace CSharp.Locking {

    using System;
    using System.Collections.Generic;
    using CSharp.Util;


    /// <summary>
    /// The default <c>ICondition</c> implementation for use with <c>CSharp.Locking.ReentrantLock</c>. 
    /// It allows specific conditions to wait and signal, while being intrinsic to the root lock.
    /// </summary>
    public class LockCondition : ICondition {

        private readonly string _signalId;
        private readonly LockMonitor _monitor;
        private readonly HashSet<string> _waitingThreads;
        private readonly HashStack<string, string> _threadSignals;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharp.Locking.LockCondition"/> class.
        /// </summary>
        /// <param name="signalId">Signal identifier used to notify waiting threads which condition was statisfied.</param>
        /// <param name="monitor">The <c>LockMonitor</c> used in the parent lock.</param>
        public LockCondition(string signalId, LockMonitor monitor) {
            _signalId = signalId;
            _monitor = monitor;
            _waitingThreads = new HashSet<string>();
            _threadSignals = new HashStack<string, string>();
        }

        /// <summary>
        /// Signals all threads waiting on this condition.
        /// </summary>
        public void Signal() {
            // For signal, we iterate through all of the waiting threads and push the condition
            // id onto the stack for that particular thread. Once this is done, we are safe to 
            // pulse the monitor to release waiting threads. 
            foreach (string threadId in _waitingThreads) {
                _threadSignals.Push(threadId, _signalId);
            }

            _monitor.PulseAll();
        }

        /// <summary>
        /// Blocks the current thread until the condition is signaled.
        /// </summary>
        public void Await() {
            _waitingThreads.Add(ThreadHelper.CurrentThreadId);

            // Wait until signaled, at which time, we check to see if the signal
            // for the released thread matches this condition's id, and if so, the
            // thread is no longer in the waiting state
            string signal = null;
            while (signal != _signalId) {
                _monitor.Wait();

                signal = _threadSignals.Pop(ThreadHelper.CurrentThreadId);
            }

            // If we've made it this far, the current thread is no longer waiting
            _waitingThreads.Remove(ThreadHelper.CurrentThreadId);
        }

    }
}

