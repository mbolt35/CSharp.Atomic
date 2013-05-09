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
    using System.Threading;


    /// <summary>
    /// This interface represents a thread access locking implementation.
    /// 
    /// <para>The common use case for these objects will in a <c>try/finally</c> block:</para>
    /// @code
    /// ILock _lock;
    /// List<int> _integers = new List<int>();
    /// 
    /// void AddToList(int integer) {
    ///     _lock.Lock();
    ///     try { 
    ///         _integers.Add(integer);
    ///     }
    ///     finally {
    ///         _lock.Unlock();
    ///     }
    /// }
    /// @endcode
    /// 
    /// </summary>
    /// \author Matt Bolt
    public interface ILock {

        /// <summary>
        /// Acquires the lock if it is available. If not available, the current <c>Thread</c> will block
        /// until it is available.
        /// </summary>
        void Lock();

        /// <summary>
        /// Attempts to acquire the lock, given a timeout in milliseconds.
        /// </summary>
        /// <returns><c>true</c> if the lock was available and acquired. Otherwise, <c>false</c> is returned
        /// if the <c>timeoutMs</c> elapses before the lock can be acquired.</returns>
        /// <param name="timeoutMs">The maximum amount of time to wait before returning a <c>false</c></param>
        bool TryLock(int timeoutMs);

        /// <summary>
        /// Attempts to acquire the lock, given a timeout as a <c>TimeSpan</c>.
        /// </summary>
        /// <returns><c>true</c> if the lock was available and acquired. Otherwise, <c>false</c> is returned
        /// if the <c>timeout</c> elapses before the lock can be acquired.</returns>
        /// <param name="timeout">The maximum amount of time to wait before returning a <c>false</c></param>
        bool TryLock(TimeSpan timeout);

        /// <summary>
        /// Releases the lock.
        /// </summary>
        void Unlock();

        /// <summary>
        /// Returns an <c>ICondition</c> instance for use with this <c>ILock</c> instance.
        ///
        /// <para>
        /// The returned <c>ICondition</c> instance supports the same usages as do the <c>Monitor</c> methods 
        /// (<c>Monitor.Wait()</c> and <c>Monitor.PulseAll()</c>).
        /// </para>
        ///
        /// <list>
        /// <item>
        /// If this lock is not held when any of the <c>ICondition.Await()</c> or <c>ICondition.Signal()<c/>
        /// methods are called, then an exception is thrown.
        /// </item>
        /// <item>
        /// When the condition <c>ICondition.Await()</c> methods are called the lock is released and, before they
        /// return, the lock is reacquired and the lock hold count restored to what it was when the method was called.
        /// </item>
        /// <item>
        /// The ordering of lock reacquisition for threads returning
        /// from waiting methods is the same as for threads initially
        /// acquiring the lock, which is in the default case not specified.
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>The <c>ICondition</c> object</returns>
        ICondition NewCondition();

        /// <summary>
        /// Gets a value indicating whether the current thread is the lock holder.
        /// Note that for locks 
        /// </summary>
        /// <value><c>true</c> if the current thread is the lock holder; otherwise, <c>false</c>.</value>
        bool IsHeldByCurrentThread {
            get;
        }
    }
}

