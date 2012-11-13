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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using MaasOne.Xml;


namespace MaasOne.Search.BOSS
{


    public class SearchResult : SearchBaseResult
    {
        private SearchDownloadSettings mSettings = null;
        public SearchDownloadSettings Settings { get { return mSettings; } }
        public SearchDataContainer[] Containers { get { return (SearchDataContainer[])base.Containers; } }

        internal SearchResult(SearchDataContainer[] items, SearchDownloadSettings settings)
            : base(items)
        {
            mSettings = settings;
        }
    }


    public abstract class SearchDataContainer : SearchDataBaseContainer
    {

        private SearchResultType mType;
        public SearchResultType Type
        {
            get { return mType; }
        }
        public SearchData[] Items { get { return (SearchData[])base.Items; ; } }

        protected SearchDataContainer(SearchData[] results, SearchResultType type, int start, int count, long total)
            : base(results, start, count, total)
        {
            mType = type;
        }

    }



    public class WebSearchDataContainer : SearchDataContainer
    {

        public WebSearchData[] Items
        {
            get { return (WebSearchData[])base.Items; }
        }

        internal WebSearchDataContainer(WebSearchData[] results, int start, int count, long total)
            : base(results, SearchResultType.Web, start, count, total)
        {
        }

    }



    public class ImageSearchDataContainer : SearchDataContainer
    {

        public ImageSearchData[] Items
        {
            get { return (ImageSearchData[])base.Items; }
        }

        internal ImageSearchDataContainer(ImageSearchData[] results, int start, int count, long total)
            : base(results, SearchResultType.Images, start, count, total)
        {
        }

    }



    public class NewsSearchDataContainer : SearchDataContainer
    {

        public NewsSearchResult[] Items
        {
            get { return (NewsSearchResult[])base.Items; }
        }

        internal NewsSearchDataContainer(NewsSearchResult[] results, int start, int count, long total)
            : base(results, SearchResultType.News, start, count, total)
        {
        }

    }

    public class SpellingSearchDataContainer : SearchDataContainer
    {

        public SpellingSearchData[] Items
        {
            get { return (SpellingSearchData[])base.Items; }
        }

        internal SpellingSearchDataContainer(SpellingSearchData[] results, int start, int count, long total)
            : base(results, SearchResultType.Spelling, start, count, total)
        {
        }

    }



}
