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

namespace CSharp.Threading {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CSharp.Collections.Concurrent;
    using CSharp.Atomic;


    /// <summary>
    /// This delegate is used to handle exceptions that occur in the executor. 
    /// </summary>
    /// <param name="exception">The <c>Exception</c> that occurred.</param>
    public delegate void ExceptionHandler(Exception exception);

    /// <summary>
    /// This executor uses a single thread and executes queued actions sequentially using an
    /// internal <c>BlockingQueue</c>. 
    /// </summary>
    /// <author>Matt Bolt</author>
    public class SingleThreadExecutor {

        private readonly BlockingQueue<Action> _actions;

        private Thread _thread;
        private ThreadPriority _priority;
        private AtomicBoolean _running;
        private AtomicBoolean _shuttingDown;

        /// <summary>
        /// Creates a new <c>SingleThreadExecutor</c> using a normal thread priority.
        /// </summary>
        public SingleThreadExecutor()
            : this(ThreadPriority.Normal) {
            
        }

        /// <summary>
        /// Creates a new <c>SingleThreadExecutor</c> using a custom thread priority.
        /// </summary>
        /// <param name="priority">
        /// The priority to assign the thread.
        /// </param>
        public SingleThreadExecutor(ThreadPriority priority) {
            _actions = new BlockingQueue<Action>();
            _running = new AtomicBoolean(false);
            _shuttingDown = new AtomicBoolean(false);

            _priority = priority;
        }

        /// <summary>
        /// Queues an <c>Action</c> for execution by a single thread. <c>Action</c> items are
        /// executed in the same order in which they are added.
        /// </summary>
        /// <param name="action">
        /// The <c>Action</c> delegate to queue for execution.
        /// </param>
        public void Execute(Action action) {
            if (_shuttingDown.Get()) {
                throw new Exception("Executor is shutting down.");
            }

            if (_running.CompareAndSet(false, true)) {
                _thread = new Thread(ExecuteAction);
                _thread.IsBackground = true;
                _thread.Priority = _priority;
                _thread.Start();
            }

            _actions.Enqueue(action);
        }

        /// <summary>
        /// Completes the current queue and joins the thread.
        /// </summary>
        public void Shutdown() {
            if (!_running.Get()) {
                return;
            }

            if (_shuttingDown.CompareAndSet(false, true)) {
                _actions.Enqueue(() => {
                    _running.CompareAndSet(true, false);
                });
                _thread.Join();
            }
        }

        /// <summary>
        /// Attempts to abort the <c>Thread</c> in the current state.
        /// </summary>
        public void ShutdownNow() {
            if (!_running.Get()) {
                return;
            }

            if (_shuttingDown.CompareAndSet(false, true)) {
                _running.CompareAndSet(true, false);

                try {
                    _thread.Abort();
                } catch (Exception e) {
                    if (null != OnException) {
                        OnException(e);
                    }
                }
            }
        }

        private void ExecuteAction() {
            while (_running.Get()) {
                try {
                    Action action = _actions.Dequeue();
                    action();
                } catch (Exception e) {
                    if (null != OnException) {
                        OnException(e);
                    }
                }
            }
        }

        /// <summary>
        /// The pending <c>Action</c>s left to execute.
        /// </summary>
        public int Pending {
            get {
                return _actions.Count;
            }
        }

        /// <summary>
        /// This event dispatches when an <c>Exception</c> occurs one of the queued <c>Action</c>
        /// executions.
        /// </summary>
        public event ExceptionHandler OnException;

    }
}
