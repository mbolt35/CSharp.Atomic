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
    using System.Threading;
    using CSharp.Atomic;


    /// <summary>
    /// This class acts as a lock-less, thread safe count down which can be used to perform multiple async tasks 
    /// and await signals from all threads without having to wait for the thread to terminate. It's based on the 
    /// <see href="http://docs.oracle.com/javase/7/docs/api/java/util/concurrent/CountDownLatch.html">
    /// java.util.concurrent.CountDownLatch</see> class found in Java.
    /// </summary>
    /// <remarks>
    /// This class performs a very similar task to the <c>CountDownEvent</c> class from .NET 4.5, but is compatible 
    /// with .NET 3.0+ for use with platforms like Unity3D.
    /// </remarks>
    /// <author>Matt Bolt</author>
    public class CountDownLatch {

        private AtomicInt _count;
        private ManualResetEvent _wait;

        /// <summary>
        /// Creates a new <c>CountDownLatch</c> instance with the provided counter initializer.
        /// </summary>
        /// <param name="count">
        /// The initial value of the counter. Note that this value must be greater than 0.
        /// </param>
        public CountDownLatch(int count) {
            if (count <= 0) {
                throw new Exception("Count value cannot be less than or equal to 0.");
            }

            _wait = new ManualResetEvent(false);
            _count = new AtomicInt(count);
        }

        /// <summary>
        /// This method decrements the counter by <c>1</c>. If the counter reaches 0, the reset event is set.
        /// </summary>
        public void CountDown() {
            if (_count.PreDecrement() <= 0) {
                _wait.Set();
            }
        }


        /// <summary>
        /// Blocks execution of the current thread until the counter has reached 0.
        /// </summary>
        public void Await() {
            _wait.WaitOne();
        }

        /// <summary>
        /// Blocks execution of the current thread until the counter has reached 0 or the timeout expires.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The total amount of time, in milliseconds, to wait before continuing.
        /// </param>
        public void Await(int millisecondsTimeout) {
            _wait.WaitOne(millisecondsTimeout);
        }

        /// <summary>
        /// Blocks execution of the current thread until the counter has reached 0 or the timeout expires.
        /// </summary>
        /// <param name="timeout">
        /// The <c>TimeSpan</c> instance used as the time to wait before continuing.
        /// </param>
        public void Await(TimeSpan timeout) {
            _wait.WaitOne(timeout);
        }

        /// <summary>
        /// The current value of the counter.
        /// </summary>
        public int CurrentCount {
            get {
                return _count.Get();
            }
        }
    }
}