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



namespace MaasOne.Finance.YahooPortfolio
{

    public partial class PortfolioDownload : DownloadClient<Portfolio>
    {
        public PortfolioDownloadSettings Settings { get { return (PortfolioDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public PortfolioDownload()
        {
            this.Settings = new PortfolioDownloadSettings();
        }

        protected override Portfolio ConvertResult(ConnectionInfo connInfo, System.IO.Stream stream, SettingsBase settings)
        {
            PortfolioDownloadSettings set = (PortfolioDownloadSettings)settings;
            return ConvertHtmlDoc(MyHelper.ParseXmlDocument(stream));
        }


        internal Portfolio ConvertHtmlDoc(XDocument doc)
        {
            if (doc != null)
            {
                List<IID> lstIDs = new List<IID>();
                PortfolioInfo ownPf = null;
                List<PortfolioInfo> portfolios = new List<PortfolioInfo>();
                List<PortfolioDataRow> dt = new List<PortfolioDataRow>();
                string selectedView = string.Empty;
                List<PortfolioColumnType> lstColumns = new List<PortfolioColumnType>();
                List<PortfolioColumnType> lstViewColumns = new List<PortfolioColumnType>();
                List<string> views = new List<string>();
                XElement mainNode = XPath.GetElement("//div[@id=\"yfi-main\"]", doc);
                if (mainNode != null)
                {
                    XElement portfoliosNode = XPath.GetElement("/div[@class=\"yfi_dropdown_select\"]/div[@class=\"bd\"]/form/div/select", mainNode);
                    if (portfoliosNode != null)
                    {
                        foreach (XElement childNode in portfoliosNode.Elements())
                        {
                            if (childNode.Name.LocalName == "option" && childNode.HasElements)
                            {
                                string id = childNode.Attribute(XName.Get("value")).Value;
                                string name = childNode.Value;
                                portfolios.Add(new PortfolioInfo(name, id));
                                if (childNode.Attribute(XName.Get("selected")) != null && childNode.Attribute(XName.Get("selected")).Value == "selected") ownPf = portfolios[portfolios.Count - 1];
                            }
                        }
                    }

                    XElement[] viewNodes = XPath.GetElements("/div[@class=\"yfi_tablist\"]/div/ul/li", mainNode);
                    if (viewNodes.Length > 0)
                    {
                        foreach (XElement viewNode in viewNodes)
                        {
                            if (!viewNode.Value.Contains("Detailed") && !viewNode.Value.Contains("Add Custom View"))
                            {
                                views.Add(viewNode.Value);
                                if (XPath.GetElement("/strong", viewNode) != null)
                                {
                                    selectedView = viewNode.Value;
                                }
                            }
                        }
                    }

                    XElement tableNode = XPath.GetElement("/div[@class=\"yfi_module yfi-quotes-table\"]/div[2]/table", mainNode);
                    if (tableNode != null)
                    {
                        XElement[] columnCells = XPath.GetElements("/thead/tr/th", tableNode);

                        foreach (XElement thNode in columnCells)
                        {
                            XAttribute colNameAtt = thNode.Attribute(XName.Get("class"));
                            if (colNameAtt != null && colNameAtt.Value.StartsWith("col-"))
                            {
                                string colName = colNameAtt.Value.Split(' ')[0].Replace("col-", "");
                                Nullable<PortfolioColumnType> colType = this.GetColumnType(colName);

                                if (colType != null)
                                {
                                    int subColumnCount = Convert.ToInt32(thNode.Attribute(XName.Get("colspan")).Value);
                                    if (subColumnCount == 1)
                                    {
                                        lstColumns.Add(colType.Value);
                                        lstViewColumns.Add(colType.Value);
                                    }
                                    else
                                    {
                                        Nullable<PortfolioColumnType>[] addCT = new Nullable<PortfolioColumnType>[subColumnCount];
                                        switch (colType)
                                        {
                                            case PortfolioColumnType.change:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.change; addCT[1] = PortfolioColumnType.percent_change; }
                                                lstViewColumns.Add(PortfolioColumnType.change_and_percent);
                                                break;
                                            case PortfolioColumnType.time:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.time; addCT[1] = PortfolioColumnType.price; }
                                                lstViewColumns.Add(PortfolioColumnType.price_and_time);
                                                break;
                                            case PortfolioColumnType.day_low:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.day_low; addCT[1] = PortfolioColumnType.day_high; }
                                                lstViewColumns.Add(PortfolioColumnType.day_range);
                                                break;
                                            case PortfolioColumnType.day_value_change:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.day_value_change; addCT[1] = PortfolioColumnType.day_value_percent_change; }
                                                lstViewColumns.Add(PortfolioColumnType.day_value_change_and_percent);
                                                break;
                                            case PortfolioColumnType.fiftytwo_week_low:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.fiftytwo_week_low; addCT[1] = PortfolioColumnType.fiftytwo_week_high; }
                                                lstViewColumns.Add(PortfolioColumnType.fiftytwo_week_range);
                                                break;
                                            case PortfolioColumnType.pre_mkt_time:
                                                PortfolioColumnType t = PortfolioColumnType.pre_mkt_price_and_time;
                                                if (subColumnCount >= 2) { addCT[0] = PortfolioColumnType.pre_mkt_time; addCT[1] = PortfolioColumnType.pre_mkt_price; }
                                                if (subColumnCount == 4) { addCT[2] = PortfolioColumnType.after_mkt_time; addCT[3] = PortfolioColumnType.after_mkt_price; t = PortfolioColumnType.pre_after_mkt_price_and_time; }
                                                lstViewColumns.Add(t);
                                                break;
                                            case PortfolioColumnType.after_mkt_time:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.after_mkt_time; addCT[1] = PortfolioColumnType.after_mkt_price; }
                                                lstViewColumns.Add(PortfolioColumnType.pre_after_mkt_price_and_time);
                                                break;
                                            case PortfolioColumnType.pre_mkt_change:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.pre_mkt_change; addCT[1] = PortfolioColumnType.pre_mkt_percent_change; }
                                                lstViewColumns.Add(PortfolioColumnType.pre_mkt_change_and_percent);
                                                break;
                                            case PortfolioColumnType.after_mkt_change:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.after_mkt_change; addCT[1] = PortfolioColumnType.after_mkt_percent_change; }
                                                lstViewColumns.Add(PortfolioColumnType.after_mkt_change_and_percent);
                                                break;
                                            case PortfolioColumnType.holdings_gain:
                                                if (subColumnCount == 2) { addCT[0] = PortfolioColumnType.holdings_gain; addCT[1] = PortfolioColumnType.holdings_percent_gain; }
                                                lstViewColumns.Add(PortfolioColumnType.holdings_gain_and_percent);
                                                break;
                                        }

                                        for (int i = 0; i < addCT.Length; i++)
                                        {
                                            if (addCT[i] != null)
                                            {
                                                lstColumns.Add(addCT[i].Value);
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine(colName);
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        XElement[] tableCells = XPath.GetElements("/tbody/tr", tableNode);
                        foreach (XElement trNode in tableCells)
                        {
                            XElement[] enm = MyHelper.EnumToArray(trNode.Elements());
                            if (enm.Length > 0)
                            {
                                PortfolioDataRow r = new PortfolioDataRow();
                                for (int i = 0; i < enm.Length; i++)
                                {
                                    XElement tdNode = enm[i];
                                    XAttribute colNameAtt = tdNode.Attribute(XName.Get("class"));
                                    if (colNameAtt != null && colNameAtt.Value.StartsWith("col-"))
                                    {
                                        XElement[] tdEnm = MyHelper.EnumToArray(tdNode.Elements());
                                        string colName = colNameAtt.Value.Split(' ')[0].Replace("col-", "");
                                        if (i < lstColumns.Count && lstColumns[i].ToString() == colName &&
                                            colName != "delete" &&
                                            (tdNode.Attribute(XName.Get("class")) != null && !tdNode.Attribute(XName.Get("class")).Value.StartsWith("col-delete")) &&
                                            tdEnm.Length > 0 && tdEnm[0].Name.LocalName == "span" && tdEnm[0].Attribute(XName.Get("class")) != null && tdEnm[0].Attribute(XName.Get("class")).Value == "wrapper")
                                        {
                                            string cellTxt = tdNode.Value;
                                            r[lstColumns[i]] = cellTxt;
                                            if (lstColumns[i] == PortfolioColumnType.symbol) lstIDs.Add(new SimpleID(cellTxt));
                                        }
                                    }

                                }
                                dt.Add(r);
                            }
                        }
                    }
                }

                return new Portfolio(ownPf, lstIDs.ToArray(), dt.ToArray(), selectedView, views.ToArray(), lstColumns.ToArray(), lstViewColumns.ToArray(), portfolios.ToArray());
            }
            else { return null; }
        }


        private Nullable<PortfolioColumnType> GetColumnType(string name)
        {
            foreach (PortfolioColumnType pct in Enum.GetValues(typeof(PortfolioColumnType)))
            {
                if (pct.ToString() == name)
                {
                    return pct;
                }
            }
            return null;
        }

    }



    public partial class Portfolio
    {
        private PortfolioInfo[] mPortfolios = null;
        public PortfolioInfo[] Portfolios { get { return mPortfolios; } }
        private PortfolioInfo mInfo = null;
        public PortfolioInfo Info { get { return mInfo; } }
        private IID[] mIDs = null;
        public IID[] IDs { get { return mIDs; } }
        private string mSelectedView = string.Empty;
        public string SelectedView { get { return mSelectedView; } }

        private PortfolioColumnType[] mViewColumns = new PortfolioColumnType[] { };
        public PortfolioColumnType[] ViewColumns { get { return mViewColumns; } }
        private PortfolioColumnType[] mColumns = new PortfolioColumnType[] { };
        public PortfolioColumnType[] Columns { get { return mColumns; } }
        private PortfolioDataRow[] mRows = null;
        public PortfolioDataRow[] Rows { get { return mRows; } }
        private string[] mAvailableViews = null;
        public string[] AvailableViews { get { return mAvailableViews; } }
        internal Portfolio(PortfolioInfo info, IID[] ids, PortfolioDataRow[] data, string selectedView, string[] views, PortfolioColumnType[] availableColumns, PortfolioColumnType[] availableViewColumns, PortfolioInfo[] portfolios)
        {
            mInfo = info;
            mIDs = ids;
            mRows = data;
            mSelectedView = selectedView;
            mAvailableViews = views;
            mColumns = availableColumns;
            mViewColumns = availableViewColumns;
            mPortfolios = portfolios;
        }

        public override string ToString()
        {
            return mInfo.ToString();
        }

        public static string GetColumnTypeTitle(PortfolioColumnType ct) { return GetColumnTypeTitle(ct, null); }
        public static string GetColumnTypeTitle(PortfolioColumnType ct, System.Globalization.CultureInfo culture)
        {
            return YahooLocalizationManager.GetValue("fin_pf_viewColumn_" + ct.ToString(), culture);
        }
    }


    public class PortfolioDataRow
    {
        private PortfolioColumnType[] mAvailableColumns = new PortfolioColumnType[] { };
        private object[] mValues = new object[((int)PortfolioColumnType.comment) + 1];

        public PortfolioColumnType[] AvailableColumns { get { return mAvailableColumns; } }
        public object this[PortfolioColumnType column]
        {
            get
            {
                return mValues[(int)column];
            }
            set
            {
                mValues[(int)column] = value;
            }
        }

        internal PortfolioDataRow()
        {
        }

        internal void SetAvailableColumns()
        {
            List<PortfolioColumnType> lst = new List<PortfolioColumnType>();
            for (int i = 0; i < (int)PortfolioColumnType.comment; i++)
            {
                if (mValues[i] != null) lst.Add((PortfolioColumnType)i);
            }
            mAvailableColumns = lst.ToArray();
        }

    }





    public class PortfolioDownloadSettings : SettingsBase
    {
        public YAccountManager Account { get; set; }
        public string PortfolioID { get; set; }
        public int ViewIndex { get; set; }
        public bool DownloadRealTimeView { get; set; }
        public bool DownloadFundamentalsView { get; set; }

        protected override System.Net.CookieContainer Cookies { get { return this.Account != null ? this.Account.Cookies : null; } }


        public PortfolioDownloadSettings()
        {
            this.ViewIndex = 1;
        }

        protected override string GetUrl()
        {
            string urlEnd = string.Empty;
            if (this.DownloadRealTimeView) { urlEnd = "e"; }
            else if (this.DownloadFundamentalsView) { urlEnd = "fv"; }
            else { urlEnd = "v" + (this.ViewIndex + 1).ToString(); }
            return "http://finance.yahoo.com/portfolio/" + this.PortfolioID + "/view/" + urlEnd;
        }

        public override object Clone()
        {
            return new PortfolioDownloadSettings() { Account = this.Account, PortfolioID = this.PortfolioID, ViewIndex = this.ViewIndex, DownloadFundamentalsView = this.DownloadFundamentalsView, DownloadRealTimeView = this.DownloadRealTimeView };
        }
    }


}
