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
using System.IO;
using MaasOne.Base;
using MaasOne.Xml;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;
using MaasOne.Finance.YahooFinance.Support;
using System.Xml.Linq;


namespace MaasOne
{
    public partial class YAccountManager : MaasOne.IAccountManager, System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event AsyncIAccountManagerDownloadCompletedEventHandler LoggedStatusChanged;

        private CookieContainer mCookies = null;
        private string mCrumb = string.Empty;

        public CookieContainer Cookies
        {
            get
            {
                return mCookies;
            }
            set
            {
                bool islog = (mCookies != value && value != null);
                mCookies = value;
                if (islog && this.LoggedStatusChanged != null) this.LoggedStatusChanged(this, new LoginStateEventArgs(this.IsLoggedIn, null));
                this.OnPropertyChanged("Cookies");
                this.OnPropertyChanged("IsLoggedIn");
            }
        }
        public bool IsLoggedIn { get { return this.IsLoggedInFunc(mCookies); } }
        public string Crumb
        {
            get
            {
                return mCrumb;
            }
            set
            {
                mCrumb = value;
                this.OnPropertyChanged("Crumb");
            }
        }
        public void SetCrumb(string value)
        {
            mCrumb = value;
        }

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }


        public void LogInAsync(System.Net.NetworkCredential user, object userArgs)
        {
            if (!this.IsLoggedIn)
            {
                if (user == null) throw new ArgumentNullException("User credential is null.");
                mCookies = new CookieContainer();
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.logInDl_Completed;
                upl.UploadAsync(this.GetLoginDownloadSettings(user), userArgs);
            }
        }
        private WebFormDownloadSettings GetLoginDownloadSettings(System.Net.NetworkCredential user)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".tries", "1"));
            lst.Add(new KeyValuePair<string, string>(".src", "/"));
            lst.Add(new KeyValuePair<string, string>(".md5", ""));
            lst.Add(new KeyValuePair<string, string>(".hash", ""));
            lst.Add(new KeyValuePair<string, string>(".js", ""));
            lst.Add(new KeyValuePair<string, string>(".last", ""));
            lst.Add(new KeyValuePair<string, string>("promo", ""));
            lst.Add(new KeyValuePair<string, string>(".intl", "en"));
            lst.Add(new KeyValuePair<string, string>(".lang", "en-US"));
            lst.Add(new KeyValuePair<string, string>(".bypass", ""));
            lst.Add(new KeyValuePair<string, string>(".partner", ""));
            lst.Add(new KeyValuePair<string, string>(".u", ""));
            lst.Add(new KeyValuePair<string, string>(".v", ""));
            lst.Add(new KeyValuePair<string, string>(".challenge", ""));
            lst.Add(new KeyValuePair<string, string>(".yplus", ""));
            lst.Add(new KeyValuePair<string, string>(".emailCode", ""));
            lst.Add(new KeyValuePair<string, string>("pkg", ""));
            lst.Add(new KeyValuePair<string, string>("stepid", ""));
            lst.Add(new KeyValuePair<string, string>(".ev", ""));
            lst.Add(new KeyValuePair<string, string>("hasMsgr", "0"));
            lst.Add(new KeyValuePair<string, string>(".chkP", "Y"));
            lst.Add(new KeyValuePair<string, string>(".done", ""));
            lst.Add(new KeyValuePair<string, string>(".pd", ""));
            lst.Add(new KeyValuePair<string, string>(".ws", ""));
            lst.Add(new KeyValuePair<string, string>(".cp", ""));
            lst.Add(new KeyValuePair<string, string>("pad", ""));
            lst.Add(new KeyValuePair<string, string>("aad", ""));
            lst.Add(new KeyValuePair<string, string>("login", user.UserName));
            lst.Add(new KeyValuePair<string, string>("passwd", user.Password));
            lst.Add(new KeyValuePair<string, string>(".persistent", "y"));
            lst.Add(new KeyValuePair<string, string>(".save", ""));
            lst.Add(new KeyValuePair<string, string>("passwd_raw", ""));

            List<string> lstSearchFor = new List<string>();
            lstSearchFor.Add(".u");
            lstSearchFor.Add(".v");
            lstSearchFor.Add(".challenge");
            lstSearchFor.Add(".pd");
            lstSearchFor.Add("pad");
            lstSearchFor.Add("aad");
            lstSearchFor.Add(".ws");
            lstSearchFor.Add(".cp");

            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "https://login.yahoo.com/config/login";
            settings.RefererUrlPart = "https://login.yahoo.com/config/login";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = lstSearchFor.ToArray();
            settings.FormActionPattern = "action=\"https://login.yahoo.com/config/login.*?\"";
            return settings;
        }
        private void logInDl_Completed(WebFormUpload sender, DownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.logInDl_Completed;
            if (this.IsLoggedInFunc(mCookies)) { if (this.LoggedStatusChanged != null) this.LoggedStatusChanged(this, new LoginStateEventArgs(this.IsLoggedIn, e.UserArgs)); }
            else { mCookies = null; }
            if (this.PropertyChanged != null) this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("IsLoggedIn"));
        }
        private bool IsLoggedInFunc(CookieContainer cookies)
        {
            bool foundY = false;
            bool foundF = false;
            if (cookies != null)
            {
                foreach (Cookie c in cookies.GetCookies(new Uri("http://yahoo.com")))
                {
                    switch (c.Name)
                    {
                        case "Y":
                            foundY = true;
                            break;
                        case "F":
                            foundF = true;
                            break;
                    }
                }
            }
            return foundY & foundF;
        }


        public void LogOutAsync(object userArgs)
        {
            if (this.IsLoggedIn)
            {
                Html2XmlDownload dl = new Html2XmlDownload();
                dl.Settings.Account = this;
                dl.Settings.DownloadStream = false;
                dl.Settings.Url = "http://login.yahoo.com/config/login?logout=1&.direct=2&.done=&.src=&.intl=us&.lang=en-US";
                dl.AsyncDownloadCompleted += this.LogOutAsync_Completed;
                dl.DownloadAsync(userArgs);
            }
        }
        private void LogOutAsync_Completed(DownloadClient<XDocument> sender, DownloadCompletedEventArgs<XDocument> e)
        {
            if (e.Response.Connection.State == ConnectionState.Success)
            {
                mCookies = null;
                this.SetCrumb(string.Empty);
                if (this.PropertyChanged != null) this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("IsLoggedIn"));
                if (this.LoggedStatusChanged != null) this.LoggedStatusChanged(this, new LoginStateEventArgs(this.IsLoggedIn, e.UserArgs));
            }
        }

    }
}
