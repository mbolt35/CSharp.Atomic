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
    using CSharp.Atomic;


    /// <summary>
    /// This class stores a static instance of <c>T</c> in each <c>Thread</c> it is
    /// accessed. It uses an internal implementation of the <c>ThreadStatic</c>
    /// attribute. 
    /// </summary>
    /// <typeparam name="T">The type local to each thread.</typeparam>
    /// \author Matt Bolt
    public class ThreadLocal<T> {

        // Generate a unique ThreadLocal identifier for each instance
        private static readonly AtomicInt ThreadLocalIds;

        [ThreadStatic]
        private static Dictionary<int, T> Instances;

        static ThreadLocal() {
            ThreadLocalIds = new AtomicInt(0);
        }

        private int _id;
        private Func<T> _factory;

        /// <summary>
        /// Creates a new <c>ThreadLocal</c> instance using a <c>Func</c> capable
        /// of creating new <c>T</c> instances.
        /// </summary>
        /// <param name="factory">
        /// The <c>Func</c> instance used to create <c>T</c> instances.
        /// </param>
        public ThreadLocal(Func<T> factory) {
            _id = ThreadLocalIds.Increment();
            _factory = factory;
        }

        /// <summary>
        /// This method retreives the <c>T</c> instance local to the current <c>Thread</c>. 
        /// If an instance does not exist, one is created via the <c>Func</c> passed
        /// via the constructor.
        /// </summary>
        /// <returns>
        /// The <c>T</c> instance local to the current thread.
        /// </returns>
        public T Get() {
            if (null == Instances) {
                Instances = new Dictionary<int, T>();
            }

            if (!Instances.ContainsKey(_id)) {
                Instances[_id] = _factory();
            }

            return Instances[_id];
        }

        /// <summary>
        /// This method allows you to set the <c>T</c> instance local to the current
        /// <c>Thread</c>. 
        /// </summary>
        /// <param name="instance">
        /// The <c>T</c> instance to set on the local <c>Thread</c>.
        /// </param>
        public void Set(T instance) {
            if (null == Instances) {
                Instances = new Dictionary<int, T>();
            }

            Instances[_id] = instance;
        }
    }
    
}
