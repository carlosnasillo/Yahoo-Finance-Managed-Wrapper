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


namespace MaasOne.RSS
{
    public partial class FeedDownload : Base.DownloadClient<FeedResult>
    {

        public Base.DefaultResponse<FeedResult> Download(string url)
        {
            if (url == string.Empty)
                throw new ArgumentNullException("url", "The url is empty.");
            return this.Download(new Uri(url));
        }
        public Base.DefaultResponse<FeedResult> Download(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url", "The url is null.");
            return this.Download(new Uri[] { url });
        }
        public Base.DefaultResponse<FeedResult> Download(IEnumerable<string> urls)
        {
            if (urls == null)
                throw new ArgumentNullException("urls", "The list is null.");
            List<Uri> lst = new List<Uri>();
            foreach (string url in urls)
            {
                lst.Add(new Uri(url));
            }
            return this.Download(lst);
        }
        public Base.DefaultResponse<FeedResult> Download(IEnumerable<Uri> urls)
        {
            if (urls == null)
                throw new ArgumentNullException("urls", "The list is null.");
            return (Base.DefaultResponse<FeedResult>)base.Download(new FeedDownloadSettings(urls));
        }

    }
}
