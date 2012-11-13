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
using System.Net;


namespace MaasOne.Base
{

    public abstract partial class SettingsBase : ICloneable
    {

        protected abstract string GetUrl();
        public abstract object Clone();

        private List<KeyValuePair<HttpRequestHeader, string>> mAdditionalHeaders = new List<KeyValuePair<HttpRequestHeader, string>>();

        protected virtual List<KeyValuePair<HttpRequestHeader, string>> AdditionalHeaders
        {
            get
            {
                return mAdditionalHeaders;
            }
        }
        protected virtual RequestMethod Method { get { return RequestMethod.GET; } }
        protected virtual CookieContainer Cookies { get { return null; } }
        protected virtual string ContentType { get { return string.Empty; } }
        protected virtual string PostData { get { return string.Empty; } }
        protected virtual bool DownloadResponseStream { get { return true; } }

        internal string GetUrlInternal()
        {
            return this.GetUrl();
        }
        internal List<KeyValuePair<HttpRequestHeader, string>> GetAdditionalHeadersInternal { get { return mAdditionalHeaders; } }
        internal RequestMethod MethodInternal { get { return this.Method; } }
        internal CookieContainer CookiesInternal { get { return this.Cookies; } }
        internal string ContentTypeInternal { get { return this.ContentType; } }
        internal string PostDataInternal { get { return this.PostData; } }
        internal bool DownloadResponseStreamInternal { get { return this.DownloadResponseStream; } }
    }

}
