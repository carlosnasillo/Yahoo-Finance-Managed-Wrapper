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
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading company information.
    /// </summary>
    /// <remarks></remarks>
    public partial class CompanyInfoDownload : Base.DownloadClient<CompanyInfoResult>
    {


        public CompanyInfoDownloadSettings Settings { get { return (CompanyInfoDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public CompanyInfoDownload()
        {
            this.Settings = new CompanyInfoDownloadSettings();
        }

        public void DownloadAsync(IID managedID, object userArgs)
        {
            if (managedID == null)
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            this.DownloadAsync(managedID.ID, userArgs);
        }
        public void DownloadAsync(string unmanagedID, object userArgs)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.DownloadAsync(new string[] { unmanagedID }, userArgs);
        }
        public void DownloadAsync(IEnumerable<IID> ids, object userArgs)
        {
            if (ids == null)
                throw new ArgumentNullException("ids", "The passed list is null.");
            this.DownloadAsync(FinanceHelper.IIDsToStrings(ids), userArgs);
        }
        public void DownloadAsync(IEnumerable<string> ids, object userArgs)
        {
            if (ids == null)
                throw new ArgumentNullException("ids", "The passed list is null.");
            this.DownloadAsync(new CompanyInfoDownloadSettings(MyHelper.EnumToArray(ids)), userArgs);
        }
        public void DownloadAsync(CompanyInfoDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override CompanyInfoResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<CompanyInfoData> companies = new List<CompanyInfoData>();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] results = XPath.GetElements("//stock", doc);
            foreach (XElement node in results)
            {
                CompanyInfoData stk = new CompanyInfoData();
                string name = MyHelper.GetXmlAttributeValue(node, FinanceHelper.NameCompanySymbol);
                if (name != string.Empty) stk.SetID(name.ToUpper());
                foreach (XElement propertyNode in node.Elements())
                {
                    switch (propertyNode.Name.LocalName)
                    {
                        case FinanceHelper.NameCompanyCompanyName:
                            stk.Name = propertyNode.Value;
                            break;
                        case FinanceHelper.NameCompanyStart:
                            System.DateTime dte1 = default(System.DateTime);
                            if (System.DateTime.TryParse(propertyNode.Value.Replace(FinanceHelper.NameCompanyNotAvailable, "1"), culture, System.Globalization.DateTimeStyles.AssumeUniversal, out dte1))
                                stk.StartDate = dte1;
                            break;
                        case FinanceHelper.NameCompanyEnd:
                            string dateStr = string.Empty;
                            if (propertyNode.Value.IndexOf(FinanceHelper.NameCompanyNotAvailable) > -1)
                            {
                                string[] dates = propertyNode.Value.Split('-');
                                if (dates.Length >= 3)
                                {
                                    if (dates[0].IndexOf(FinanceHelper.NameCompanyNotAvailable) > -1)
                                    {
                                        dateStr += System.DateTime.Now.Year.ToString() + "-";
                                    }
                                    else
                                    {
                                        dateStr += dates[0] + "-";
                                    }
                                    if (dates[1].IndexOf(FinanceHelper.NameCompanyNotAvailable) > -1)
                                    {
                                        dateStr += System.DateTime.Now.Month.ToString() + "-";
                                    }
                                    else
                                    {
                                        dateStr += dates[1] + "-";
                                    }
                                    if (dates[2].IndexOf(FinanceHelper.NameCompanyNotAvailable) > -1)
                                    {
                                        dateStr += System.DateTime.Now.Day.ToString() + "-";
                                    }
                                    else
                                    {
                                        dateStr += dates[2];
                                    }
                                    if (dates.Length > 3)
                                    {
                                        dateStr += "-";
                                        for (int i = 3; i <= dates.Length - 1; i++)
                                        {
                                            dateStr += dates[i] + "-";
                                        }
                                    }
                                }
                                else
                                {
                                    dateStr = propertyNode.Value.Replace(FinanceHelper.NameCompanyNotAvailable, System.DateTime.Now.Month.ToString());
                                }
                            }
                            System.DateTime dte2;
                            if (System.DateTime.TryParse(dateStr, culture, System.Globalization.DateTimeStyles.AssumeUniversal, out dte2))
                                stk.EndDate = dte2;
                            break;
                        case FinanceHelper.NameCompanySector:
                            stk.SectorName = propertyNode.Value;
                            break;
                        case FinanceHelper.NameCompanyIndustry:
                            stk.IndustryName = propertyNode.Value;
                            break;
                        case FinanceHelper.NameCompanyFullTimeEmployees:
                            int i2 = 0;
                            if (int.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Any, culture, out i2))
                                stk.FullTimeEmployees = i2;
                            break;
                    }
                }
                companies.Add(stk);
            }
            return new CompanyInfoResult(companies.ToArray());
        }


    }



    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyInfoResult
    {
        private CompanyInfoData[] mItems = null;
        public CompanyInfoData[] Items
        {
            get { return mItems; }
        }
        internal CompanyInfoResult(CompanyInfoData[] items)
        {
            mItems = items;
        }
    }


    /// <summary>
    /// Stores informations about a company. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class CompanyInfoData : IID, ISettableID
    {
        private string mID = string.Empty;
        /// <summary>
        /// The ID of the company
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return mID; }
        }
        public void SetID(string id)
        {
            mID = id;
        }
        /// <summary>
        /// The name of the company
        /// </summary>
        /// <value></value>
        /// <returns>The name of the company</returns>
        /// <remarks></remarks>
        public string Name { get; set; }
        /// <summary>
        /// The first trading day of the company's stock
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime StartDate { get; set; }
        /// <summary>
        /// The last trading day of the company's stock
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime EndDate { get; set; }
        /// <summary>
        /// The full time employees in this company
        /// </summary>
        /// <value></value>
        /// <returns>The number of employees</returns>
        /// <remarks></remarks>
        public int FullTimeEmployees { get; set; }
        /// <summary>
        /// The name of the sector the company is part of
        /// </summary>
        /// <value></value>
        /// <returns>A sector name</returns>
        /// <remarks></remarks>
        public string SectorName { get; set; }
        /// <summary>
        /// The name of the industry the quote is part of
        /// </summary>
        /// <value></value>
        /// <returns>An industry name</returns>
        /// <remarks></remarks>
        public string IndustryName { get; set; }

    }


    public class CompanyInfoDownloadSettings : Base.SettingsBase
    {

        public string[] IDs { get; set; }

        public CompanyInfoDownloadSettings()
        {
            this.IDs = new string[] { };
        }
        public CompanyInfoDownloadSettings(string id)
        {
            this.IDs = new string[] { id };
        }
        public CompanyInfoDownloadSettings(string[] ids)
        {
            this.IDs = ids;
        }


        protected override string GetUrl()
        {
            if (this.IDs != null && this.IDs.Length > 0)
            {
                System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
                whereClause.Append("symbol in (");
                foreach (string id in this.IDs)
                {
                    whereClause.Append('"');
                    whereClause.Append(MyHelper.CleanYqlParam(id));
                    whereClause.Append("\",");
                }
                whereClause.Remove(whereClause.Length - 1, 1);
                whereClause.Append(')');
                return MyHelper.YqlUrl("*", "yahoo.finance.stocks", whereClause.ToString(), null, false);
            }
            else
            {
                throw new ArgumentException("There must be minimum one ID available.", "IDs");
            }
        }

        public override object Clone()
        {
            return new CompanyInfoDownloadSettings((string[])this.IDs.Clone());
        }
    }
}
