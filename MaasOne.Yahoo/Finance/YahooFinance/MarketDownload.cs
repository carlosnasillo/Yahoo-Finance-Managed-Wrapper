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
using MaasOne.Xml;
using System.Threading;
using System.ComponentModel;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading sectors, industries and company IDs.
    /// </summary>
    /// <remarks></remarks>
    public partial class MarketDownload : Base.DownloadClient<MarketResult>
    {

        private System.Globalization.CultureInfo mDefaultCulture = new System.Globalization.CultureInfo("en-US");
        /// <summary>
        /// Raises if an asynchronous download of sectors completes.
        /// </summary>
        /// <param name="sender">The event raising object</param>
        /// <param name="ea">The event args of the asynchronous download</param>
        /// <remarks></remarks>
        public event AsyncSectorsDownloadCompletedEventHandler AsyncSectorsDownloadCompleted;
        public delegate void AsyncSectorsDownloadCompletedEventHandler(Base.DownloadClient<MarketResult> sender, SectorsDownloadCompletedEventArgs ea);
        /// <summary>
        /// Raises if an asynchronous download of industries completes.
        /// </summary>
        /// <param name="sender">The event raising object</param>
        /// <param name="ea">The event args of the asynchronous download</param>
        /// <remarks></remarks>
        public event AsyncIndustriesDownloadCompletedEventHandler AsyncIndustriesDownloadCompleted;
        public delegate void AsyncIndustriesDownloadCompletedEventHandler(Base.DownloadClient<MarketResult> sender, IndustryDownloadCompletedEventArgs ea);

        public MarketDownloadSettings Settings { get { return (MarketDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public MarketDownload()
        {
            this.Settings = new MarketDownloadSettings();
        }

        /// <summary>
        /// Starts an asynchronous download of all available sectors and its industry IDs.
        /// </summary>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAllSectorsAsync(object userArgs)
        {
            this.DownloadAsync(new MarketDownloadSettings() { Sectors = new Sector[] { } }, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of sectors and its industry IDs with the passed names
        /// </summary>
        /// <param name="sectors">The names of the sectors</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadSectorsAsync(IEnumerable<Sector> sectors, object userArgs)
        {
            this.DownloadAsync(new MarketDownloadSettings() { Sectors = MyHelper.EnumToArray(sectors) }, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of industries with passed IndustryData
        /// </summary>
        /// <param name="industries">The industries</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>m
        public void DownloadIndustriesAsync(IEnumerable<IndustryData> industries, object userArgs)
        {
            List<Industry> lst = new List<Industry>();
            if (industries != null)
            {
                foreach (IndustryData ind in industries)
                {
                    lst.Add(ind.ID);
                }
            }
            this.DownloadIndustriesAsync(lst.ToArray(), userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of industries with passed IDs
        /// </summary>
        /// <param name="industryIDs">The IDs of the industries</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadIndustriesAsync(IEnumerable<Industry> industryIDs, object userArgs)
        {
            this.DownloadAsync(new MarketDownloadSettings() { Industries = (MyHelper.EnumToArray(industryIDs)) }, userArgs);
        }

        public void DownloadAsync(MarketDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override Base.DownloadCompletedEventArgs<MarketResult> ConvertDownloadCompletedEventArgs(Base.DefaultDownloadCompletedEventArgs<MarketResult> e)
        {
            MarketDownloadSettings set = (MarketDownloadSettings)e.Settings;
            if (set.Sectors != null)
            {
                SectorsDownloadCompletedEventArgs args = new SectorsDownloadCompletedEventArgs(e.UserArgs, (SectorResponse)e.Response, set);
                if (AsyncSectorsDownloadCompleted != null)
                {
                    AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(this);
                    asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncSectorsDownloadCompleted(this, (SectorsDownloadCompletedEventArgs)obj); }), args);
                }
                return args;

            }
            else if (set.Industries != null)
            {
                IndustryDownloadCompletedEventArgs args = new IndustryDownloadCompletedEventArgs(e.UserArgs, (IndustryResponse)e.Response, set);
                if (AsyncIndustriesDownloadCompleted != null)
                {
                    AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(this);
                    asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncIndustriesDownloadCompleted(this, (IndustryDownloadCompletedEventArgs)obj); }), args);                   
                }
                return args;
            }
            else
            {
                return null;
            }
        }
        protected override Base.Response<MarketResult> ConvertResponse(Base.DefaultResponse<MarketResult> response)
        {
            if (response.Result is SectorResult)
            {
                return new SectorResponse(response.Connection, (SectorResult)response.Result);
            }
            else if (response.Result is IndustryResult)
            {
                return new IndustryResponse(response.Connection, (IndustryResult)response.Result);
            }
            else
            {
                return null;
            }
        }
        protected override MarketResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            MarketDownloadSettings set = (MarketDownloadSettings)settings;
            if (set.Sectors != null)
            {
                List<SectorData> sectors = new List<SectorData>();
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement[] results = XPath.GetElements("//sector",doc);
                foreach (XElement node in results)
                {

                    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                    if (culture != null)
                        ci = culture;
                    SectorData sect = new SectorData();
                    string nameAtt = MyHelper.GetXmlAttributeValue(node, FinanceHelper.NameMarketName);
                    if (nameAtt != string.Empty)
                    {
                        for (Sector s = 0; s <= Sector.Utilities; s++)
                        {
                            if (s.ToString().Replace("_", " ") == nameAtt)
                            {
                                sect.ID = s;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                    foreach (XElement industryNode in node.Elements())
                    {
                        if (industryNode.Name.LocalName == "industry")
                        {
                            IndustryData ind = new IndustryData();
                            foreach (XAttribute att in industryNode.Attributes())
                            {
                                if (att.Name.LocalName == FinanceHelper.NameIndustryID)
                                {
                                    int i = 0;
                                    int.TryParse(att.Value, out i);
                                    if (i != 0)
                                    {
                                        ind.ID = (Industry)i;
                                    }
                                }
                                else if (att.Name.LocalName == FinanceHelper.NameMarketName)
                                {
                                    ind.Name = att.Value;
                                }
                            }
                            sect.Industries.Add(ind);
                        }
                    }
                    sectors.Add(sect);
                }
                return new SectorResult(sectors.ToArray());
            }
            else if (set.Industries != null)
            {
                List<IndustryData> industries = new List<IndustryData>();
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement[] results = XPath.GetElements("//industry",doc);
                foreach (XElement node in results)
                {

                    IndustryData ind = new IndustryData();
                    foreach (XAttribute att in node.Attributes())
                    {
                        if (att.Name.LocalName == FinanceHelper.NameIndustryID)
                        {
                            int i = 0;
                            int.TryParse(att.Value, out i);
                            if (i != 0)
                            {
                                ind.ID = (Industry)i;
                            }
                        }
                        else if (att.Name.LocalName == FinanceHelper.NameMarketName)
                        {
                            ind.Name = att.Value;
                        }
                    }
                    foreach (XElement companyNode in node.Elements())
                    {
                        if (companyNode.Name.LocalName == "company")
                        {
                            CompanyInfoData comp = new CompanyInfoData();
                            foreach (XAttribute att in companyNode.Attributes())
                            {
                                if (att.Name.LocalName == FinanceHelper.NameCompanySymbol)
                                {
                                    comp.SetID(att.Value);
                                }
                                else if (att.Name.LocalName == FinanceHelper.NameMarketName)
                                {
                                    comp.Name = att.Value;
                                }
                            }
                            ind.Companies.Add(comp);
                        }
                    }
                    industries.Add(ind);
                }
                return new IndustryResult(industries.ToArray());
            }
            else
            {
                return null;
            }
        }

    }


    /// <summary>
    /// Base container class for market result data
    /// </summary>
    public abstract class MarketResult
    {


    }




    /// <summary>
    /// Provides information and response of an asynchronous sector download.
    /// </summary>
    /// <remarks></remarks>
    public class SectorsDownloadCompletedEventArgs : Base.DownloadCompletedEventArgs<MarketResult>
    {
        /// <summary>
        /// Gets the response with sector information.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SectorResponse Response
        {
            get { return (SectorResponse)base.Response; }
        }
        public MarketDownloadSettings Settings { get { return (MarketDownloadSettings)base.Settings; } }

        internal SectorsDownloadCompletedEventArgs(object userArgs, SectorResponse resp, MarketDownloadSettings settings)
            : base(userArgs, resp, settings)
        {
        }
    }

    /// <summary>
    /// Provides connection information and sector information.
    /// </summary>
    /// <remarks></remarks>
    public class SectorResponse : Base.Response<MarketResult>
    {
        /// <summary>
        /// Gets the received sector informations.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SectorResult Result
        {
            get { return (SectorResult)base.Result; }
        }
        internal SectorResponse(Base.ConnectionInfo info, SectorResult result)
            : base(info, result)
        {
        }
    }




    /// <summary>
    /// Stores the result data
    /// </summary>
    public class SectorResult : MarketResult
    {

        private SectorData[] mItems = null;
        public SectorData[] Items
        {
            get { return mItems; }
        }
        internal SectorResult(SectorData[] items)
        {
            mItems = items;
        }

    }


    /// <summary>
    /// Stores informations of a sector
    /// </summary>
    /// <remarks></remarks>
    public class SectorData
    {
        private string mName = string.Empty;
        private Sector mID;

        private List<IndustryData> mIndustries = new List<IndustryData>();
        /// <summary>
        /// The ID of the sector
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Sector ID
        {
            get { return mID; }
            set
            {
                mID = value;
                mName = mID.ToString().Replace("_", " ");
            }
        }
        /// <summary>
        /// The name of the sector
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }
        /// <summary>
        /// The industries of the sector
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<IndustryData> Industries
        {
            get { return mIndustries; }
            set { mIndustries = value; }
        }


        public static string GetSectorName(Sector sect)
        {
            return YahooLocalizationManager.GetValue("fin_yf_sector_" + sect.ToString());
        }

    }





    /// <summary>
    /// Provides information and response of an asynchronous industry download.
    /// </summary>
    /// <remarks></remarks>
    public class IndustryDownloadCompletedEventArgs : Base.DownloadCompletedEventArgs<MarketResult>
    {
        /// <summary>
        /// Gets the response with industry information
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IndustryResponse Response
        {
            get { return (IndustryResponse)base.Response; }
        }
        public MarketDownloadSettings Settings { get { return (MarketDownloadSettings)base.Settings; } }

        internal IndustryDownloadCompletedEventArgs(object userArgs, IndustryResponse resp, MarketDownloadSettings settings)
            : base(userArgs, resp, settings)
        {
        }
    }

    /// <summary>
    /// Provides connection information and industry information.
    /// </summary>
    /// <remarks></remarks>
    public class IndustryResponse : Base.Response<MarketResult>
    {
        /// <summary>
        /// Gets the received industry informations.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IndustryResult Result { get { return (IndustryResult)base.Result; } }
        internal IndustryResponse(Base.ConnectionInfo info, IndustryResult result)
            : base(info, result)
        {
        }
    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class IndustryResult : MarketResult
    {
        private IndustryData[] mItems = null;
        public IndustryData[] Items
        {
            get { return mItems; }
        }
        internal IndustryResult(IndustryData[] items)
        {
            mItems = items;
        }
    }

    /// <summary>
    /// Stores informations of an industry
    /// </summary>
    /// <remarks></remarks>
    public class IndustryData : MarketResult
    {

        /// <summary>
        /// The name of the industry
        /// </summary>
        /// <value></value>
        /// <returns>The name of the industry</returns>
        /// <remarks></remarks>
        public string Name { get; set; }
        /// <summary>
        /// The Yahoo ID of the industry
        /// </summary>
        /// <value></value>
        /// <returns>An ID string</returns>
        /// <remarks></remarks>
        public Industry ID { get; set; }
        /// <summary>
        /// The companies of the industry
        /// </summary>
        /// <value></value>
        /// <returns>A list of companies</returns>
        /// <remarks></remarks>
        public List<CompanyInfoData> Companies { get; set; }

        public IndustryData()
        {
            this.Companies = new List<CompanyInfoData>();
        }


        public static string GetIndustryName(Industry ind)
        {
            return YahooLocalizationManager.GetValue("fin_yf_industry_" + ind.ToString());
        }


    }




    public class MarketDownloadSettings : Base.SettingsBase
    {
        private Sector[] mSectors;
        /// <summary>
        /// Gets the downloaded sectors.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Sector[] Sectors
        {
            get { return mSectors; }
            set
            {
                mSectors = value;
                mIndustries = null;
            }
        }
        private Industry[] mIndustries;
        /// <summary>
        /// Gets the IDs of the downloaded industries.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Industry[] Industries
        {
            get { return mIndustries; }
            set
            {
                mIndustries = value;
                mSectors = null;
            }
        }

        internal MarketDownloadType Type
        {
            get
            {
                if (mSectors != null && mIndustries == null)
                {
                    return MarketDownloadType.Sectors;
                }
                else if (mIndustries != null && mSectors == null)
                {
                    return MarketDownloadType.Industry;
                }
                else
                {
                    return MarketDownloadType.AllSectors;
                }
            }
        }

        protected override string GetUrl()
        {
            switch (this.Type)
            {
                case MarketDownloadType.AllSectors:
                    return this.DownloadURLSector(new Sector[] { });
                case MarketDownloadType.Sectors:
                    return this.DownloadURLSector(mSectors);
                case MarketDownloadType.Industry:
                    return this.DownloadURLIndustry(mIndustries);
                default:
                    return string.Empty;
            }
        }

        private string DownloadURLSector(Sector[] names)
        {
            System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
            if (names != null && names.Length > 0)
            {
                whereClause.Append("name in (");
                foreach (Sector sector in names)
                {
                    whereClause.Append('"');
                    whereClause.Append(sector.ToString().Replace("_", " "));
                    whereClause.Append("\",");
                }
                whereClause.Remove(whereClause.Length - 1, 1);
                whereClause.Append(')');
            }
            return MyHelper.YqlUrl("*", "yahoo.finance.sectors", whereClause.ToString(), null, false);
        }
        private string DownloadURLIndustry(Industry[] indutryIDs)
        {
            if (indutryIDs != null && indutryIDs.Length > 0)
            {
                System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
                whereClause.Append("id in (");
                foreach (Industry id in indutryIDs)
                {
                    whereClause.Append('"');
                    whereClause.Append(Convert.ToInt32(id).ToString());
                    whereClause.Append("\",");
                }
                whereClause.Remove(whereClause.Length - 1, 1);
                whereClause.Append(')');
                return MyHelper.YqlUrl("*", "yahoo.finance.industry", whereClause.ToString(), null, false);
            }
            else
            {
                return string.Empty;
            }
        }

        public override object Clone()
        {
            MarketDownloadSettings cln = new MarketDownloadSettings();
            if (this.Sectors != null) { cln.Sectors = this.Sectors; }
            if (this.Industries != null) { cln.Industries = this.Industries; }
            return cln;
        }


    }

    internal enum MarketDownloadType
    {
        AllSectors,
        Sectors,
        Industry
    }


}
