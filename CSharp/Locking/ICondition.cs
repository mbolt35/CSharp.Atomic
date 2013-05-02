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

    /// <summary>
    /// This interface defines a condition instance that can be used for flow control
    /// within a lock. 
    /// 
    /// <para>
    /// For instance, two conditions can be used to created a bounded buffer where one
    /// condition waits while the buffer is empty, and the other waits when the buffer 
    /// is full. When items are removed from the buffer, the condition waiting on full
    /// will be signaled. When items are added to the buffer, the condition waiting on
    /// empty will be signaled.
    /// </para>
    /// </summary>
    public interface ICondition {

        /// <summary>
        /// Signals all threads waiting on this condition.
        /// </summary>
        void Signal();

        /// <summary>
        /// Blocks the current thread until the condition is signaled.
        /// </summary>
        void Await();
    }
}

