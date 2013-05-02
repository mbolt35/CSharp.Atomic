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
    using CSharp.Atomic;


    /// <summary>
    /// This class is used to generate condition ids for lock instances. The ids generated
    /// need only be unique per each lock instance, and are *not* exposed outside of the lock.
    /// </summary>
    /// <author>Matt Bolt</author>
    public class ConditionIds {
        private readonly string _prefix;
        private readonly AtomicLong _ids;

        public ConditionIds(string prefix, long startingId) {
            _prefix = prefix;
            _ids = new AtomicLong(startingId);
        }

        /// <summary>
        /// Creates and returns the next condition id.
        /// </summary>
        /// <remarks>This method is thread safe.</remarks>
        public string Next() {
            return _prefix + '-' + _ids.Increment();
        }
    }
}

