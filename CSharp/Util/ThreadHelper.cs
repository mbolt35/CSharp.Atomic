////////////////////////////////////////////////////////////////////////////////
//
//  MATTBOLT.BLOGSPOT.COM
//  Copyright(C) 2013 Matt Bolt
//
//  Permission is hereby granted, free of charge, to any person obtaining a 
//  copy of this software and associated documentation files (the "Software"), 
//  to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//  and/or sell copies of the Software, and to permit persons to whom the 
//  Software is furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included 
//  in all copies or substantial portions of the Software
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
//  THE SOFTWARE.
//
////////////////////////////////////////////////////////////////////////////////

namespace CSharp.Util {
    using System;
    using System.Threading;


    /// <summary>
    /// This static class provides helper methods for working with <c>System.Threading</c>.
    /// </summary>
    /// \author Matt Bolt
    public static class ThreadHelper {

        /// <summary>
        /// This method returns the current <c>Thread</c> instance's <c>ManagedThreadId</c> as a 
        /// <c>string</c>.
        /// </summary>
        /// <value>The current thread's managed thread id.</value>
        public static string CurrentThreadId {
            get {
                return Thread.CurrentThread.ManagedThreadId.ToString();
            }
        }

    }
}

