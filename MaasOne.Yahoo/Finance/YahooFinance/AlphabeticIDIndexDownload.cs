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
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{

    /// <summary>
    /// Provides methods for downloading Alphabetic Index for ID Search
    /// </summary>
    public partial class AlphabeticIDIndexDownload : Base.DownloadClient<AlphabeticIDIndexResult>
    {

        public AlphabeticIDIndexSettings Settings { get { return (AlphabeticIDIndexSettings)base.Settings; } set { base.SetSettings(value); } }

        public AlphabeticIDIndexDownload()
        {
            this.Settings = new AlphabeticIDIndexSettings();
        }

        public void DownloadTopIndexAsync(object userArgs)
        {
            this.DownloadAsync(new AlphabeticIDIndexSettings(), null);
        }
        public void DownloadIndexAsync(AlphabeticalTopIndex topIndex, object userArgs)
        {
            this.DownloadAsync(new AlphabeticIDIndexSettings() { TopIndex = topIndex }, null);
        }
      

        public void DownloadAsync(AlphabeticIDIndexSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }
        
       
        protected override AlphabeticIDIndexResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            AlphabeticIDIndexSettings s = (AlphabeticIDIndexSettings)settings;
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            XDocument doc = MyHelper.ParseXmlDocument(stream);
           XElement[] resultsNodes = null;

            switch (s.Type)
            {
                case AlphabeticalIndexDownloadType.TopIndex:
                    resultsNodes = XPath.GetElements("//results",doc);
                    List<AlphabeticalTopIndex> lstTopIndex = new List<AlphabeticalTopIndex>();

                    if (resultsNodes.Length > 0)
                    {
                        XElement resultNode = resultsNodes[0];

                        foreach (XElement tdNode in resultNode.Elements())
                        {
                            foreach (XElement aNode in tdNode.Elements())
                            {
                                if (aNode.Name.LocalName == "a")
                                {
                                    string att = MyHelper.GetXmlAttributeValue(aNode, "href");
                                    if (att != string.Empty)
                                    {
                                        lstTopIndex.Add(new AlphabeticalTopIndex(aNode.Value, "http://biz.yahoo.com" + att));
                                    }
                                }
                            }
                        }

                    }

                    return new AlphabeticIDIndexResult(lstTopIndex.ToArray());



                case AlphabeticalIndexDownloadType.Index:
                    resultsNodes = XPath.GetElements("//results",doc);
               List<AlphabeticalIndex> lstIndex = new List<AlphabeticalIndex>();

                    if (resultsNodes.Length > 0)
                    {
                        XElement resultNode = resultsNodes[0];

                        foreach (XElement tdNode in resultNode.Elements())
                        {
                            foreach (XElement tableNode in tdNode.Elements())
                            {
                                if (tableNode.Name.LocalName == "table")
                                {
                                    XElement[] chdLst = MyHelper.EnumToArray(MyHelper.EnumToArray(tableNode.Elements())[0].Elements());
                                    if (chdLst.Length >= 3)
                                    {
                                        lstIndex.Add(new AlphabeticalIndex(chdLst[1].Value, s.TopIndex.URL));
                                        if (chdLst.Length > 3)
                                        {
                                            for (int i = 3; i <= chdLst.Length - 1; i++)
                                            {
                                                XElement tdTbNode = chdLst[i];
                                                XElement[] enm = MyHelper.EnumToArray(tdTbNode.Elements());
                                                if (enm.Length > 0 && enm[0].Name.LocalName == "a")
                                                {

                                                    string name = enm[0].Value.Replace("\n", "").Replace(" ", "").Trim();
                                                    string url = string.Empty;
                                                    string att = MyHelper.GetXmlAttributeValue(enm[0], "href");
                                                    if (att != string.Empty)
                                                    {
                                                        url = "http://biz.yahoo.com" + att.Trim();
                                                    }
                                                    if (url != string.Empty & name != string.Empty)
                                                    {
                                                        lstIndex.Add(new AlphabeticalIndex(name, url));
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }
                    s.TopIndex.SetIndices(lstIndex.ToArray());
                    return new AlphabeticIDIndexResult(s.TopIndex.SubIndices);
                default:
                    return null;
            }
        }





    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class AlphabeticIDIndexResult
    {

        private AlphabeticalIndex[] mItems = null;
        public AlphabeticalIndex[] Items
        {
            get { return mItems; }
        }

        public AlphabeticIDIndexResult(AlphabeticalIndex[] items)
        {
            mItems = items;
        }

    }

    public class AlphabeticalIndex
    {

        private string mIndex = string.Empty;
        public string Index
        {
            get { return mIndex; }
        }
        private string mURL = string.Empty;
        internal string URL
        {
            get { return mURL; }
        }

        internal AlphabeticalIndex(string i, string url)
        {
            mIndex = i;
            mURL = url;
        }

        public override string ToString()
        {
            return this.Index;
        }
    }

    public class AlphabeticalTopIndex : AlphabeticalIndex
    {

        private AlphabeticalIndex[] mSubIndices = new AlphabeticalIndex[] { };
        public AlphabeticalIndex[] SubIndices
        {
            get { return mSubIndices; }
        }
        internal void SetIndices(AlphabeticalIndex[] ind)
        {
            mSubIndices = ind;
        }

        internal AlphabeticalTopIndex(string i, string url)
            : base(i, url)
        {
        }

    }


    public class AlphabeticIDIndexSettings : Base.SettingsBase
    {
        private AlphabeticalTopIndex mTopIndex = null;
        /// <summary>
        /// NULL or TopIndex [e.g. '0-9', 'A', 'B', etc.]. If NULL it downloads all TopIndices. If has value it downloads the sub-indices of the TopIndex.
        /// </summary>
        public AlphabeticalTopIndex TopIndex
        {
            get
            {
                return mTopIndex;
            }
            set
            {
                mTopIndex = value;
            }
        }
        internal AlphabeticalIndexDownloadType Type
        {
            get
            {
                if (mTopIndex != null)
                {
                    return AlphabeticalIndexDownloadType.Index;
                }
                else
                {
                    return AlphabeticalIndexDownloadType.TopIndex;
                }
            }
        }

        protected override string GetUrl()
        {
            string whereClause = string.Empty;
            if (mTopIndex != null)
            {
                 whereClause = string.Format("url='{0}' AND xpath='//html/body/center[3]/table[3]/tr/td'", mTopIndex.URL);
           }
            else
            {
                whereClause = "url='http://biz.yahoo.com/i/' AND xpath='//html/body/center[3]/table[2]/tr/td'";
            }
            return MyHelper.YqlUrl("*", "html", whereClause, null, false);
        }

        public override object Clone()
        {
            AlphabeticIDIndexSettings cln = new AlphabeticIDIndexSettings();
            if (this.TopIndex != null) cln.TopIndex = this.TopIndex;
            return cln;
        }


    }

    internal enum AlphabeticalIndexDownloadType
    {
        TopIndex,
        Index,
        ID
    }



}
