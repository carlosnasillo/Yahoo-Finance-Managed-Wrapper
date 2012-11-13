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
    public abstract partial class DownloadClient<T>
    {


        private IWebProxy mProxy = WebProxy.GetDefaultProxy();
        /// <summary>
        /// Gets or sets the used proxy setting for download.
        /// </summary>
        /// <value></value>
        /// <returns>The actual setted proxy</returns>
        /// <remarks>Default value are proxy settings from Internet Explorer/Windows Internet Settings.</remarks>
        public IWebProxy Proxy
        {
            get { return mProxy; }
            set { mProxy = value; }
        }


        public IResponse GetResponse()
        {
            return this.Download();
        }
        public virtual Response<T> Download()
        {
            return this.Download((SettingsBase)this.Settings.Clone());
        }
        protected Response<T> Download(SettingsBase settings)
        {
            if (settings == null) { throw new ArgumentNullException("Settings", "The settings for downloading with " + this.GetType().Name + " are null."); }
            using (TimeoutWebClient<T> wc = new TimeoutWebClient<T>(mTimeout))
            {
                if (mProxy != null)
                    wc.Proxy = mProxy;
                Response<System.IO.Stream> sr = wc.Download(settings);
                Response<T> result = this.ConvertResponse(new DefaultResponse<T>(sr.Connection, this.ConvertResult(sr.Connection, sr.Result, settings)));
                if (sr.Result != null) { sr.Result.Dispose(); }
                return result;
            }
        }

        private void SetProxy(TimeoutWebClient<T> wc)
        {
            if (mProxy != null)
                wc.Proxy = mProxy;
        }


    }



    public abstract partial class SettingsBase
    {
        protected virtual bool KeepAlive { get { return true; } }
        internal bool KeepAliveInternal { get { return this.KeepAlive; } }
    }
}
