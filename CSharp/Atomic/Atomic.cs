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

namespace CSharp.Atomic {

    using System;
    using System.Threading;

    /// <summary>
    /// Provides lock-free atomic read/write utility for a reference type, <c>T</c>, instance. The atomic classes found in this package
    /// were are meant to replicate the <c>java.util.concurrent.atomic</c> package in Java by Doug Lea. The two main differences
    /// are implicit casting back to the <c>T</c> data type, and the use of a non-volatile inner variable.
    /// 
    /// <para>The internals of these classes contain wrapped usage of the <c>System.Threading.Interlocked</c> class, which is how
    /// we are able to provide atomic operation without the use of locks. </para>
    /// </summary>
    /// <author>Matt Bolt</author>
    public class Atomic<T> where T : class {

        private T _value;

        /// <summary>
        ///     Creates a new <c>Atomic</c> instance with an initial value of <c>null</c>.
        /// </summary>
        public Atomic()
            : this(null) {

        }

        /// <summary>
        ///     Creates a new <c>Atomic</c> instance with the initial value provided.
        /// </summary>
        public Atomic(T value) {
            _value = value;
        }

        /// <summary>
        ///     This method returns the current value.
        /// </summary>
        /// <returns>
        ///     The <c>T</c> instance.
        /// </returns>
        public T Get() {
            return _value;
        }

        /// <summary>
        ///     This method sets the current value atomically.
        /// </summary>
        /// <param name="value">
        ///     The new value to set.
        /// </param>
        public void Set(T value) {
            Interlocked.Exchange(ref _value, value);
        }

        /// <summary>
        ///     This method atomically sets the value and returns the original value.
        /// </summary>
        /// <param name="value">
        ///     The new value.
        /// </param>
        /// <returns>
        ///     The value before setting to the new value.
        /// </returns>
        public T GetAndSet(T value) {
            return Interlocked.Exchange(ref _value, value);
        }

        /// <summary>
        ///     Atomically sets the value to the given updated value if the current value <c>==</c> the expected value.
        /// </summary>
        /// <param name="expected">
        ///     The value to compare against.
        /// </param>
        /// <param name="result">
        ///     The value to set if the value is equal to the <c>expected</c> value.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the comparison and set was successful. A <c>false</c> indicates the comparison failed.
        /// </returns>
        public bool CompareAndSet(T expected, T result) {
            return Interlocked.CompareExchange(ref _value, result, expected) == expected;
        }

        /// <summary>
        ///     This operator allows an implicit cast from <c>Atomic&lt;T&gt;</c> to <c>T</c>.
        /// </summary>
        public static implicit operator T(Atomic<T> value) {
            return value.Get();
        }

    }

}