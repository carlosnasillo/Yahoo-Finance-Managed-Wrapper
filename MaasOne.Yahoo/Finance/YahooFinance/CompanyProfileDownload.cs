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
    public partial class CompanyProfileDownload : Base.DownloadClient<CompanyProfileResult>
    {

        public CompanyProfileDownloadSettings Settings { get { return (CompanyProfileDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public CompanyProfileDownload()
        {
            this.Settings = new CompanyProfileDownloadSettings();
        }


        public void DownloadAsync(IID managedID, object userArgs)
        {
            if (managedID == null)
                throw new ArgumentException("The IID is null", "managedID");
            this.DownloadAsync(managedID.ID, userArgs);
        }
        public void DownloadAsync(string unmanagedID, object userArgs)
        {
            if (unmanagedID.Trim() == string.Empty)
                throw new ArgumentException("The ID is empty", "unmanagedID");
            this.DownloadAsync(new CompanyProfileDownloadSettings(unmanagedID), userArgs);
        }

        public void DownloadAsync(CompanyProfileDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override CompanyProfileResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            CompanyProfileData res = null;
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            if (stream != null)
            {
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement resultNode = XPath.GetElement("//table[@id=\"yfncsumtab\"]/tr[2]", doc);

                if (resultNode != null)
                {
                    res = new CompanyProfileData();
                    res.SetID(FinanceHelper.CleanIndexID(((CompanyProfileDownloadSettings)settings).ID.ToUpper()));

                    XElement nameNode = XPath.GetElement("td[1]/b[1]", resultNode);
                    if (nameNode != null)
                    {
                        res.CompanyName = nameNode.Value;
                    }

                    XElement addressNode = XPath.GetElement("td[1]", resultNode);
                    if (addressNode != null)
                    {
                        System.Text.StringBuilder formattedAddress = new System.Text.StringBuilder();
                        try
                        {
                            string addNodeStr = addressNode.ToString();
                            if (addNodeStr != string.Empty)
                            {
                                addNodeStr = addNodeStr.Substring(addNodeStr.IndexOf("/>") + 2);
                                string[] rawAddress = addNodeStr.Substring(0, addNodeStr.IndexOf("Website")).Split(new string[] { "<b>", "<br />", "</b>", "\r", "\n", " - ", "</a>" }, StringSplitOptions.RemoveEmptyEntries);
                                if (rawAddress.Length >= 7)
                                {
                                    foreach (string line in rawAddress)
                                    {
                                        string l = line.Trim();
                                        if (l != string.Empty && !l.StartsWith("<a") && l != "Map")
                                        {
                                            formattedAddress.AppendLine(l);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {                            
                        }
                        res.Address = formattedAddress.ToString().TrimEnd();
                    }

                    XElement indicesNode = XPath.GetElement("td[1]/table[2]/tr/td/table/tr/td[2]", resultNode);
                    if (indicesNode != null)
                    {
                        List<KeyValuePair<string, string>> lstIndices = new List<KeyValuePair<string, string>>();
                        foreach (XElement indexLink in indicesNode.Elements())
                        {
                            if (indexLink.Name.LocalName == "a")
                            {
                                string indexID = Uri.UnescapeDataString(MyHelper.GetXmlAttributeValue(indexLink, "href").ToUpper().Replace("HTTP://FINANCE.YAHOO.COM/Q?S=", "").Replace("&D=T", ""));
                                string name = string.Empty;
                                foreach (string p in indexLink.Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                                {
                                    name += p.Trim() + " ";
                                }
                                lstIndices.Add(new KeyValuePair<string, string>(indexID, name.TrimEnd()));
                            }
                        }
                        res.Details.IndexMembership = lstIndices.ToArray();
                    }

                    XElement sectorsNode = XPath.GetElement("td[1]/table[2]/tr/td/table/tr[2]/td[2]", resultNode);
                    if (sectorsNode != null)
                    {
                        foreach (XElement sectorLink in sectorsNode.Elements())
                        {
                            if (sectorLink.Name.LocalName == "a")
                            {
                                foreach (Sector sect in Enum.GetValues(typeof(Sector)))
                                {
                                    string name = string.Empty;
                                    foreach (string p in sectorLink.Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                                    {
                                        name += p.Trim() + " ";
                                    }
                                    name = name.TrimEnd();
                                    if (sect.ToString().Replace("_", " ") == name)
                                    {
                                        res.Details.Sector = sect;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    XElement industryNode = XPath.GetElement("td[1]/table[2]/tr/td/table/tr[3]/td[2]", resultNode);
                    if (industryNode != null)
                    {
                        foreach (XElement industryLink in industryNode.Elements())
                        {
                            if (industryLink.Name.LocalName == "a")
                            {
                                int indIndex = 0;
                                if (int.TryParse(MyHelper.GetXmlAttributeValue(industryLink, "href").Replace("http://biz.yahoo.com/ic/", "").Replace(".html", ""), out indIndex))
                                {
                                    res.Details.Industry = (Industry)indIndex;
                                }
                            }
                        }
                    }

                    XElement employeesNode = XPath.GetElement("td[1]/table[2]/tr/td/table/tr[4]/td[2]", resultNode);
                    if (employeesNode != null)
                    {
                        int fte;
                        if (int.TryParse(employeesNode.Value.Trim(), System.Globalization.NumberStyles.Any, convCulture, out fte))
                        {
                            res.Details.FullTimeEmployees = fte;
                        }
                    }

                    XElement summaryNode = XPath.GetElement("td[1]/p[1]", resultNode);
                    if (summaryNode != null)
                    {
                        System.Text.StringBuilder summaryText = new System.Text.StringBuilder();
                        foreach (string line in summaryNode.Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                        {
                            summaryText.Append(line.Trim() + " ");
                        }
                        res.BusinessSummary = summaryText.ToString().TrimEnd();
                    }

                    XElement websitesNodes = XPath.GetElement("td[1]/table[5]/tr/td", resultNode);
                    if (websitesNodes != null)
                    {
                        List<Uri> lstWebsites = new List<Uri>();
                        foreach (XElement linkNode in websitesNodes.Elements())
                        {
                            if (linkNode.Name.LocalName == "a")
                            {
                                lstWebsites.Add(new Uri(MyHelper.GetXmlAttributeValue(linkNode, "href")));
                            }
                        }
                        res.CompanyWebsites = lstWebsites.ToArray();
                    }





                    XElement governanceNode = null;
                    XElement governanceHeader = XPath.GetElement("td[3]/table[1]/tr/th/span", resultNode);
                    if (governanceHeader != null && governanceHeader.Value.Contains("Governance"))
                    {
                        governanceNode = XPath.GetElement("td[3]/table[2]/tr/td", resultNode);
                    }
                    if (governanceNode != null)
                    {
                        System.Text.StringBuilder governanceText = new System.Text.StringBuilder();
                        foreach (string line in governanceNode.Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                        {
                            governanceText.Append(line.Trim() + " ");
                        }
                        res.CorporateGovernance = governanceText.ToString().TrimEnd();
                    }



                    XElement executivesNode = null;
                    XElement executivesHeader = XPath.GetElement("td[3]/table[3]/tr/th/span", resultNode);
                    if (executivesHeader != null && executivesHeader.Value.Contains("Executives"))
                    {
                        executivesNode = XPath.GetElement("td[3]/table[4]/tr/td/table", resultNode);
                    }
                    else
                    {
                        executivesNode = XPath.GetElement("td[3]/table[2]/tr/td/table", resultNode);
                    }

                    if (executivesNode != null)
                    {

                        List<ExecutivePersonInfo> lst = new List<ExecutivePersonInfo>();
                        bool isFirst = true;
                        foreach (XElement row in executivesNode.Elements())
                        {
                            if (!isFirst)
                            {
                                if (row.Name.LocalName == "tr")
                                {
                                    XElement[] enm = MyHelper.EnumToArray(row.Elements());
                                    if (enm.Length >= 3)
                                    {
                                        ExecutivePersonInfo exec = new ExecutivePersonInfo();

                                        string name = string.Empty;

                                        foreach (string l in MyHelper.EnumToArray(enm[0].Elements())[0].Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                                        {
                                            name += l.Trim() + " ";
                                        }

                                        exec.Name = name.TrimEnd();

                                        string position = string.Empty;

                                        var enm2 = MyHelper.EnumToArray(enm[0].Elements());
                                        foreach (string l in enm2[enm2.Length - 1].Value.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                                        {
                                            position += l.Trim() + " ";
                                        }

                                        exec.Position = position.Trim();


                                        string payStr = enm[1].Value.Replace("\r\n", "").Trim();
                                        if (!payStr.Contains("N/A"))
                                        {
                                            exec.Pay = FinanceHelper.GetMillionValue(payStr) * 1000000;
                                        }

                                        string exercisedStr = enm[2].Value.Replace("\r\n", "").Trim();
                                        if (!exercisedStr.Contains("N/A"))
                                        {
                                            double d = FinanceHelper.GetMillionValue(exercisedStr);
                                            exec.Exercised = (int)(d * 1000000);
                                        }

                                        lst.Add(exec);
                                    }
                                }
                            }
                            else
                            {
                                isFirst = false;
                            }
                        }
                        res.KeyExecutives = lst.ToArray();
                    }
                    if (res.BusinessSummary.StartsWith("There is no ")) { res = null; }
                }

            }

            return new CompanyProfileResult(res);
        }


    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyProfileResult
    {
        private CompanyProfileData mItem = null;
        public CompanyProfileData Item
        {
            get { return mItem; }
        }

        internal CompanyProfileResult(CompanyProfileData item)
        {
            mItem = item;
        }
    }

    public class CompanyProfileData : ISettableID
    {
        private string mID = string.Empty;
        public string ID
        {
            get { return mID; }
        }
        public void SetID(string id)
        {
            mID = id;
        }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string BusinessSummary { get; set; }
        public string CorporateGovernance { get; set; }
        public Uri[] CompanyWebsites { get; set; }
        public CompanyDetails Details { get; set; }
        public ExecutivePersonInfo[] KeyExecutives { get; set; }

        public CompanyProfileData()
        {
            this.CompanyWebsites = new Uri[] { };
            this.Details = new CompanyDetails();
            this.KeyExecutives = new ExecutivePersonInfo[] { };
        }
    }

    public class CompanyDetails
    {
        public KeyValuePair<string, string>[] IndexMembership { get; set; }
        public Nullable<Sector> Sector { get; set; }
        public Nullable<Industry> Industry { get; set; }
        public int FullTimeEmployees { get; set; }

        public CompanyDetails()
        {
            this.IndexMembership = new KeyValuePair<string, string>[] { };
        }
    }

    public class ExecutivePersonInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public double Pay { get; set; }
        public int Exercised { get; set; }
    }


    public class CompanyProfileDownloadSettings : Base.SettingsBase
    {

        public string ID { get; set; }

        public CompanyProfileDownloadSettings()
        {
            this.ID = string.Empty;
        }
        public CompanyProfileDownloadSettings(string id)
        {
            this.ID = id;
        }

        protected override string GetUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            return string.Format("http://finance.yahoo.com/q/pr?s={0}", this.ID);
        }

        public override object Clone()
        {
            return new CompanyProfileDownloadSettings(this.ID);
        }
    }

}
