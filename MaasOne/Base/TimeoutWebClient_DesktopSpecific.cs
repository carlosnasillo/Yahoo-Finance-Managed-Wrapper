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
    internal partial class TimeoutWebClient<T>
    {


        private IWebProxy mProxy = WebRequest.DefaultWebProxy;
        public IWebProxy Proxy
        {
            get { return mProxy; }
            set { mProxy = value; }
        }


        public TimeoutWebClient(int timeout)
        {
            this.Timeout = timeout;
        }
        private void AddHeaders(HttpWebRequest wr, List<KeyValuePair<HttpRequestHeader, string>> headers)
        {
            foreach (KeyValuePair<HttpRequestHeader, String> header in headers)
            {
                wr.Headers.Add(header.Key, header.Value);
            }
        }

        public Response<System.IO.Stream> Download(SettingsBase userSettings)
        {
            if (!mDisposedValue)
            {

                StreamDownloadSettings<T> ss = new StreamDownloadSettings<T>(userSettings);
                DateTime startTime = System.DateTime.Now;
                HttpWebRequest wr = this.GetWebRequest(ss);
                byte[] postDataBytes = null;
                if (userSettings.PostDataInternal != string.Empty)
                {
                    postDataBytes = System.Text.Encoding.ASCII.GetBytes(userSettings.PostDataInternal);
                    wr.ContentLength = postDataBytes.Length;
                }
                mActualDownload = wr;

                System.IO.MemoryStream memStream = null;
                System.Net.WebException dlException = null;
                int size = 0;
                List<KeyValuePair<HttpResponseHeader, string>> headers = new List<KeyValuePair<HttpResponseHeader, string>>();                               
                DateTime endTime = System.DateTime.Now;

                try
                {
                    if (postDataBytes != null)
                    {
                        using (System.IO.Stream s = wr.GetRequestStream())
                        {
                            s.Write(postDataBytes, 0, postDataBytes.Length);
                        }
                    }

                    using (HttpWebResponse resp = (HttpWebResponse)wr.GetResponse())
                    {
                        foreach (var header in resp.Headers.Keys)
                        {
                            headers.Add(new KeyValuePair<HttpResponseHeader, string>());
                        }
                        if (userSettings.DownloadResponseStreamInternal)
                        {
                            System.IO.Stream s = resp.GetResponseStream();
                            endTime = System.DateTime.Now;
                            memStream = MyHelper.CopyStream(s);
                            s.Dispose();
                        }
                    }

                    if (memStream != null && memStream.CanSeek)
                        int.TryParse(memStream.Length.ToString(), out size);
                }
                catch (Exception ex)
                {
                    dlException = this.GetOrCreateWebException(ex, null);
                }
                finally
                {
                    mActualDownload = null;
                }


                return new DefaultResponse<System.IO.Stream>(new ConnectionInfo(dlException, this.Timeout, size, startTime, endTime, headers.ToArray()), memStream);

            }
            else
            {
                return null;
            }
        }


        private void SetProxyAndTimeout(HttpWebRequest wr, SettingsBase s)
        {
            if (mProxy != null)
                wr.Proxy = mProxy;
            wr.Timeout = this.Timeout;
            wr.KeepAlive = s.KeepAliveInternal;
        }
        private byte[] StringToAscii(string s)
        {
            return System.Text.Encoding.ASCII.GetBytes(s);
        }
        public static WebExceptionStatus GetTimeoutStatus()
        {
            return WebExceptionStatus.Timeout;
        }
    }
}
