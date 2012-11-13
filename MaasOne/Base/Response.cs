// ******************************************************************************
// ** 
// **  MaasOne WebServices
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;


namespace MaasOne.Base
{
    /// <summary>
    /// Base response class for download processes. This class must be inherited.
    /// </summary>
    /// <remarks></remarks>
    public abstract class Response<T> : IResponse
    {
        private ConnectionInfo mConnection = null;
        private T mResult;

        /// <summary>
        /// Gets connection information of the download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConnectionInfo Connection { get { return mConnection; } }
        /// <summary>
        /// Gets the received managed data.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public T Result { get { return mResult; } }
        public object GetObjectResult() { return this.Result; }

        protected Response(ConnectionInfo connInfo, T result)
        {
            mConnection = connInfo;
            mResult = result;
        }



     

    }
}
