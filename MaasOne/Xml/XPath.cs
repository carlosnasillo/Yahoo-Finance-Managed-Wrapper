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
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace MaasOne.Xml
{

    public class XPath
    {
        public bool IsRootPath { get { return mCheckChildren && mValue == String.Empty; } }
        public XPath Child { get { return mChild; } }
        public bool RegexAttributeValue { get; set; }

        private string mValue = string.Empty;
        private string mName = string.Empty;
        private TokenExtensionType mExtensionType = TokenExtensionType.None;
        private bool mCheckChildren = false;
        private int mIndex = -1;
        private string mAttributeTag = string.Empty;
        private string mAttributeValue = string.Empty;
        private XPath mChild = null;

        public XPath(string value, bool regex) : this(value) { this.RegexAttributeValue = regex; }
        public XPath(string value) { this.SetToken(this.GetTokenArray(value)); }
        private XPath(string[] tokenValues) { this.SetToken(tokenValues); }
        
        private void SetToken(string[] tokenValues)
        {
            if (tokenValues == null || tokenValues.Length == 0) throw new ArgumentException();


            mValue = tokenValues[0];
            if (mValue.StartsWith("/"))
            {
                mValue = mValue.Substring(1);
                mCheckChildren = true;
            }


            if (mValue != string.Empty)
            {
                bool isInQuote = false;
                bool isInNodeName = true;
                bool isInAttributeName = false;
                bool isInAttributeValue = false;
                for (int i = 0; i < mValue.Length; i++)
                {
                    if (mValue[i] == '\"') { isInQuote = !isInQuote; continue; }

                    if (!isInQuote)
                    {
                        switch (mValue[i])
                        {
                            case '[':
                                isInNodeName = false;
                                isInAttributeName = true;
                                break;
                            case ']':
                                isInAttributeName = false;
                                isInAttributeValue = false;
                                if (mExtensionType != TokenExtensionType.AttributeID)
                                {
                                    if (int.TryParse(mAttributeTag, out mIndex))
                                    {
                                        mExtensionType = TokenExtensionType.Index;
                                    }
                                    mAttributeTag = string.Empty;
                                }
                                break;
                            case '@':
                                mExtensionType = TokenExtensionType.AttributeID;
                                break;
                            case '=':
                                if (isInAttributeName)
                                {
                                    isInAttributeName = false;
                                    isInAttributeValue = true;
                                }
                                break;
                            default:
                                if (isInNodeName) { mName += mValue[i]; }
                                else if (isInAttributeName) { mAttributeTag += mValue[i]; }
                                break;
                        }
                    }
                    else if (isInAttributeValue)
                    {
                        mAttributeValue += mValue[i];
                    }
                }
            }


            if (tokenValues.Length > 1)
            {
                string[] newVals = new string[tokenValues.Length - 1];
                for (int i = 1; i < tokenValues.Length; i++)
                {
                    newVals[i - 1] = tokenValues[i];
                }
                mChild = new XPath(newVals);
            }
        }
        private string[] GetTokenArray(string value)
        {
            List<string> parts = new List<string>();
            bool isInQuotes = false;
            string tempText = value.Trim();
            for (int i = 0; i < tempText.Length; i++)
            {
                if (!isInQuotes)
                {
                    if (tempText[i] == '/')
                    {
                        parts.Add(tempText.Substring(0, i));
                        tempText = tempText.Substring(i + 1);
                        i = -1;
                    }
                    else if (tempText[i] == '\"')
                    {
                        isInQuotes = true;
                    }
                }
                else
                {
                    if (tempText[i] == '\"')
                    {
                        isInQuotes = false;
                    }
                }
            }
            if (tempText != string.Empty) parts.Add(tempText);
            string[] values = parts.ToArray();
            if (values.Length > 0)
            {
                string[] res = values;
                bool isRootPath = false;

                int childTokenCount = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == string.Empty) { childTokenCount++; }
                    else { break; }
                }
                if (childTokenCount > 2) throw new ArgumentException("The token is invalid, because of more than two child token at start.");
                isRootPath = childTokenCount == 2;
                if (isRootPath && values.Length == 2) throw new ArgumentException("There exists only the root token.");
                List<string> lst = new List<string>();
                if (isRootPath) lst.Add("/");
                for (int i = childTokenCount; i < values.Length; i++)
                {
                    if (i == 0 && childTokenCount == 0)
                    {
                        lst.Add(values[i]);
                    }
                    else
                    {
                        lst.Add('/' + values[i]);
                    }
                }
                res = lst.ToArray();
                return res;
            }
            else
            {
                throw new ArgumentException("The XPath token is empty.", "value");
            }
        }

        public XElement GetElement(XContainer container)
        {
            XElement[] nodes = this.GetElements(container, true);
            if (nodes.Length == 0) { return null; }
            else { return nodes[0]; }
        }
        public XElement[] GetElements(XContainer container)
        {
            return this.GetElements(container, false);
        }
        private XElement[] GetElements(XContainer container, bool returnFirstResult, bool isRootSeek = false)
        {
            List<XElement> matchNodes = new List<XElement>();


            if (this.IsRootPath)
            {
                if (container is XElement && ((XElement)container).Name.LocalName == mChild.mName)
                {
                    if (mChild.mExtensionType == TokenExtensionType.None)
                    {
                        matchNodes.Add((XElement)container);
                    }
                    else if (mChild.mExtensionType == TokenExtensionType.AttributeID)
                    {
                        XAttribute att = ((XElement)container).Attribute(XName.Get(mChild.mAttributeTag));
                        if (att != null && this.StringValuesEquals(mChild.mAttributeValue, att.Value)) { matchNodes.Add((XElement)container); }
                    }
                }

                if (!(returnFirstResult && matchNodes.Count > 0))
                {
                    foreach (XElement elem in container.Elements())
                    {
                        matchNodes.AddRange(this.GetElements(elem, returnFirstResult, true));
                        if (returnFirstResult && matchNodes.Count > 0) break;
                    }
                }
            }
            else
            {
                int cnt = 0;
                foreach (XElement elem in container.Elements())
                {
                    if (elem.Name.LocalName == mName)
                    {
                        cnt++;
                        switch (mExtensionType)
                        {
                            case TokenExtensionType.None:
                                matchNodes.Add(elem);
                                break;
                            case TokenExtensionType.Index:
                                if (cnt == mIndex) matchNodes.Add(elem);
                                break;
                            case TokenExtensionType.AttributeID:
                                XAttribute att = elem.Attribute(XName.Get(mAttributeTag));
                                if (att != null && this.StringValuesEquals(mAttributeValue, att.Value)) matchNodes.Add(elem);
                                break;
                        }
                        if (returnFirstResult && matchNodes.Count > 0) break;
                    }
                }
            }



            List<XElement> results = new List<XElement>();

            XPath child = mChild;
            if (this.IsRootPath) child = child.Child;
            if (!isRootSeek && child != null)
            {
                foreach (XElement match in matchNodes)
                {
                    results.AddRange(child.GetElements(match, returnFirstResult));
                    if (returnFirstResult && results.Count > 0) break;
                }
            }
            else
            {
                results.AddRange(matchNodes);
            }

            return results.ToArray();
        }

        private bool StringValuesEquals(string pattern, string text)
        {
            if (this.RegexAttributeValue)
            {
                return Regex.Match(text, pattern).Success;
            }
            else
            {
                return pattern == text;
            }
        }

        public override string ToString() { return (mCheckChildren ? "/" : "") + mValue + (mChild != null ? mChild.ToString() : ""); }

        

        public static XElement GetElement(string xpath, XContainer conatiner, bool regexAttributes = false)
        {
            return (new XPath(xpath) { RegexAttributeValue = regexAttributes }).GetElement(conatiner);
        }
        public static XElement[] GetElements(string xpath, XContainer conatiner, bool regexAttributes = false)
        {
            return (new XPath(xpath) { RegexAttributeValue = regexAttributes }).GetElements(conatiner);
        }


        private enum TokenExtensionType
        {
            None,
            AttributeID,
            Index
        }


    }

}
