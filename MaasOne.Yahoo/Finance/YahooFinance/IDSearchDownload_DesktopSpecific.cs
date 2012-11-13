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


namespace MaasOne.Finance.YahooFinance
{
    public partial class IDSearchDownload
    {

        /// <summary>
        /// Downloads search results for Yahoo IDs by keyword and other options
        /// </summary>
        /// <param name="text">The used search text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<IDSearchResult> Download(string text)
        {
            if (text.Trim() == string.Empty)
                throw new ArgumentNullException("text", "The text is empty.");
            IDSearchBaseSettings<IDSearchResult> settings = null;
            if (this.Settings != null && this.Settings is IQuerySettings)
            {
                settings = (IDSearchBaseSettings<IDSearchResult>)this.Settings.Clone();
            }
            else
            {
                settings = new IDInstantSearchDownloadSettings();
            }
            ((IQuerySettings)settings).Query = text;
            return this.Download(settings);
        }
        public Base.Response<IDSearchResult> Download(AlphabeticalIndex index)
        {
            return this.Download(new IDAlphabeticSearchDownloadSettings() { Index = index });
        }
        public Base.Response<IDSearchResult> Download(IDSearchBaseSettings<IDSearchResult> settings)
        {
            return base.Download(settings);
        }



    }
}
