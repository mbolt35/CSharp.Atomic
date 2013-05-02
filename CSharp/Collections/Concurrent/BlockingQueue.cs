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

namespace CSharp.Collections.Concurrent {

    using System;
    using System.Threading;
    using System.Collections;
    using System.Collections.Generic;
    using CSharp.Threading;
    using CSharp.Locking;


    /// <summary>
    /// This <c>IEnumerable</c> implementation can be used to throttle a producer/consumer model.
    /// </summary>
    /// <typeparam name="T">The type used in this queue.</typeparam>
    /// <author>Matt Bolt</author>
    public class BlockingQueue<T> : IEnumerable<T>, IEnumerable {
        private readonly ILock _lock;
        private readonly ICondition _notEmpty;
        private readonly Queue<T> _queue;

        public BlockingQueue() 
            : this(1) {

        }

        public BlockingQueue(int capacity) {
            _queue = new Queue<T>(capacity);
            _lock = new ReentrantLock();
            _notEmpty = _lock.NewCondition();
        }

        public BlockingQueue(IEnumerable<T> collection) {
            _queue = new Queue<T>(collection);
            _lock = new ReentrantLock();
            _notEmpty = _lock.NewCondition();
        }

        /// <summary>
        /// This method adds an item to the queue.
        /// </summary>
        /// <param name="item">
        /// The item to push into the queue.
        /// </param>
        public void Enqueue(T item) {
            _lock.Lock();
            try { 
                _queue.Enqueue(item);
                _notEmpty.Signal();
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// This method will remove and return the item in the front of the queue if one exists, or
        /// it will block the current thread until there is an item to dequeue.
        /// </summary>
        /// <returns>
        /// The <c>T</c> item in the front of the queue.
        /// </returns>
        public T Dequeue() {
            _lock.Lock();
            try { 
                while (_queue.Count == 0) {
                    _notEmpty.Await();
                }

                return _queue.Dequeue();
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// Removes all objects from the queue.
        /// </summary>
        public void Clear() {
            _lock.Lock();
            try {
                _queue.Clear();
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// Determines whether an element is in the queue.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the queue. The value can be null for reference types.
        /// </param>
        /// <returns>
        /// <c>true</c> if item is found in the queue. Otherwise, <c>false</c>
        /// </returns>
        public bool Contains(T item) {
            _lock.Lock();
            try {
                return _queue.Contains(item);
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// Copies the queue elements to an existing one-dimensional <c>T</c> array, starting at the specified array 
        /// index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <c>T</c> array that is the destination of the elements copied from 
        /// the queue. The <c>T</c> array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// array is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// arrayIndex is less than zero.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of elements in the source queue is greater than the available space from arrayIndex to the end of the 
        /// destination array.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex) {
            _lock.Lock();
            try {
                _queue.CopyTo(array, arrayIndex);
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// Returns the object at the beginning of the queue without removing it.
        /// </summary>
        /// 
        /// <returns>
        /// The object at the beginning of the queue.
        /// </returns>
        /// 
        /// <exception cref="System.InvalidOperationException">
        /// The queue is empty.
        /// </exception>
        public T Peek() {
            _lock.Lock();
            try {
                return _queue.Peek();
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// Copies the queue elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the queue</returns>
        public T[] ToArray() {
            _lock.Lock();
            try {
                return _queue.ToArray();
            }
            finally {
                _lock.Unlock();
            }
        }

        /// <summary>
        /// This property contains the size of the queue.
        /// </summary>
        public int Count {
            get {
                _lock.Lock();
                try {
                    return _queue.Count;
                }
                finally {
                    _lock.Unlock();
                }
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return new LockingEnumerator<T>(_queue.GetEnumerator(), _lock);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
