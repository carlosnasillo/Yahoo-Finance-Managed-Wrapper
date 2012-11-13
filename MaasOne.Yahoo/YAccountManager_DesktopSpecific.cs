// ******************************************************************************
// ** 
// **  Yahoo! Managed
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
using MaasOne.Base;
using MaasOne.Xml;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;
using MaasOne.Finance.YahooFinance.Support;

namespace MaasOne
{
    public partial class YAccountManager
    {
        public bool LogIn(System.Net.NetworkCredential user)
        {
            if (!this.IsLoggedIn)
            {
                if (user == null) throw new ArgumentNullException("User credential is null.");
                mCookies = new CookieContainer();
                WebFormUpload upl = new WebFormUpload();
                upl.Upload(this.GetLoginDownloadSettings(user));
                if (!this.IsLoggedIn) mCookies = null; 
            }
            return this.IsLoggedIn;
        }

        /// <summary>
        /// Logging out.
        /// </summary>
        /// <returns>The Logging</returns>
        public bool LogOut()
        {
            if (this.IsLoggedIn)
            {
                Html2XmlDownload dl = new Html2XmlDownload();
                dl.Settings.Account = this;
                dl.Settings.DownloadStream = false;
                dl.Settings.Url = "http://login.yahoo.com/config/login?logout=1&.direct=2&.done=&.src=&.intl=us&.lang=en-US";
                dl.AsyncDownloadCompleted += this.LogOutAsync_Completed;
                Response<XDocument> resp = dl.Download();
                if (resp.Connection.State == ConnectionState.Success)
                {
                    mCookies = null;
                    this.SetCrumb(string.Empty);
                    if (this.PropertyChanged != null) this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("IsLoggedIn"));
                }               
            }
            return this.IsLoggedIn;
        }


    }
}
