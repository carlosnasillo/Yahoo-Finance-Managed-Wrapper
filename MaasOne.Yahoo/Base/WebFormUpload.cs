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
using MaasOne.Base;
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Base
{
    internal partial class WebFormUpload
    {

        public event AsyncDownloadCompletedEventHandler AsyncUploadCompleted;
        public delegate void AsyncDownloadCompletedEventHandler(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e);

        public WebFormDownloadSettings Settings { get; set; }

        public WebFormUpload()
        {
            this.Settings = new WebFormDownloadSettings();
        }

        public void UploadAsync(WebFormDownloadSettings settings, object userArgs)
        {
            AsyncArgs args = new AsyncArgs(userArgs) { Settings = settings };
            if (settings.Account.Crumb == string.Empty)
            {
                Html2XmlDownload html = new Html2XmlDownload();
                html.Settings.Account = settings.Account;
                html.Settings.Url = settings.Url;
                html.AsyncDownloadCompleted += this.html_DownloadAsyncCompleted;
                html.DownloadAsync(args);
            }
            else
            {
                this.UploadAsync2(args);
            }
        }


        private void html_DownloadAsyncCompleted(DownloadClient<XDocument> sender, DownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncDownloadCompleted -= this.html_DownloadAsyncCompleted;
            AsyncArgs args = (AsyncArgs)e.UserArgs;
            this.ConvertHtml(e.Response.Result, args);
            this.UploadAsync2(args);
        }

        private void UploadAsync2(AsyncArgs args)
        {
            PostDataUpload dl = new PostDataUpload();
            this.PrepareDownloader(dl, args);
            if (dl.Settings.PostStringData != string.Empty)
            {
                dl.AsyncDownloadCompleted += this.dl_DownloadAsyncCompleted;
                dl.DownloadAsync(args.UserArgs);
            }
        }

        private void dl_DownloadAsyncCompleted(DownloadClient<System.IO.Stream> sender, DownloadCompletedEventArgs<System.IO.Stream> e)
        {
            sender.AsyncDownloadCompleted -= this.dl_DownloadAsyncCompleted;
            XDocument doc = null;
            if (e.Response.Result != null)
            {
                doc = MyHelper.ParseXmlDocument(e.Response.Result);
            }
            if (this.AsyncUploadCompleted != null) this.AsyncUploadCompleted(this, ((DefaultDownloadCompletedEventArgs<System.IO.Stream>)e).CreateNew(doc));
        }

        private void ConvertHtml(XDocument doc, AsyncArgs args)
        {
            string fap = string.Empty;
            if (args.Settings.FormActionPattern != string.Empty)
            {
                fap = "//form[@" + args.Settings.FormActionPattern + "]";
            }
            else
            {
                fap = "//form[@action=\"" + args.Settings.RefererUrlPart + "\"]";
            }
            XElement formNode = new XPath(fap, true).GetElement(doc);
            if (formNode != null)
            {
                XElement[] inputNodes = XPath.GetElements("//input", formNode);
                foreach (XElement inp in inputNodes)
                {
                    XAttribute nameAtt = inp.Attribute(XName.Get("name"));
                    if (nameAtt != null)
                    {
                        string value = string.Empty;
                        XAttribute valueAtt = inp.Attribute(XName.Get("value"));
                        if (valueAtt != null && valueAtt.Value != string.Empty)
                        {
                            value = valueAtt.Value;
                        }

                        if (nameAtt.Value.Contains("challenge"))
                        {
                            args.IsLoginChallenge = true;
                        }

                        if (nameAtt.Value.Contains("crumb"))
                        {
                            args.Settings.Account.SetCrumb(valueAtt.Value);
                        }
                        else
                        {
                            if (args.Settings.SearchForWebForms != null)
                            {
                                foreach (string name in args.Settings.SearchForWebForms)
                                {
                                    if (name == nameAtt.Value)
                                    {
                                        if (args.Settings.AdditionalWebForms != null && args.Settings.AdditionalWebForms.Count > 0)
                                        {
                                            for (int i = 0; i < args.Settings.AdditionalWebForms.Count; i++)
                                            {
                                                if (args.Settings.AdditionalWebForms[i].Key == nameAtt.Value)
                                                {
                                                    args.Settings.AdditionalWebForms[i] = new KeyValuePair<string, string>(nameAtt.Value, valueAtt.Value);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PrepareDownloader(PostDataUpload dl, AsyncArgs args)
        {
            if (args.Settings.Account.Crumb != string.Empty || args.IsLoginChallenge)
            {
                if (args.Settings.DownloadResponse) dl.Settings.DownloadResponse = true;
                bool setCrumb = true;
                if (args.Settings.AdditionalWebForms.Count > 0 && !args.IsLoginChallenge)
                {
                    for (int i = 0; i < args.Settings.AdditionalWebForms.Count; i++)
                    {
                        if (args.Settings.AdditionalWebForms[i].Key == ".yficrumb")
                        {
                            args.Settings.AdditionalWebForms[i] = new KeyValuePair<string, string>(".yficrumb", args.Settings.Account.Crumb);
                            setCrumb = false;
                            break;
                        }
                    }
                }
                if (args.IsLoginChallenge) setCrumb = false;

                if (setCrumb) args.Settings.AdditionalWebForms.Insert(0, new KeyValuePair<string, string>(".yficrumb", args.Settings.Account.Crumb));
                if (args.Settings.RefererUrlPart.StartsWith("http"))
                {
                    args.Settings.Url = args.Settings.RefererUrlPart;
                }
                else
                {
                    args.Settings.Url = "http://" + new Uri(args.Settings.Url).Host + args.Settings.RefererUrlPart;
                }

                dl.Settings.UrlString = args.Settings.Url;
                dl.Settings.Account = args.Settings.Account;
                StringBuilder postData = new StringBuilder();
                bool isFirst = true;
                foreach (var kvp in args.Settings.AdditionalWebForms)
                {
                    string data = Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value);
                    if (isFirst) { isFirst = false; }
                    else { data = "&" + data; }
                    postData.Append(data);
                }
                dl.Settings.PostStringData = postData.ToString();
            }
        }





        private class AsyncArgs : DownloadEventArgs
        {
            public bool IsLoginChallenge { get; set; }
            public WebFormDownloadSettings Settings { get; set; }
            public AsyncArgs(object userArgs)
                : base(userArgs)
            {
            }
        }


    }


    internal class WebFormDownloadSettings
    {
        public YAccountManager Account { get; set; }
        public string Url { get; set; }
        public string RefererUrlPart { get; set; }
        public List<KeyValuePair<string, string>> AdditionalWebForms { get; set; }
        public string[] SearchForWebForms { get; set; }
        public string FormActionPattern { get; set; }
        public bool DownloadResponse { get; set; }
    }

}


