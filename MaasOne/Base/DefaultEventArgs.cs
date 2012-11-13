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

    public class DefaultDownloadCompletedEventArgs<T> : DownloadCompletedEventArgs<T>
    {
        internal DefaultDownloadCompletedEventArgs(object userArgs, Response<T> response, SettingsBase settings)
            : base(userArgs, response, settings)
        {
        }

        public DefaultDownloadCompletedEventArgs<N> CreateNew<N>(N newResult)
        {
            return this.CreateNew<N>(newResult, this.UserArgs);
        }
        public DefaultDownloadCompletedEventArgs<N> CreateNew<N>(N newResult, object userArgs)
        {
            return new DefaultDownloadCompletedEventArgs<N>(userArgs, new DefaultResponse<N>(this.Response.Connection, newResult), this.Settings);
        }
    }

    public class DefaultResponse<T> : Response<T>
    {
        internal DefaultResponse(ConnectionInfo info, T result)
            : base(info, result)
        {
        }
        public DefaultResponse<N> CreateNew<N>(N newResult)
        {
            return new DefaultResponse<N>(this.Connection, newResult);
        }
    }


}
