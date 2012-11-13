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


namespace MaasOne.Search
{

   
    public abstract class SearchBaseSettings<T> : Base.SettingsBase, IQuerySettings where T : SearchBaseResult
    {

        public string Query { get; set; }
        public SearchBaseSettings()
        {
            this.Query = string.Empty;
        }

    }



    public abstract class SearchBaseResult
    {

        private SearchDataBaseContainer[] mContainers = null;
        public SearchDataBaseContainer[] Containers { get { return mContainers; } }

        protected SearchBaseResult(SearchDataBaseContainer[] containers)
        {
            mContainers = containers;
        }

    }


    public abstract class SearchDataBaseContainer : System.Collections.IEnumerable
    {

        private int mStart;
        private int mCount;
        private long mTotalResults;
        private SearchBaseData[] mItems;

        public int Start
        {
            get { return mStart; }
        }
        public int Count
        {
            get { return mCount; }
        }
        public long TotalResults
        {
            get { return mTotalResults; }
        }
        public SearchBaseData[] Items
        {
            get { return mItems; }
        }
        public SearchBaseData this[int index] { get { return mItems[index]; } }

        protected SearchDataBaseContainer(SearchBaseData[] items, int start, int count, long total)
        {
            mItems = items;
            mStart = start;
            mCount = count;
            mTotalResults = total;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return mItems.GetEnumerator();
        }
    }


    public abstract class SearchBaseData
    {
        private string mTitle;
        /// <summary>
        /// Returns the Title string for this WebRequest.
        /// </summary>
        /// <remarks>
        /// The Title field contains the text specified in the HTML TITLE tag of the page.
        /// </remarks>
        public string Title { get { return mTitle; } }
        private string mDescription;
        /// <summary>
        /// Returns the description text of the result.
        /// </summary>
        /// <remarks>
        /// This string contains a portion of the text from the body of the HTML page that contains the specified search terms.
        /// </remarks>
        public string Description { get { return mDescription; } }
        private Uri mUrl;
        /// <summary>
        /// Returns the full URL for the result as a string.
        /// </summary>
        /// <remarks>
        /// The Url field contains the full URL to the page. This may contain extended information, such as instrumentation, and may be different than the DisplayUrl that you will want to display to the user. Typically, the Title is used to display the text of a hyperlink and the Url is used to specify the link target. 
        /// This string contains the URL to the Web page.
        /// </remarks>
        public Uri Url { get { return mUrl; } }

        protected SearchBaseData(string title, string description, Uri url)
        {
            mTitle = title;
            mDescription = description;
            mUrl = url;
        }

    }

}
