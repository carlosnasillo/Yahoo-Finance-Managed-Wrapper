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
using System.Collections;


namespace MaasOne.Xml
{


    public partial class XDocument : XContainer
    {
        public XDeclaration Declaration { get; set; }

        public XDocument() { this.Declaration = new XDeclaration("1.0", System.Text.Encoding.UTF8.ToString(), ""); }
        public XDocument(object[] content) { foreach (object obj in content) this.mElements.Add(obj); }
        public XDocument(XDeclaration declaration, object[] content) : this(content) { this.Declaration = declaration; }

        public static MaasOne.Xml.XDocument Load(System.IO.Stream stream) { return Load(MyHelper.StreamToString(stream, System.Text.Encoding.UTF8)); }
        public static MaasOne.Xml.XDocument Load(string text)
        {
            return new XmlParser().ParseDocument(text);
        }

    }

    public class XName
    {
        public string LocalName { get; set; }
        public string NamespaceName { get; set; }

        public XName(string localName)
        {
            this.NamespaceName = string.Empty;
            this.LocalName = localName;
        }
        public XName(string localName, string nameSpaceName)
        {
            this.NamespaceName = nameSpaceName;
            this.LocalName = localName;
        }

        public static XName Get(string name)
        {
            return Get(name, string.Empty);
        }
        public static XName Get(string name, string ns)
        {
            return new XName(name, ns);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is XName && ((XName)obj).LocalName == this.LocalName && ((XName)obj).NamespaceName == this.NamespaceName;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + this.LocalName.GetHashCode();
            hash = (hash * 7) + this.NamespaceName.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return (this.NamespaceName != string.Empty ? this.NamespaceName + ":" : "") + this.LocalName;
        }
    }


    public class XComment
    {
        public string Value { get; set; }

        public XComment(string value)
        {
            this.Value = value;
        }

        internal void GetXml(System.Text.StringBuilder sb, int intendation)
        {
            sb.Append(new string(' ', intendation) + "<!--" + this.Value.Replace("&", "&amp;") + "-->");
        }
    }

    public class XDeclaration
    {
        public string Version { get; set; }
        public string Standalone { get; set; }
        public string Encoding { get; set; }

        public XDeclaration(string version, string encoding, string standalone)
        {
            this.Version = version;
            this.Encoding = encoding;
            this.Standalone = standalone;
        }

        internal void GetXml(System.Text.StringBuilder sb, int intendation)
        {
            sb.Append(new string(' ', intendation) + "<?xml");
            if (this.Version != string.Empty) sb.Append(" version=\"" + this.Version + "\"");
            if (this.Encoding != string.Empty) sb.Append(" encoding=\"" + this.Encoding + "\"");
            if (this.Standalone != string.Empty) sb.Append(" standalone=\"" + this.Standalone + "\"");
            sb.Append("?>");
        }


    }


    public class XObject
    {
        private XElement mParent = null;
        public XElement Parent { get { return mParent; } }
        internal void SetParent(XElement value)
        {
            mParent = value;
        }
        public XObject() { }



    }



    public class XAttribute : XObject
    {
        private XName mName = null;
        public XName Name { get { return mName; } }
        public string Value { get; set; }

        public XAttribute(XName name, string value)
        {
            mName = name;
            this.Value = value;
        }

        internal void GetXml(System.Text.StringBuilder sb)
        {
            sb.Append(" " + this.Name.ToString() + "=\"" + XmlParser.EncodeXml(this.Value) + "\"");
        }

    }



    public abstract class XContainer : XObject
    {
        protected System.ComponentModel.BindingList<object> mElements = new System.ComponentModel.BindingList<object>();

        public bool HasElements { get { return mElements.Count > 0; } }


        internal XContainer()
        {
            this.mElements.ListChanged += this.mElements_Changed;
        }

        private void mElements_Changed(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                if (this is XElement && mElements[e.NewIndex] is XObject) ((XObject)mElements[e.NewIndex]).SetParent((XElement)this);
            }          
        }

        public XElement Element(XName name)
        {
            foreach (object obj in mElements)
            {
                if (obj is XElement && XmlParser.CompareXName(((XElement)obj).Name, name))
                {
                    return (XElement)obj;
                }
            }
            return null;
        }
        public IEnumOfXElement Elements()
        {
            return new IEnumOfXElement(mElements);
        }
        public IEnumOfXElement Elements(XName name)
        {
            return new IEnumOfXElement(mElements, name);
        }


        public void Add(object obj)
        {
            if (obj == null) throw new ArgumentNullException();
            //if (obj is XObject) ((XObject)obj).SetParent(this is XElement ? (XElement)this : null);
            mElements.Add(obj);
        }

        public string InnerXml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            this.GetInnerXml(sb, 0, true);
            return sb.ToString();
        }

        internal void GetInnerXml(System.Text.StringBuilder sb, int intendation, bool isFirst = false)
        {
            if (this is XDocument && ((XDocument)this).Declaration != null) { ((XDocument)this).Declaration.GetXml(sb, intendation); isFirst = false; }
            foreach (object obj in mElements)
            {
                if (obj is XElement)
                {
                    if (!isFirst) sb.AppendLine();
                    ((XElement)obj).GetXml(sb, intendation);
                }
                else if (obj is XComment)
                {
                    if (!isFirst) sb.AppendLine();
                    ((XComment)obj).GetXml(sb, intendation);
                }
                else if (obj is string)
                {
                    if (!isFirst) sb.AppendLine();
                    sb.Append(XmlParser.EncodeXml(obj.ToString()));
                }
                isFirst = false;
            }
        }


        public override string ToString()
        {
            return this.InnerXml();
        }


    }


    public class XElement : XContainer
    {
        private XName mName = null;
        public XName Name { get { return mName; } }
        public string Value
        {
            get
            {
                string res = string.Empty;
                foreach (object obj in mElements)
                {
                    if (obj is XElement)
                    {
                        res += ((XElement)obj).Value;
                    }
                    else if (obj is string)
                    {
                        res += obj.ToString();
                    }
                }
                return res;
            }
        }

        public bool HasAttributes
        {
            get
            {
                foreach (object obj in mElements)
                {
                    if (obj is XAttribute) return true;
                }
                return false;
            }
        }

        public XAttribute Attribute(XName name)
        {
            foreach (object obj in mElements)
            {
                if (obj is XAttribute && XmlParser.CompareXName(((XAttribute)obj).Name, name))
                {
                    return (XAttribute)obj;
                }
            }
            return null;
        }
        public XAttribute[] Attributes()
        {
            List<XAttribute> lst = new List<XAttribute>();
            foreach (object obj in mElements)
            {
                if (obj is XAttribute)
                {
                    lst.Add((XAttribute)obj);
                }
            }
            return lst.ToArray();
        }
        public XAttribute[] Attributes(XName name)
        {
            List<XAttribute> lst = new List<XAttribute>();
            foreach (object obj in mElements)
            {
                if (obj is XAttribute && XmlParser.CompareXName(((XAttribute)obj).Name, name))
                {
                    lst.Add((XAttribute)obj);
                }
            }
            return lst.ToArray();
        }

        public XElement(XName name)
        {
            mName = name;
        }


        public string NodeXml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            this.GetXml(sb, 0);
            return sb.ToString();
        }

        internal void GetXml(System.Text.StringBuilder sb, int intendation)
        {
            sb.Append(new string(' ', intendation) + "<" + this.Name.ToString());
            XAttribute[] atts = (XAttribute[])this.Attributes();
            foreach (XAttribute att in atts)
            {
                att.GetXml(sb);
            }
            if (mElements.Count > 0)
            {
                sb.Append(">");
                this.GetInnerXml(sb, intendation + 4);
                sb.AppendLine();
                sb.Append(new string(' ', intendation) + "</" + this.Name.ToString() + ">");
            }
            else
            {
                if (atts.Length == 0) sb.Append(" ");
                sb.Append("/>");
            }

        }

        public override string ToString()
        {
            return this.NodeXml();
        }


    }


    public class IEnumOfXElement : IEnumerable<XElement>
    {
        private XElement[] mElementsList = new XElement[] { };
        internal System.ComponentModel.BindingList<object> ReferenceList = null;
        internal XName Name = null;


        public virtual XElement this[int index]
        {

            get
            {
                if (index < 0) throw new IndexOutOfRangeException();
                int cnt = 0;
                for (int i = 0; i < this.ReferenceList.Count; i++)
                {
                    if (this.ReferenceList[i] is XElement)
                    {
                        if (cnt == index && (this.Name != null ? XmlParser.CompareXName(((XElement)this.ReferenceList[i]).Name, this.Name) : true))
                            return (XElement)this.ReferenceList[i];
                        cnt++;
                    }
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < 0) throw new IndexOutOfRangeException();
                int cnt = 0;
                for (int i = 0; i < this.ReferenceList.Count; i++)
                {
                    if (this.ReferenceList[i] is XElement)
                    {
                        if (cnt == index && (this.Name != null ? XmlParser.CompareXName(((XElement)this.ReferenceList[i]).Name, this.Name) : true))
                            this.ReferenceList[i] = value;
                        cnt++;
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }

        internal IEnumOfXElement(System.ComponentModel.BindingList<object> refList) : this(refList, null) { }
        internal IEnumOfXElement(System.ComponentModel.BindingList<object> refList, XName name)
        {
            this.ReferenceList = refList;
            this.Name = name;
            this.UpdateRefList();
            this.ReferenceList.ListChanged += this.ReferenceList_Changed;
        }

        public void Remove()
        {
            if (this.ReferenceList.Count > 0)
            {
                for (int i = this.ReferenceList.Count - 1; i >= 0; i--)
                {
                    if (this.ReferenceList[i] is XElement) this.ReferenceList.RemoveAt(i);
                }
            }
        }
        private void ReferenceList_Changed(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            this.UpdateRefList();
        }
        private void UpdateRefList()
        {
            List<XElement> lst = new List<XElement>();
            for (int i = 0; i < this.ReferenceList.Count; i++)
            {
                if (this.ReferenceList[i] is XElement)
                {
                    if (this.Name != null ? XmlParser.CompareXName(((XElement)this.ReferenceList[i]).Name, this.Name) : true)
                        lst.Add((XElement)this.ReferenceList[i]);
                }
            }
            mElementsList = lst.ToArray();
        }

        #region IEnumerable<XElement> Members

        IEnumerator<XElement> IEnumerable<XElement>.GetEnumerator()
        {
            return ((IEnumerable<XElement>)mElementsList).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)mElementsList).GetEnumerator();
        }

        #endregion



    }


}
