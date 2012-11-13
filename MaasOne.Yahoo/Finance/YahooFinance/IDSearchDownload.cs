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
using System.Text;
using MaasOne.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for searching for Yahoo! Finance IDs.
    /// </summary>
    /// <remarks></remarks>
    public partial class IDSearchDownload : Base.DownloadClient<IDSearchResult>
    {

        public IDSearchBaseSettings<IDSearchResult> Settings { get { return (IDSearchBaseSettings<IDSearchResult>)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public IDSearchDownload()
        {
            this.Settings = new IDInstantSearchDownloadSettings();
        }

        public void DownloadAsync(string text, object userArgs)
        {
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
            this.DownloadAsync(settings, userArgs);
        }
        public void DownloadAsync(AlphabeticalIndex index, object userArgs)
        {
            this.DownloadAsync(new IDAlphabeticSearchDownloadSettings() { Index = index }, userArgs);
        }
        public void DownloadAsync(IDSearchBaseSettings<IDSearchResult> settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override IDSearchResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            IDSearchResult result = null;
            List<IDSearchData> lst = new List<IDSearchData>();
            if (stream != null)
            {

                if (settings is IDInstantSearchDownloadSettings)
                {
                    #region Instant
                    string resultStr = MyHelper.StreamToString(stream, ((IDInstantSearchDownloadSettings)settings).TextEncoding);
                    MatchCollection results = Regex.Matches(resultStr, "{\"symbol\":.*?}");
                    foreach (Match res in results)
                    {
                        string[] prp = res.Value.Replace("{", "").Replace("}", "").Split(',');
                        if (prp.Length > 0)
                        {
                            string name = string.Empty;
                            string id = string.Empty;
                            string category = string.Empty;
                            string exchange = string.Empty;
                            string type = string.Empty;
                            foreach (string p in prp)
                            {
                                string[] kvp = p.Replace("\"", "").Split(':');
                                if (kvp.Length == 2)
                                {
                                    switch (kvp[0])
                                    {
                                        case "symbol":
                                            id = kvp[1].Trim();
                                            break;
                                        case "name":
                                            name = kvp[1].Trim();
                                            break;
                                        case "exch":
                                            exchange = kvp[1].Trim();
                                            break;
                                        case "type":
                                            switch (kvp[1].Trim())
                                            {
                                                case "S":
                                                    type = "Stock";
                                                    break;
                                                case "I":
                                                    type = "Index";
                                                    break;
                                                case "F":
                                                    type = "Future";
                                                    break;
                                                case "E":
                                                    type = "ETF";
                                                    break;
                                                case "M":
                                                    type = "Fund";
                                                    break;
                                            }
                                            break;
                                    }
                                }
                            }
                            lst.Add(new IDSearchData(name, id, type, exchange, string.Empty, null));
                        }
                    }
                    Dictionary<SecurityType, int> dict = new Dictionary<SecurityType, int>();
                    dict.Add(SecurityType.Any, lst.Count);
                    return new IDSearchResult(lst.ToArray(), 0, lst.Count, lst.Count, dict);
                    #endregion
                }
                else if (settings is IDQuerySearchDownloadSettings)
                {
                    #region Query
                    IDQuerySearchDownloadSettings sett = (IDQuerySearchDownloadSettings)settings;

                    int pageingFrom = sett.ResultsIndex, pagingTo = sett.ResultsIndex + 20, overall = 0;
                    Dictionary<SecurityType, int> resultsCount = new Dictionary<SecurityType, int>();

                    System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
                    XDocument doc = MyHelper.ParseXmlDocument(stream);

                    XElement resultNode = XPath.GetElement("//div[@id=\"yfi_sym_lookup\"]", doc);
                    if (resultNode != null)
                    {
                        XElement navigationNode = XPath.GetElement("ul[1]", resultNode);
                        if (navigationNode != null)
                        {
                            string s;
                            int t;


                            s = XPath.GetElement("li[1]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.Any, t);
                            }
                            s = XPath.GetElement("li[2]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.Stock, t);
                            }
                            s = XPath.GetElement("li[3]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.Fund, t);
                            }
                            s = XPath.GetElement("li[4]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.ETF, t);
                            }
                            s = XPath.GetElement("li[5]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.Index, t);
                            }
                            s = XPath.GetElement("li[6]/a/em", navigationNode).Value;
                            if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                            {
                                resultsCount.Add(SecurityType.Future, t);
                            }


                            if (MyHelper.EnumToArray(navigationNode.Elements()).Length == 7)
                            {
                                resultsCount.Add(SecurityType.Warrant, 0);
                                s = XPath.GetElement("li[7]/a/em", navigationNode).Value;
                                if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                                {
                                    resultsCount.Add(SecurityType.Currency, t);
                                }
                            }
                            else if (MyHelper.EnumToArray(navigationNode.Elements()).Length == 8)
                            {
                                s = XPath.GetElement("li[7]/a/em", navigationNode).Value;
                                if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                                {
                                    resultsCount.Add(SecurityType.Warrant, t);
                                }
                                s = XPath.GetElement("li[8]/a/em", navigationNode).Value;
                                if (int.TryParse(s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim(), out t))
                                {
                                    resultsCount.Add(SecurityType.Currency, t);
                                }
                            }

                        }

                        XElement contentNode = XPath.GetElement("div[1]", resultNode);
                        if (contentNode != null)
                        {


                            XElement tableNode = XPath.GetElement("/div[1]/table", contentNode);
                            XElement tableHeadNode = XPath.GetElement("/thead/tr", tableNode);
                            XElement tableBodyNode = XPath.GetElement("/tbody", tableNode);

                            List<string> tableColumnNames = new List<string>();
                            tableColumnNames.Add("symbol");
                            tableColumnNames.Add("name");
                            bool hasISIN = XPath.GetElement("/th[3]", tableHeadNode).Value.ToLower().Contains("isin");
                            if (hasISIN)
                            {
                                tableColumnNames.Add("isin");
                            }
                            else
                            {
                                tableColumnNames.Add("lasttrade");
                            }
                            int l = MyHelper.EnumToArray(tableHeadNode.Elements()).Length;
                            for (int i = 3; i < l; i++)
                            {
                                if (hasISIN)
                                {
                                    switch (i)
                                    {
                                        case 3:
                                            tableColumnNames.Add("lasttrade");
                                            break;
                                        case 4:
                                            tableColumnNames.Add("type");
                                            break;
                                        case 5:
                                            tableColumnNames.Add("exchange");
                                            break;
                                    }
                                }
                                else
                                {
                                    string name = MyHelper.GetEnumItemAt(tableHeadNode.Elements(), i).Value.ToLower();
                                    if (name.Contains("type"))
                                    {
                                        tableColumnNames.Add("type");
                                    }
                                    else if (name.Contains("industry"))
                                    {
                                        tableColumnNames.Add("industry");
                                    }
                                    else if (name.Contains("exchange"))
                                    {
                                        tableColumnNames.Add("exchange");
                                    }

                                }
                            }


                            foreach (XElement rowNode in tableBodyNode.Elements())
                            {
                                IEnumerable<XElement> enm = rowNode.Elements();
                                if (MyHelper.EnumToArray(enm).Length >= tableColumnNames.Count)
                                {
                                    string name = string.Empty, id = string.Empty, type = string.Empty, industry = string.Empty, exchange = string.Empty;
                                    ISIN isin = null;

                                    for (int i = 0; i < tableColumnNames.Count; i++)
                                    {
                                        switch (tableColumnNames[i])
                                        {
                                            case "symbol":
                                                id = MyHelper.GetEnumItemAt(enm, i).Value.Trim();
                                                break;
                                            case "name":
                                                name = MyHelper.GetEnumItemAt(enm, i).Value.Trim();
                                                break;
                                            case "isin":
                                                if (MyHelper.GetEnumItemAt(enm, i).Value.Trim() != string.Empty)
                                                {
                                                    try
                                                    {
                                                        isin = new ISIN(MyHelper.GetEnumItemAt(enm, i).Value.Trim());
                                                    }
                                                    catch { }
                                                }
                                                break;
                                            case "lasttrade":
                                                break;
                                            case "type":
                                                type = MyHelper.GetEnumItemAt(enm, i).Value.Trim();
                                                break;
                                            case "industry":
                                                industry = MyHelper.GetEnumItemAt(enm, i).Value.Trim();
                                                break;
                                            case "exchange":
                                                exchange = MyHelper.GetEnumItemAt(enm, i).Value.Trim();
                                                break;
                                        }
                                    }
                                    lst.Add(new IDSearchData(name, id, type, exchange, industry, isin));


                                }
                            }

                            overall = lst.Count;
                            XElement paginationNode = XPath.GetElement("//div[@id=\"pagination\"]", doc);
                            if (paginationNode != null)
                            {
                                PaginationScanner scn = new PaginationScanner();
                                scn.SetPagination(paginationNode.Value, out pageingFrom, out pagingTo, out overall);
                            }

                        }

                    }

                    result = new IDSearchResult(lst.ToArray(), pageingFrom, pagingTo, overall, resultsCount);
                    #endregion

                }
                else if (settings is IDAlphabeticSearchDownloadSettings)
                {
                    #region Alphabet
                    XDocument doc = MyHelper.ParseXmlDocument(stream);
                    XElement[] resultsNodes = XPath.GetElements("//results", doc);

                    if (resultsNodes.Length > 0)
                    {
                        XElement resultNode = resultsNodes[0];

                        foreach (XElement trNode in resultNode.Elements())
                        {
                            XElement[] enm = MyHelper.EnumToArray(trNode.Elements());
                            if (trNode.Name.LocalName == "tr" && enm.Length >= 2)
                            {
                                string name = string.Empty;
                                foreach (XElement subNode in enm[0].Elements())
                                {
                                    name += subNode.Value.Trim() + " ";
                                }
                                name = name.TrimEnd();
                                string id = enm[1].Value.Trim();
                                lst.Add(new IDSearchData(name, id, "stock", "", null, null));
                            }
                        }
                    }

                    result = new IDSearchResult(lst.ToArray(), -1, -1, -1, new Dictionary<SecurityType, int>());
                    #endregion

                }
            }
            if (result == null) result = new IDSearchResult(lst.ToArray(), -1, -1, -1, new Dictionary<SecurityType, int>());
            return result;
        }

        private class PaginationScanner
        {

            public void SetPagination(string txt, out int pageingFrom, out int pagingTo, out int overall)
            {
                pageingFrom = 0;
                pagingTo = 0;
                overall = 0;
                bool startedRecord = false;
                for (int i = 0; i < txt.Length; i++)
                {
                    if (startedRecord && !char.IsDigit(txt[i]) && txt[i] != '.' && txt[i] != ',' && txt[i] != ' ')
                    {
                        txt = txt.Substring(i + 1);
                        break;
                    }
                    if (char.IsDigit(txt[i]))
                    {
                        if (!startedRecord) startedRecord = true;
                        pageingFrom *= 10;
                        pageingFrom += Convert.ToInt32(txt[i]);
                    }
                }

                startedRecord = false;
                for (int i = 0; i < txt.Length; i++)
                {
                    if (startedRecord && !char.IsDigit(txt[i]) && txt[i] != '.' && txt[i] != ',' && txt[i] != ' ')
                    {
                        txt = txt.Substring(i + 1);
                        break;
                    }
                    if (char.IsDigit(txt[i]))
                    {
                        if (!startedRecord) startedRecord = true;
                        pagingTo *= 10;
                        pagingTo += Convert.ToInt32(txt[i]);
                    }
                }

                startedRecord = false;
                for (int i = 0; i < txt.Length; i++)
                {
                    if (startedRecord && !char.IsDigit(txt[i]) && txt[i] != '.' && txt[i] != ',' && txt[i] != ' ')
                    {
                        txt = txt.Substring(i + 1);
                        break;
                    }
                    if (char.IsDigit(txt[i]))
                    {
                        if (!startedRecord) startedRecord = true;
                        overall *= 10;
                        overall += Convert.ToInt32(txt[i]);
                    }
                }


            }

        }

    }



    /// <summary>
    /// Stores the result data
    /// </summary>
    public class IDSearchResult : IDSearchBaseResult
    {
        /// <summary>
        /// Gets the received ID search results.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDSearchData[] Items
        {
            get { return (IDSearchData[])base.Items; }
        }
        private int mFromIndex;
        public int FromIndex { get { return mFromIndex; } }
        private int mToIndex;
        public int ToIndex { get { return mToIndex; } }
        private int mOverallResults;
        public int ToOverallResults { get { return mOverallResults; } }
        private Dictionary<SecurityType, int> mResultsCount = null;
        public Dictionary<SecurityType, int> ResultsCount { get { return mResultsCount; } }

        internal IDSearchResult(IDSearchData[] items, int fromIndex, int toIndex, int overallResults, Dictionary<SecurityType, int> resultsCount)
            : base(items)
        {
            mFromIndex = fromIndex;
            mToIndex = toIndex;
            mOverallResults = overallResults;
            mResultsCount = resultsCount;
        }
    }


    /// <summary>
    /// Stores information about a search result. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class IDSearchData : IDSearchBaseData
    {

        private string mName = string.Empty;
        private SecurityType mType = SecurityType.Any;
        private string mExchange = string.Empty;
        private string mIndustry = null;
        private ISIN mISIN = null;

        /// <summary>
        /// The name of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }
        /// <summary>
        /// The type of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SecurityType Type
        {
            get { return mType; }
        }
        /// <summary>
        /// The stock exchange of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Exchange
        {
            get { return mExchange; }
        }
        /// <summary>
        /// The industry name of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Industry
        {
            get { return mIndustry; }
        }
        public ISIN ISIN
        {
            get { return mISIN; }
        }

        internal IDSearchData(string name, string id, string type, string exchange, string industry, ISIN isin)
            : base(id)
        {
            mName = name;
            mIndustry = industry;
            mExchange = exchange;
            mISIN = isin;
            switch (type.ToLower())
            {
                case "stock":
                case "aktien":
                case "titre":
                case "acción":
                    mType = SecurityType.Stock;
                    break;
                case "mutual fund":
                case "fund":
                case "fonds":
                case "Fonds commun de placement":
                case "fondo mutuo":
                    mType = SecurityType.Fund;
                    break;
                case "etf":
                case "trackers":
                case "fondos cotizados en bolsa":
                    mType = SecurityType.ETF;
                    break;
                case "index":
                case "indice":
                case "índice":
                    mType = SecurityType.Index;
                    break;
                case "futures":
                case "future":
                case "futuro":
                    mType = SecurityType.Future;
                    break;
                case "warrant":
                case "zertifikate & os":
                case "warrants":
                    mType = SecurityType.Warrant;
                    break;
                case "currency":
                case "währungen":
                case "devises":
                case "divisas":
                    mType = SecurityType.Currency;
                    break;
                default:
                    mType = SecurityType.Any;
                    if (type != string.Empty)
                        System.Diagnostics.Debug.WriteLine(type);
                    break;
            }
        }

    }



    public class IDInstantSearchDownloadSettings : IDSearchBaseSettings<IDSearchResult>, ITextEncodingSettings, IQuerySettings
    {
        public string Query { get; set; }
        public System.Text.Encoding TextEncoding { get; set; }
        public Culture Culture { get; set; }
        public IDInstantSearchDownloadSettings()
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.Query = string.Empty;
            this.Culture = new Culture(Language.en, Country.US);
        }
        public IDInstantSearchDownloadSettings(string query)
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.Query = query;
            this.Culture = new Culture(Language.en, Country.US);
        }
        protected override string GetUrl()
        {
            if (this.Query == string.Empty)
                throw new ArgumentNullException("Query", "The passed query text is empty.");
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            //url.Append("http://d.yimg.com/autoc.finance.yahoo.com/autoc?query=")
            //url.Append(Uri.EscapeDataString(search.Trim))
            //url.Append("&callback=YAHOO.Finance.SymbolSuggest.ssCallback")
            url.Append("http://d.yimg.com/aq/autoc?query=");
            url.Append(Uri.EscapeDataString(this.Query.Trim()));
            url.Append(YahooHelper.CultureToParameters(this.Culture));
            url.Append("&callback=YAHOO.util.ScriptNodeDataSource.callbacks");
            return url.ToString();
        }
        public override object Clone()
        {
            return new IDInstantSearchDownloadSettings() { Culture = this.Culture, Query = this.Query };
        }
    }

    /// <summary>
    /// Stores settings for Yahoo ID search
    /// </summary>
    /// <remarks></remarks>
    public class IDQuerySearchDownloadSettings : IDSearchBaseSettings<IDSearchResult>, IQuerySettings
    {

        public string Query { get; set; }
        private YahooServer mServer = YahooServer.USA;
        private SecurityType mType = SecurityType.Any;
        private FinancialMarket mMarkets = FinancialMarket.AllMarkets;
        private SecurityRankProperty mRankedBy = SecurityRankProperty.NoRanking;
        private System.ComponentModel.ListSortDirection mRankingDirection = System.ComponentModel.ListSortDirection.Ascending;

        /// <summary>
        /// The server source for downloading. Supported servers: Canada, France, Germany, Spain, UK, USA
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public YahooServer Server
        {
            get { return mServer; }
            set { mServer = value; }
        }
        /// <summary>
        /// The search will be limited to a special type or all
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SecurityType Type
        {
            get { return mType; }
            set { mType = value; }
        }
        /// <summary>
        /// The search will be limited to a special market or all
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FinancialMarket Markets
        {
            get { return mMarkets; }
            set { mMarkets = value; }
        }
        /// <summary>
        /// The ranking property of the result list
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Will be ignored if "GermanServer" is True</remarks>
        public SecurityRankProperty RankedBy
        {
            get { return mRankedBy; }
            set { mRankedBy = value; }
        }
        /// <summary>
        /// The ranking direction of the result list
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Will be ignored if "GermanServer" is True</remarks>
        public System.ComponentModel.ListSortDirection RankingDirection
        {
            get { return mRankingDirection; }
            set { mRankingDirection = value; }
        }
        public int ResultsIndex { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public IDQuerySearchDownloadSettings()
        {
            this.ResultsIndex = 0;
        }

        protected override string GetUrl()
        {
            if (this.Query.Trim() == string.Empty)
                throw new ArgumentNullException("Query", "The query text is empty.");
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            url.Append("http://");
            url.Append(YahooHelper.ServerString(this.Server));
            url.Append("finance.yahoo.com/lookup/");
            switch (this.Type)
            {
                case SecurityType.Any:
                    url.Append("all");
                    break;
                case SecurityType.ETF:
                    url.Append("etfs");
                    break;
                case SecurityType.Fund:
                    url.Append("funds");
                    break;
                case SecurityType.Future:
                    url.Append("futures");
                    break;
                case SecurityType.Index:
                    url.Append("indices");
                    break;
                case SecurityType.Stock:
                    url.Append("stocks");
                    break;
                case SecurityType.Warrant:
                    url.Append("warrants");
                    break;
                case SecurityType.Currency:
                    url.Append("currency");
                    break;
            }
            url.Append("?s=");
            url.Append(Uri.EscapeDataString(this.Query));
            url.Append("&t=");
            if (this.Type == SecurityType.Fund)
            {
                url.Append('M');
            }
            else
            {
                url.Append(char.ToUpper(this.Type.ToString()[0]));
            }
            url.Append("&m=");
            switch (this.Markets)
            {
                case FinancialMarket.France:
                    url.Append("FR");
                    break;
                case FinancialMarket.Germany:
                    url.Append("DR");
                    break;
                case FinancialMarket.Spain:
                    url.Append("ES");
                    break;
                case FinancialMarket.UK:
                    url.Append("GB");
                    break;
                case FinancialMarket.UsAndCanada:
                    url.Append("US");
                    break;
                default:
                    url.Append("ALL");
                    break;
            }
            url.Append("&r=");
            if (this.RankedBy != SecurityRankProperty.NoRanking)
            {
                int rankNumber = Convert.ToInt32(this.RankedBy);
                if (this.RankingDirection == ListSortDirection.Descending)
                    rankNumber += 1;
                url.Append(rankNumber.ToString());
            }
            url.Append("&b=");
            url.Append(this.ResultsIndex);

            return url.ToString();
            string whereClause = string.Format("url=\"{0}\" AND xpath='//div[@id=\"yfi_sym_lookup\"]'", url.ToString());
            return MyHelper.YqlUrl("*", "html", whereClause, null, false);
        }

        public override object Clone()
        {
            return new IDQuerySearchDownloadSettings()
            {
                Query = this.Query,
                ResultsIndex = this.ResultsIndex,
                Server = this.Server,
                Type = this.Type,
                Markets = this.Markets,
                RankedBy = this.RankedBy,
                RankingDirection = this.RankingDirection
            };
        }

    }



    public class IDAlphabeticSearchDownloadSettings : IDSearchBaseSettings<IDSearchResult>
    {
        /// <summary>
        /// The Index downloaded with [AlphabeticIDIndexDownload].
        /// </summary>
        public AlphabeticalIndex Index { get; set; }

        public IDAlphabeticSearchDownloadSettings()
        {

        }
        protected override string GetUrl()
        {
            return MyHelper.YqlUrl("*", "html", string.Format("url='{0}' AND xpath='//html/body/table/tr'", this.Index.URL), null, false);
        }

        public override object Clone()
        {
            return new IDAlphabeticSearchDownloadSettings() { Index = this.Index };
        }
    }


}
