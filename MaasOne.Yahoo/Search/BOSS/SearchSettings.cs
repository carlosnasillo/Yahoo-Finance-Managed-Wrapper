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



    public class SearchDownloadSettings : Base.SettingsBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mConsumerKey, mConsumerSecret;
        private bool mHttpsUsed = false;
        public string ConsumerKey
        {
            get { return mConsumerKey; }
            set { mConsumerKey = value; this.OnPropertyChanged("ConsumerKey"); }
        }
        public string ConsumerSecret
        {
            get { return mConsumerSecret; }
            set { mConsumerSecret = value; this.OnPropertyChanged("ConsumerSecret"); }
        }
        public bool HttpsUsed
        {
            get { return mHttpsUsed; }
            set { mHttpsUsed = value; this.OnPropertyChanged("HttpsUsed"); }
        }
        public System.ComponentModel.BindingList<SearchService> Services { get; set; }

        public SearchDownloadSettings()
        {
            this.Services = new System.ComponentModel.BindingList<SearchService>();
            mConsumerKey = string.Empty;
            mConsumerSecret = string.Empty;
        }

        protected override string GetUrl()
        {
            if (this.ConsumerKey == string.Empty)
                throw new ArgumentException("The OAuth Consumer Key is empty.", "clientOptions.ConsumerKey");
            if (this.ConsumerSecret == null || this.ConsumerSecret.Length == 0)
                throw new ArgumentException("The OAuth Consumer Secret is null or empty.", "clientOptions.ConsumerSecret");
            if (this.Services == null || this.Services.Count == 0)
                throw new ArgumentNullException("services", "There are no passed services for searching.");

            System.Text.StringBuilder url = new System.Text.StringBuilder();
            if (this.HttpsUsed)
            {
                url.Append("https");
            }
            else
            {
                url.Append("http");
            }
            url.Append("://yboss.yahooapis.com/ysearch/");

            foreach (MaasOne.Search.BOSS.SearchService serv in this.Services)
            {
                url.Append(serv.ServiceName + ",");
            }
            url.Remove(url.Length - 1, 1);

            url.Append("?");

            for (int i = 0; i < this.Services.Count; i++)
            {
                string prt = string.Empty;
                if (this.Services[i] is WebSearchService)
                {
                    prt = this.Services[i].GetUrlPart();
                }
                else if (this.Services[i] is ImageSearchService)
                {
                    prt = this.Services[i].GetUrlPart();
                }
                else if (this.Services[i] is NewsSearchService)
                {
                    prt = this.Services[i].GetUrlPart();
                }
                else if (this.Services[i] is SpellingSearchService)
                {
                    prt = this.Services[i].GetUrlPart();
                }
                if (i == 0) prt = prt.Substring(1);
                url.Append(prt);
            }
            foreach (SearchService serv in this.Services)
            {

            }

            url.Append("&format=xml");

            this.AdditionalHeaders.Clear();
            this.AdditionalHeaders.Add(this.GetOAuthHeader(url.ToString()));
            return url.ToString();
        }

        protected virtual void OnPropertyChanged(string prpName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(prpName));
        }

        public override object Clone()
        {
            SearchDownloadSettings cln = new SearchDownloadSettings();
            cln.ConsumerKey = this.ConsumerKey;
            cln.ConsumerSecret = this.ConsumerSecret;
            cln.HttpsUsed = this.HttpsUsed;
            foreach (SearchService serv in this.Services)
            {
                if (serv is WebSearchService)
                {
                    cln.Services.Add((WebSearchService)serv.Clone());
                }
                else if (serv is ImageSearchService)
                {
                    cln.Services.Add((ImageSearchService)serv.Clone());
                }
                else if (serv is NewsSearchService)
                {
                    cln.Services.Add((NewsSearchService)serv.Clone());
                }
                else if (serv is SpellingSearchService)
                {
                    cln.Services.Add((SpellingSearchService)serv.Clone());
                }
            };
            cln.AdditionalHeaders.Clear();
            cln.AdditionalHeaders.AddRange(this.AdditionalHeaders);
            return cln;
        }

        private KeyValuePair<HttpRequestHeader, string> GetOAuthHeader(string url)
        {
            OAuthBase oa = new OAuthBase();
            string nUrl = string.Empty;
            string nParam = string.Empty;
            string nonce = oa.GenerateNonce();
            string timestamp = oa.GenerateTimeStamp();
            string signature = oa.GenerateSignature(new Uri(Uri.EscapeUriString(url)), this.ConsumerKey, this.ConsumerSecret, "", "", "GET", timestamp, nonce, MaasOne.Search.BOSS.OAuthBase.SignatureTypes.HMACSHA1, out nUrl, out nParam);
            string headerValue = string.Format("OAuth oauth_version=\"1.0\", oauth_nonce=\"{0}\", oauth_timestamp=\"{1}\", oauth_consumer_key=\"{2}\", oauth_signature_method=\"HMAC-SHA1\", oauth_signature=\"{3}\"", nonce, timestamp, this.ConsumerKey, signature);
            return new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, headerValue);
        }

    }


    public abstract class SearchService : IResultIndexSettings, IQuerySettings, ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int mIndex = 0, mCount = 10;
        public int Index { get { return mIndex; } set { mIndex = value; this.OnPropertyChanged("Index"); } }
        public int Count { get { return mCount; } set { mCount = value; this.OnPropertyChanged("Count"); } }

        internal abstract string ServiceName { get; }

        private string mQuery = string.Empty;
        public string Query { get { return mQuery; } set { mQuery = value; this.OnPropertyChanged("Query"); } }

        private Culture mCulture = new Culture(Language.en, Country.US);
        public Culture Culture { get { return mCulture; } set { mCulture = value; this.OnPropertyChanged("Culture"); } }

        public System.ComponentModel.BindingList<Uri> RestrictedSites { get; set; }

        internal SearchService()
        {
            this.RestrictedSites = new System.ComponentModel.BindingList<Uri>();
        }
        internal SearchService(SearchService original)
        {
            this.RestrictedSites = new System.ComponentModel.BindingList<Uri>();
            if (original != null)
            {
                this.Index = original.Index;
                this.Count = original.Count;
                this.Query = original.Query;
                if (original.Culture != null)
                {
                    this.Culture = original.Culture.CloneStrict();
                }
                else
                {
                    this.Culture = null;
                }
                if (original.RestrictedSites != null)
                {
                    foreach (Uri siteRest in original.RestrictedSites)
                    {
                        this.RestrictedSites.Add(siteRest);
                    }
                }
            }
        }

        protected virtual void OnPropertyChanged(string prpName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(prpName));
        }

        protected string GetTimeSpanString(System.DateTime d)
        {
            System.DateTime now = System.DateTime.Now;
            System.DateTime tDate = System.DateTime.Now;
            if (now > d)
                tDate = d;
            TimeSpan span = now - tDate;
            if (span.TotalSeconds < 1000)
            {
                return Convert.ToInt32(span.TotalSeconds).ToString() + 's';
            }
            else
            {
                if (span.TotalMinutes < 1000)
                {
                    return Convert.ToInt32(span.TotalMinutes).ToString() + 'm';
                }
                else
                {
                    if (span.TotalHours < 1000)
                    {
                        return Convert.ToInt32(span.TotalHours).ToString() + 'h';
                    }
                    else
                    {
                        if (span.TotalDays < 1000)
                        {
                            return Convert.ToInt32(span.TotalDays).ToString() + 'd';
                        }
                        else
                        {
                            if (span.TotalDays / 7 < 1000)
                            {
                                return Convert.ToInt32(span.TotalDays / 7).ToString() + 'w';
                            }
                            else
                            {
                                return "1000w";
                            }
                        }
                    }
                }
            }
        }

        protected string GetUniversalParameters()
        {
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            res.AppendFormat("&{0}.q={1}", this.ServiceName, this.Query);
            if (this.Index > 0)
            {
                res.Append("&" + this.ServiceName + ".start=");
                res.Append(Math.Max(0, this.Index).ToString());
            }

            res.Append("&" + this.ServiceName + ".count=");
            res.Append(Math.Max(1, this.Count).ToString());

            if (this.Culture != null && !(this.Culture.Language == Language.en & this.Culture.Country == Country.US))
            {
                res.Append("&" + this.ServiceName + ".");
                res.Append(string.Format("market={0}-{1}", this.Culture.Language.ToString(), this.Culture.Country.ToString().ToLower()));
            }

            if (this.RestrictedSites != null && this.RestrictedSites.Count > 0)
            {
                res.Append("&" + this.ServiceName + ".sites=");
                foreach (Uri site in this.RestrictedSites)
                {
                    res.Append(site.Host + ",");
                }
                res.Remove(res.Length - 1, 1);
            }

            return res.ToString();
        }

        public abstract object Clone();
        internal abstract string GetUrlPart();
    }


    public class WebSearchService : SearchService
    {
        private bool mAllowAdultContent = true;
        private WebFileType[] mAllowedFileTypes = new WebFileType[-1 + 1];
        private FileTypeGroup[] mAllowedFileGroups = new FileTypeGroup[-1 + 1];
        private bool mLongAbstract = false;
        private bool mLimitedWeb = false;
        private bool mHtmlTaggedText = true;
        private string mRestrictedTitle = string.Empty;
        private string mRestrictedUrl = string.Empty;
        private bool mSearchMonkey = false;

        private bool mLanguageInfo = false;
        internal override string ServiceName
        {
            get
            {
                if (this.LimitedWeb)
                {
                    return "limitedweb";
                }
                else
                {
                    return "web";
                }
            }
        }

        /// <summary>
        /// Returns minimum 3 days old search results. Costs the half fee of full web search.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LimitedWeb
        {
            get { return mLimitedWeb; }
            set { mLimitedWeb = value; this.OnPropertyChanged("LimitedWeb"); }
        }
        /// <summary>
        /// Filters out hate and porn content.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AllowAdultContent
        {
            get { return mAllowAdultContent; }
            set { mAllowAdultContent = value; this.OnPropertyChanged("AllowAdultContent"); }
        }
        /// <summary>
        /// Restrict search results to setted file types.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public WebFileType[] AllowedFileTypes
        {
            get { return mAllowedFileTypes; }
            set
            {
                if (value != null)
                {
                    mAllowedFileTypes = value;
                }
                else
                {
                    mAllowedFileTypes = new WebFileType[] { };
                }
                this.OnPropertyChanged("AllowedFileTypes");
            }
        }
        /// <summary>
        /// Will retrieve and display an abstract of a web document up to 300 characters.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LongAbstract
        {
            get { return mLongAbstract; }
            set { mLongAbstract = value; this.OnPropertyChanged("LongAbstract"); }
        }
        /// <summary>
        /// Will return search results with the title containing the value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RestrictedTitle
        {
            get { return mRestrictedTitle; }
            set { mRestrictedTitle = value; this.OnPropertyChanged("RestrictedTitle"); }
        }
        /// <summary>
        /// Will return search results with the URL containing the value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RestrictedUrl
        {
            get { return mRestrictedUrl; }
            set { mRestrictedUrl = value; this.OnPropertyChanged("RestrictedUrl"); }
        }
        /// <summary>
        /// Same effect like AllowedFileTypes property.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileTypeGroup[] AllowedFileGroups
        {
            get { return mAllowedFileGroups; }
            set
            {
                if (value != null)
                {
                    mAllowedFileGroups = value;
                }
                else
                {
                    mAllowedFileGroups = new FileTypeGroup[] { };
                }
                this.OnPropertyChanged("AllowedFileGroups");
            }
        }
        /// <summary>
        /// If [FALSE], cleans out the HTML tags from the abstract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HtmlTaggedText
        {
            get { return mHtmlTaggedText; }
            set { mHtmlTaggedText = value; this.OnPropertyChanged("HtmlTaggedText"); }
        }
        /// <summary>
        /// Provides additional data in Microformat.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SearchMonkey
        {
            get { return mSearchMonkey; }
            set { mSearchMonkey = value; this.OnPropertyChanged("SearchMonkey"); }
        }
        /// <summary>
        /// Identifies the language of the result/document.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LanguageInfo
        {
            get { return mLanguageInfo; }
            set { mLanguageInfo = value; this.OnPropertyChanged("LanguageInfo"); }
        }

        public WebSearchService()
        {
            this.Count = 50;
        }

        internal override string GetUrlPart()
        {
            System.Text.StringBuilder pars = new System.Text.StringBuilder();
            pars.Append(base.GetUniversalParameters());
            if (!this.AllowAdultContent)
                pars.Append("&filter=-porn");
            if (this.LongAbstract)
                pars.Append("&abstract=long");
            if (this.RestrictedTitle != string.Empty)
                pars.Append("&title=" + this.RestrictedTitle);
            if (this.RestrictedUrl != string.Empty)
                pars.Append("&url=" + this.RestrictedUrl);
            if (!this.HtmlTaggedText)
                pars.Append("&style=raw");

            if ((this.AllowedFileTypes != null && this.AllowedFileTypes.Length > 0) | (this.AllowedFileGroups != null && this.AllowedFileGroups.Length > 0))
            {
                pars.Append("&type=");
                List<WebFileType> lst = new List<WebFileType>();
                foreach (FileTypeGroup grp in this.AllowedFileGroups)
                {
                    foreach (WebFileType t in grp.FileTypes)
                    {
                        if (!lst.Contains(t))
                            lst.Add(t);
                    }
                }
                foreach (WebFileType t in this.AllowedFileTypes)
                {
                    if (!lst.Contains(t))
                        lst.Add(t);
                }
                foreach (WebFileType t in lst)
                {
                    pars.Append(t.ToString().ToLower());
                    if (lst.IndexOf(t) != lst.Count - 1)
                        pars.Append(",");
                }
            }
            if (this.SearchMonkey | this.LanguageInfo)
            {
                pars.Append("&view=");
                if (this.SearchMonkey)
                    pars.Append("smfeed,");
                if (this.LanguageInfo)
                    pars.Append("language,");
                pars.Remove(pars.Length - 1, 1);
            }

            return pars.ToString();
        }


        public override object Clone()
        {
            WebSearchService cln = new WebSearchService();
            cln.Count = this.Count;
            cln.Index = this.Index;
            cln.Culture = this.Culture;
            cln.Query = this.Query;
            cln.RestrictedSites = this.RestrictedSites;
            cln.LongAbstract = this.LongAbstract;
            cln.AllowAdultContent = this.AllowAdultContent;
            cln.AllowedFileTypes = (WebFileType[])this.AllowedFileTypes.Clone();
            cln.AllowedFileGroups = (FileTypeGroup[])this.AllowedFileGroups.Clone();
            cln.LimitedWeb = this.LimitedWeb;
            cln.HtmlTaggedText = this.HtmlTaggedText;
            cln.RestrictedTitle = this.RestrictedTitle;
            cln.RestrictedUrl = this.RestrictedUrl;
            return cln;
        }
    }




    public class NewsSearchService : SearchService
    {

        private bool mAlwaysLatestDateNow = true;
        private System.DateTime mLatestDate = System.DateTime.Now;
        private TimeSpan mTimeSpan = new TimeSpan(30, 0, 0, 0);
        private NewsSortRanking mSorting = NewsSortRanking.None;
        private string mRestrictedTitle = string.Empty;

        private string mRestrictedUrl = string.Empty;
        internal override string ServiceName
        {
            get { return "news"; }
        }

        /// <summary>
        /// Indicates if the [LatestDate] Property will return actual DateTime.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlwaysLatestDateNow
        {
            get { return mAlwaysLatestDateNow; }
            set { mAlwaysLatestDateNow = value; this.OnPropertyChanged("AlwaysLatestDateNow"); }
        }
        /// <summary>
        /// The maximum date of crawling date/time. Setting a date will deactivate [AlwaysLatestDateNow].
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime LatestDate
        {
            get
            {
                if (mAlwaysLatestDateNow)
                {
                    return System.DateTime.Now;
                }
                else
                {
                    return mLatestDate;
                }
            }
            set
            {
                mLatestDate = value;
                mAlwaysLatestDateNow = false;
                this.OnPropertyChanged("LatestDate");
            }
        }
        /// <summary>
        /// The timespan from latest date to minimum date of crawling date/time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan TimeSpan
        {
            get { return mTimeSpan; }
            set
            {
                if (value.TotalSeconds > 0)
                {
                    TimeSpan absValue = value;
                    if (value.TotalSeconds < 0)
                        absValue = value.Negate();
                    if (absValue.TotalDays / 7 < 1000)
                    {
                        mTimeSpan = absValue;
                    }
                    else
                    {
                        mTimeSpan = new TimeSpan(999 * 7, 0, 0, 0);
                    }
                }
                else
                {
                    mTimeSpan = new TimeSpan(30, 0, 0, 0);
                }
                this.OnPropertyChanged("TimeSpan");
            }
        }
        /// <summary>
        /// The calculated minimum date of crawling date/time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime EarliestDate
        {
            get { return mLatestDate.Subtract(mTimeSpan); }
        }
        /// <summary>
        /// The kind of sorting of results. Influences priority of result index.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public NewsSortRanking Sorting
        {
            get { return mSorting; }
            set { mSorting = value; this.OnPropertyChanged("Sorting"); }
        }
        /// <summary>
        /// Will return search results with the title containing the value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RestrictedTitle
        {
            get { return mRestrictedTitle; }
            set { mRestrictedTitle = value; this.OnPropertyChanged("RestrictedTitle"); }
        }
        /// <summary>
        /// Will return search results with the URL containing the value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RestrictedUrl
        {
            get { return mRestrictedUrl; }
            set { mRestrictedUrl = value; this.OnPropertyChanged("RestrictedUrl"); }
        }

        public NewsSearchService()
        {
            this.Count = 50;
        }

        internal override string GetUrlPart()
        {
            System.Text.StringBuilder pars = new System.Text.StringBuilder();
            pars.Append(base.GetUniversalParameters());
            pars.Append("&age=");
            if (System.DateTime.Now.AddSeconds(-60) < this.LatestDate)
            {
                pars.Append(base.GetTimeSpanString(this.EarliestDate));
            }
            else
            {
                pars.Append(base.GetTimeSpanString(this.LatestDate));
                pars.Append("-");
                pars.Append(base.GetTimeSpanString(this.EarliestDate));
            }
            if (this.Sorting != MaasOne.Search.BOSS.NewsSortRanking.None)
            {
                pars.Append("&sort=");
                switch (this.Sorting)
                {
                    case MaasOne.Search.BOSS.NewsSortRanking.AsiaRanking:
                        pars.Append("asiarank");
                        break;
                    case MaasOne.Search.BOSS.NewsSortRanking.EuRanking:
                        pars.Append("eurank");
                        break;
                    case MaasOne.Search.BOSS.NewsSortRanking.UsRanking:
                        pars.Append("usrank");
                        break;
                    default:
                        pars.Append("date");
                        break;
                }
            }
            if (this.RestrictedTitle != string.Empty)
                pars.Append("&title=" + this.RestrictedTitle);
            if (this.RestrictedUrl != string.Empty)
                pars.Append("&title=" + this.RestrictedUrl);
            return pars.ToString();
        }

        public override object Clone()
        {
            NewsSearchService cln = new NewsSearchService();
            cln.Count = this.Count;
            cln.Index = this.Index;
            cln.Culture = this.Culture;
            cln.Query = this.Query;
            cln.RestrictedSites = this.RestrictedSites;
            cln.AlwaysLatestDateNow = this.AlwaysLatestDateNow;
            cln.LatestDate = this.LatestDate;
            cln.TimeSpan = this.TimeSpan;
            cln.RestrictedTitle = this.RestrictedTitle;
            cln.RestrictedUrl = this.RestrictedUrl;
            return cln;
        }

    }






    public class ImageSearchService : SearchService
    {

        private bool mAllowAdultContent = false;
        private ImageSearchDimensions mDimensions = ImageSearchDimensions.All;
        private Uri mUrl = null;

        private Uri mRefererUrl = null;
        internal override string ServiceName
        {
            get { return "images"; }
        }

        /// <summary>
        /// Activate/Deactivates the Offensive Content Reduction filter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AllowAdultContent
        {
            get { return mAllowAdultContent; }
            set { mAllowAdultContent = value; this.OnPropertyChanged("AllowAdultContent"); }
        }
        /// <summary>
        /// The image size.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ImageSearchDimensions Dimensions
        {
            get { return mDimensions; }
            set { mDimensions = value; this.OnPropertyChanged("Dimensions"); }
        }
        /// <summary>
        /// Search for this URL. Returns this exact image result.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Uri Url
        {
            get { return mUrl; }
            set { mUrl = value; this.OnPropertyChanged("Url"); }
        }
        /// <summary>
        /// Search for this URL. Depending on other query restrictions, returns all image objects with this referring URL.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Uri RefererUrl
        {
            get { return mRefererUrl; }
            set { mRefererUrl = value; this.OnPropertyChanged("RefererUrl"); }
        }

        public ImageSearchService()
        {
            this.Count = 35;
        }

        internal override string GetUrlPart()
        {
            System.Text.StringBuilder pars = new System.Text.StringBuilder();
            pars.Append(base.GetUniversalParameters());
            if (this.AllowAdultContent)
                pars.Append("&filter=no");
            if (this.Dimensions != MaasOne.Search.BOSS.ImageSearchDimensions.All)
                pars.Append("&dimensions=" + this.Dimensions.ToString().ToLower());
            if (this.RefererUrl != null)
                pars.Append("&refererurl=" + this.RefererUrl.ToString());
            if (this.Url != null)
                pars.Append("&url=" + this.Url.ToString());
            return pars.ToString();
        }

        public override object Clone()
        {
            ImageSearchService cln = new ImageSearchService();
            cln.Count = this.Count;
            cln.Index = this.Index;
            cln.Culture = this.Culture;
            cln.Query = this.Query;
            cln.RestrictedSites = this.RestrictedSites;
            cln.AllowAdultContent = this.AllowAdultContent;
            cln.Dimensions = this.Dimensions;
            cln.Url = this.Url;
            cln.RefererUrl = this.RefererUrl;
            return cln;
        }
    }



    public class SpellingSearchService : SearchService
    {


        internal override string ServiceName
        {
            get { return "spelling"; }
        }

        public SpellingSearchService()
        {
            this.Count = 1;
        }
        internal SpellingSearchService(SpellingSearchService original)
            : base(original)
        {
        }

        internal override string GetUrlPart()
        {
            System.Text.StringBuilder paras = new System.Text.StringBuilder();
            paras.Append(base.GetUniversalParameters());
            return paras.ToString();
        }

        public override object Clone()
        {
            return new SpellingSearchService() { Count = this.Count, Culture = this.Culture, Index = this.Index, Query = this.Query, RestrictedSites = this.RestrictedSites };
        }


    }

}
