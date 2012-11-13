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
using System.Xml.Linq;


namespace MaasOne.Search.BOSS
{


    public partial class SearchDownload : Base.DownloadClient<SearchResult>
    {

        public SearchDownloadSettings Settings { get { return (SearchDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        public SearchDownload()
        {
            this.Settings = new SearchDownloadSettings();
        }

        public void DownloadAsync(SearchService service, object userArgs)
        {
            this.DownloadAsync(new SearchService[] { service }, userArgs);
        }
        public void DownloadAsync(IEnumerable<SearchService> services, object userArgs)
        {
            SearchDownloadSettings set = (SearchDownloadSettings)this.Settings.Clone();
            set.Services.Clear();
            foreach (SearchService service in services)
            {
                set.Services.Add(service);
            }
            this.DownloadAsync(set, userArgs);
        }
        public void DownloadAsync(SearchDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }



        protected override SearchResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<SearchDataContainer> containers = new List<SearchDataContainer>();
            List<SearchData> lst = new List<SearchData>();

            XDocument xmlDoc = MyHelper.ParseXmlDocument(stream);
            if (xmlDoc != null)
            {
                XElement bossResponseNode = XPath.GetElement("bossresponse",xmlDoc);
                if (bossResponseNode != null)
                {
                    int respCode = Convert.ToInt32(MyHelper.GetXmlAttributeValue(bossResponseNode, "responsecode"));

                    if (respCode == 200)
                    {
                        foreach (XElement containerNode in bossResponseNode.Elements())
                        {
                            List<SearchData> results = new List<SearchData>();
                            int start = Convert.ToInt32(MyHelper.GetXmlAttributeValue(containerNode, "start"));
                            int count = Convert.ToInt32(MyHelper.GetXmlAttributeValue(containerNode, "count"));
                            long totalResults = Convert.ToInt64(MyHelper.GetXmlAttributeValue(containerNode, "totalresults"));

                            XElement resultsNode = MyHelper.EnumToArray(containerNode.Elements())[0];
                            if (resultsNode.Name.LocalName == "results")
                            {
                                foreach (XElement resultNode in resultsNode.Elements())
                                {
                                    if (resultNode.Name.LocalName == "result")
                                    {
                                        SearchData res = null;
                                        switch (containerNode.Name.LocalName)
                                        {
                                            case "web":
                                            case "limitedweb":
                                                res = this.ToBossWebSearchResult(resultNode);
                                                break;
                                            case "images":
                                                res = this.ToBossImageSearchResult(resultNode);
                                                break;
                                            case "news":
                                                res = this.ToBossNewsSearchResult(resultNode);
                                                break;
                                            case "spelling":
                                                res = this.ToBossSpellingSearchResult(resultNode);
                                                break;
                                        }
                                        if (res != null)
                                            results.Add(res);
                                    }
                                }
                            }

                            switch (containerNode.Name.LocalName)
                            {
                                case "web":
                                case "limitedweb":
                                    List<WebSearchData> webResults = new List<WebSearchData>();
                                    foreach (SearchData res in results)
                                    {
                                        if (res is WebSearchData)
                                        {
                                            webResults.Add((WebSearchData)res);
                                        }
                                    }

                                    containers.Add(new WebSearchDataContainer(webResults.ToArray(), start, count, totalResults));
                                    break;
                                case "images":
                                    List<ImageSearchData> imgResults = new List<ImageSearchData>();
                                    foreach (SearchData res in results)
                                    {
                                        if (res is ImageSearchData)
                                        {
                                            imgResults.Add((ImageSearchData)res);
                                        }
                                    }

                                    containers.Add(new ImageSearchDataContainer(imgResults.ToArray(), start, count, totalResults));
                                    break;
                                case "news":
                                    List<NewsSearchResult> newsResults = new List<NewsSearchResult>();
                                    foreach (SearchData res in results)
                                    {
                                        if (res is NewsSearchResult)
                                        {
                                            newsResults.Add((NewsSearchResult)res);
                                        }
                                    }

                                    containers.Add(new NewsSearchDataContainer(newsResults.ToArray(), start, count, totalResults));
                                    break;
                                case "spelling":
                                    List<SpellingSearchData> splResults = new List<SpellingSearchData>();
                                    foreach (SearchData res in results)
                                    {
                                        if (res is SpellingSearchData)
                                        {
                                            splResults.Add((SpellingSearchData)res);
                                        }
                                    }

                                    containers.Add(new SpellingSearchDataContainer(splResults.ToArray(), start, count, totalResults));
                                    break;
                            }

                        }
                    }
                }
            }


            return new SearchResult(containers.ToArray(), (SearchDownloadSettings)settings);
        }


        private SearchData ToSearchResult(XElement node)
        {
            if (node != null && node.Name.LocalName.ToLower() == "result")
            {
                string title = string.Empty;
                string @abstract = string.Empty;
                Uri url = null;
                Uri clickUrl = null;

                foreach (XElement prpNode in node.Elements())
                {
                    switch (prpNode.Name.LocalName.ToLower())
                    {
                        case "title":
                            title = prpNode.Value;
                            break;
                        case "abstract":
                            @abstract = prpNode.Value;
                            break;
                        case "url":
                            if (prpNode.Value.Trim() != string.Empty)
                                url = new Uri(prpNode.Value);
                            break;
                        case "clickurl":
                            if (prpNode.Value.Trim() != string.Empty)
                                clickUrl = new Uri(prpNode.Value);
                            break;
                    }
                }
                return new SearchData(title, @abstract, url, clickUrl);
            }
            else
            {
                return null;
            }
        }

        private WebSearchData ToBossWebSearchResult(XElement node)
        {
            SearchData result = this.ToSearchResult(node);
            if (result != null)
            {
                string dispUrl = string.Empty;
                System.DateTime crwDate = default(System.DateTime);
                Language language = Language.en;
                string smFeed = string.Empty;

                foreach (XElement prpNode in node.Elements())
                {
                    switch (prpNode.Name.LocalName)
                    {
                        case "dispurl":
                            dispUrl = prpNode.Value;
                            break;
                        case "date":
                            System.DateTime.TryParse(prpNode.Value, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.AssumeUniversal, out crwDate);
                            break;
                        case "language":
                            language = this.StringToLanguage(prpNode.Value);
                            break;
                        case "smfeed":
                            smFeed = prpNode.ToString();
                            break;
                    }
                }
                return new WebSearchData(result, dispUrl, crwDate, language, smFeed);
            }
            else
            {
                return null;
            }
        }

        private ImageSearchData ToBossImageSearchResult(XElement node)
        {

            SearchData result = this.ToSearchResult(node);
            if (result != null)
            {
                Uri refererUrl = null;
                Uri refererClickUrl = null;
                Uri tmbUrl = null;
                ImageFileType fileFormat = default(ImageFileType);
                long size = 0;
                int height = 0;
                int width = 0;
                int tmbHeight = 0;
                int tmbWidth = 0;
                System.Globalization.CultureInfo convCult = new System.Globalization.CultureInfo("en-US");


                foreach (XElement prpNode in node.Elements())
                {
                    switch (prpNode.Name.LocalName)
                    {
                        case "refererurl":
                            refererUrl = new Uri(prpNode.Value);
                            break;
                        case "refererclickurl":
                            refererClickUrl = new Uri(prpNode.Value);
                            break;
                        case "size":
                            double srcSize = 0;
                            if (prpNode.Value.EndsWith("Bytes"))
                            {
                                double.TryParse(prpNode.Value.Replace("Bytes", ""), System.Globalization.NumberStyles.Any, convCult, out srcSize);
                            }
                            else if (prpNode.Value.EndsWith("KB"))
                            {
                                double.TryParse(prpNode.Value.Replace("KB", ""), System.Globalization.NumberStyles.Any, convCult, out srcSize);
                                srcSize *= 1024;
                            }
                            else if (prpNode.Value.EndsWith("MB"))
                            {
                                double.TryParse(prpNode.Value.Replace("MB", ""), System.Globalization.NumberStyles.Any, convCult, out srcSize);
                                srcSize *= Math.Pow(1024, 2);
                            }
                            size = Convert.ToInt64(srcSize);
                            break;
                        case "format":
                            switch (prpNode.Value.ToLower())
                            {
                                case "bmp":
                                    fileFormat = ImageFileType.Bmp;
                                    break;
                                case "gif":
                                    fileFormat = ImageFileType.Gif;
                                    break;
                                case "jpg":
                                    fileFormat = ImageFileType.Jpeg;
                                    break;
                                case "jpeg":
                                    fileFormat = ImageFileType.Jpeg;
                                    break;
                                case "png":
                                    fileFormat = ImageFileType.Png;
                                    break;
                                default:
                                    fileFormat = ImageFileType.Any;
                                    break;
                            }
                            break;
                        case "height":
                            int.TryParse(prpNode.Value, out height);
                            break;
                        case "width":
                            int.TryParse(prpNode.Value, out width);
                            break;
                        case "thumbnailurl":
                            if (prpNode.Value != string.Empty)
                                tmbUrl = new Uri(prpNode.Value);
                            break;
                        case "thumbnailwidth":
                            int.TryParse(prpNode.Value, out tmbWidth);
                            break;
                        case "thumbnailheight":
                            int.TryParse(prpNode.Value, out tmbHeight);
                            break;
                    }
                }

                return new ImageSearchData(result, refererUrl, refererClickUrl, size, fileFormat, height, width, tmbUrl, tmbHeight, tmbWidth);
            }
            else
            {
                return null;
            }
        }

        private NewsSearchResult ToBossNewsSearchResult(XElement node)
        {
            SearchData result = this.ToSearchResult(node);
            if (result != null)
            {
                DateTime crwDate = default(DateTime);
                Language language = Language.en;
                string source = string.Empty;
                Uri sourceUrl = null;

                foreach (XElement prpNode in node.Elements())
                {
                    switch (prpNode.Name.LocalName)
                    {
                        case "date":
                            crwDate = new DateTime(1970, 1, 1).AddSeconds(Convert.ToInt32(prpNode.Value));
                            break;
                        case "language":
                            language = this.StringToLanguage(prpNode.Value);
                            break;
                        case "source":
                            source = prpNode.Value;
                            break;
                        case "sourceurl":
                            if (prpNode.Value.Trim() != string.Empty)
                                sourceUrl = new Uri(prpNode.Value, UriKind.Absolute);
                            break;
                    }
                }

                return new NewsSearchResult(result, source, sourceUrl, crwDate, language);
            }
            else
            {
                return null;
            }
        }

        private SpellingSearchData ToBossSpellingSearchResult(XElement node)
        {
            if (node != null && node.Name.LocalName.ToLower() == "result")
            {
                string suggestion = string.Empty;

                foreach (XElement prpNode in node.Elements())
                {
                    if (prpNode.Name.LocalName == "suggestion")
                    {
                        suggestion = prpNode.Value;
                    }
                }
                return new SpellingSearchData(suggestion);
            }
            else
            {
                return null;
            }
        }

        private Language StringToLanguage(string l)
        {
            if (l.Length >= 2)
            {
                string langCode = l.Substring(0, 2);
                foreach (Language lang in Enum.GetValues(typeof(Language)))
                {
                    if (lang.ToString() == langCode)
                    {
                        return lang;
                    }
                }
            }
            return Language.en;
        }

    }




}
