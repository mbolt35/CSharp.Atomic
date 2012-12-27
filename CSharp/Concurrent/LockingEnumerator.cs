////////////////////////////////////////////////////////////////////////////////
//
//  MATTBOLT.BLOGSPOT.COM
//  Copyright(C) 2012 Matt Bolt
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

namespace CSharp.Concurrent {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;


    /// <summary>
    /// This class serves as a thread-safe wrapper for an <c>IEnumerator</c> implementation.
    /// It locks as soon as the instance is created and will release the lock once disposed.
    /// </summary>
    /// <typeparam name="T">The type of the enumerator.</typeparam>
    public class LockingEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable {
        private readonly IEnumerator<T> _enumerator;
        private readonly object _mutex;

        public LockingEnumerator(IEnumerator<T> enumerator, object mutex) {
            _enumerator = enumerator;
            _mutex = mutex;

            Monitor.Enter(_mutex);
        }

        public void Dispose() {
            Monitor.Exit(_mutex);
        }

        public bool MoveNext() {
            return _enumerator.MoveNext();
        }

        public void Reset() {
           _enumerator.Reset();
        }

        public T Current {
            get {
                return _enumerator.Current;
            }
        }

    }
}
