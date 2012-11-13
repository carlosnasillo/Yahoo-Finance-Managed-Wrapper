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
using System.Xml.Linq;


namespace MaasOne.Xml
{

    internal sealed class XmlParser
    {
        internal static bool CompareXName(XName x1, XName x2) { return x1.ToString() == x2.ToString(); }

        public static XDocument Parse(string text) { return new XmlParser().ParseDocument(text); }
        public XDocument ParseDocument(string text)
        {
            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            XDocument doc = new XDocument();
            try
            {
                StringCounter sc = new StringCounter(text);
                sc.TrimStart();
                if (sc.StartsWith("<"))
                {
                    while (!sc.IsAtEnd)
                    {
                        XContainer childNode = this.ParseNode(sc, doc);
                    }
                    XElement[] enm = MyHelper.EnumToArray(doc.Elements());
                    if (enm.Length > 1)
                    {
                        XElement root = new XElement(XName.Get("root"));
                        foreach (XElement elem in enm)
                        {
                            root.Add(elem);
                        }
                        doc.Elements().Remove();
                        doc.Add(root);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            //sw.Stop();
            //System.Diagnostics.Debug.WriteLine(sw.Elapsed.TotalMilliseconds.ToString("N0"));
            return doc;
        }

        private XName ParseName(StringCounter sc)
        {
            sc.TrimStart();
            XName name = null;
            string n = string.Empty;
            string ns = string.Empty;
            for (int i = sc.Index; i < sc.Value.Length; i++)
            {
                if (IsValidTagChar(sc.Value[i]))
                {
                    n += sc.Value[i];
                }
                else if (n == string.Empty && sc.Value[i] == ' ')
                {
                }
                else if (i > 0)
                {
                    if (sc.Value[i] == ':' && n != string.Empty && ns == string.Empty)
                    {
                        ns = n;
                        n = string.Empty;
                    }
                    else if (sc.Value[i] == ' ' || sc.Value[i] == '=' || sc.Value[i] == '>' || sc.Value[i] == '/')
                    {
                        sc.Index = i;
                        break;
                    }
                    else
                    {
                        n = string.Empty;
                        sc.Index = i;
                        break;
                    }
                }
                else
                {
                    throw new Exception("Invalid Name.");
                }
            }
            if (n != string.Empty) name = XName.Get(n.Replace("h1", "hx").Replace("h2", "hx"), ns);
            sc.TrimStart();
            return name;
        }
        private XAttribute ParseAttribute(StringCounter sc)
        {
            sc.TrimStart();
            XAttribute att = null;
            XName name = null;
            if (IsValidTagChar(sc.StartValue))
            {
                name = ParseName(sc);
            }
            if (name != null && sc.StartValue == '=')
            {
                sc.Index++;
                sc.TrimStart();
                string val = string.Empty;
                Nullable<char> quotesSign = (sc.StartValue == '\"' || sc.StartValue == '\'' ? (Nullable<char>)sc.StartValue : null);
                for (int i = (sc.Index + (quotesSign.HasValue ? 1 : 0)); i < sc.Value.Length; i++)
                {
                    if (quotesSign.HasValue)
                    {
                        if (sc.Value[i] == quotesSign.Value)
                        {
                            sc.Index = i + 1;
                            break;
                        }
                        else
                        {
                            val += sc.Value[i];
                        }
                    }
                    else
                    {
                        if (sc.Value[i] == ' ' || sc.Value[i] == '>')
                        {
                            sc.Index = i;
                            break;
                        }
                        else
                        {
                            val += sc.Value[i];
                        }
                    }
                }

                att = new XAttribute(name, XmlParser.DecodeXml(val));
            }

            sc.TrimStart();
            return att;
        }
        private XContainer ParseNode(StringCounter sc, XContainer parent)
        {
            sc.TrimStart();
            if (sc.StartValue == '<')
            {
                sc.Index = sc.Index + 1;
                XContainer node = null;
                bool isComment = false;
                bool isDeclaration = false;
                int breakOff = 0;
                XName name = null;
                if (sc.StartsWith("?xml"))
                {
                    //Declaration
                    isDeclaration = true;
                    sc.Index += 4;
                }
                else if (sc.StartValue == '!')
                {
                    //Comment
                    isComment = true;
                    if (sc.Value[sc.Index + 1] == '-') breakOff = 1;
                    if (sc.Value[sc.Index + 2] == '-') breakOff = 2;
                    sc.Index += breakOff + 1;
                    sc.TrimStart();
                }
                else if (IsValidTagChar(sc.StartValue))
                {
                    //Name
                    name = ParseName(sc);
                    if (name != null)
                    {
                        node = new XElement(name);
                    }
                    else
                    {
                        throw new Exception("Invalid Node Name.");
                    }
                }
                else
                {
                    throw new Exception("Invalid Node Name.");
                }



                if (node != null || isComment || isDeclaration)
                {

                    //Attributes
                    bool elementAtEnd = name != null ? (name.LocalName == "br") : false;

                    string comment = string.Empty;
                    string declVer = string.Empty;
                    string declEnc = string.Empty;
                    string declSta = string.Empty;
                    for (int i = sc.Index; i < sc.Value.Length; i++)
                    {
                        if (!isComment && !isDeclaration)
                        {
                            //Node Attributes
                            if (sc.Value[i] != ' ')
                            {                                                     
                                if (sc.Value[i] == '>')
                                {
                                    sc.Index = i + 1;
                                    break;
                                }
                                else if (sc.Value[i] == '/' && sc.Value[i + 1] == '>')
                                {
                                    elementAtEnd = true;
                                    sc.Index += 2;
                                    break;
                                }
                                else if (IsValidTagChar(sc.Value[i]))
                                {
                                    XAttribute att = ParseAttribute(sc.NewIndex(i));
                                    i = sc.Index - 1;
                                    if (att != null)
                                    {
                                        node.Add(att);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (isComment)
                            {
                                //Comment
                                if ((breakOff == 2 && sc.Value[i] == '-' && sc.Value[i + 1] == '-' && sc.Value[i + 2] == '>') || (breakOff == 0 && sc.Value[i] == '>') || (breakOff == 1 && sc.Value[i] == '-' && sc.Value[i + 1] == '>'))
                                {
                                    if (parent != null) parent.Add(new XComment(comment));
                                    sc.Index = i + breakOff + 1;
                                    break;
                                }
                                else
                                {
                                    comment += sc.Value[i];
                                }
                            }
                            else if (isDeclaration)
                            {
                                //Declaration
                                if (sc.Value[i] == '?' && sc.Value[i + 1] == '>')
                                {
                                    if (parent != null && parent is XDocument) ((XDocument)parent).Declaration = new XDeclaration(declVer, declEnc, declSta);
                                    sc.Index = i + 2;
                                    break;
                                }
                                else if (IsValidTagChar(sc.Value[i]))
                                {
                                    XAttribute att = ParseAttribute(sc.NewIndex(i));
                                    i = sc.Index - 1;
                                    if (att != null)
                                    {
                                        if (att.Name.LocalName.ToLower() == "version")
                                        {
                                            declVer = att.Value;
                                        }
                                        else if (att.Name.LocalName.ToLower() == "encoding")
                                        {
                                            declEnc = att.Value;
                                        }
                                        else if (att.Name.LocalName.ToLower() == "standalone")
                                        {
                                            declSta = att.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (node != null) parent.Add(node);


                    //Content
                    if (node != null && !elementAtEnd)
                    {
                        string innerText = string.Empty;
                        string fullName = name.ToString();
                        for (int i = sc.Index; i < sc.Value.Length; i++)
                        {
                            if (sc.Value[i] == '<')
                            {
                                if (innerText != string.Empty)
                                {
                                    node.Add(XmlParser.DecodeXml(innerText));
                                    innerText = string.Empty;
                                }

                                if (sc.Value[i + 1] == '/')
                                {
                                    //End Tag
                                    XName endName = ParseName(sc.NewIndex(i + 2));
                                    if (endName != null)
                                    {
                                        if (CompareXName(endName, name))
                                        {
                                            //Actual End Name
                                            sc.Index = i + 3 + fullName.Length;
                                            break;
                                        }
                                        else
                                        {
                                            if (name.LocalName != "script")
                                            {
                                                //Other End Name
                                                XContainer par = ((parent is XElement && XmlParser.CompareXName(((XElement)parent).Name, endName)) ? parent : (XContainer)this.FindParent(parent, endName));
                                                if (par != null)
                                                {
                                                    if (name.LocalName != "form")
                                                    {
                                                        XElement[] enm = MyHelper.EnumToArray(node.Elements());
                                                        if (enm.Length > 0)
                                                        {
                                                            foreach (XElement elem in enm)
                                                            {
                                                                parent.Add(elem);
                                                            }
                                                        }
                                                        node.Elements().Remove();
                                                        sc.Index = i;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        sc.Index = i + endName.ToString().Length + 3;
                                                        i = sc.Index - 1;
                                                    }
                                                }
                                                else
                                                {
                                                    sc.Index = i + endName.ToString().Length + 2;
                                                    i = sc.Index - 1;
                                                }
                                            }
                                        }
                                    }
                                    else if (fullName == "script")
                                    {
                                        //Script Text
                                        //innerText += text[i];
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid End Name.");
                                    }

                                }
                                else if (fullName == "script")
                                {
                                    //Script Text
                                    //innerText += text[i];
                                }
                                else
                                {
                                    //Start Tag
                                    XContainer childNode = this.ParseNode(sc.NewIndex(i), node);
                                    i = sc.Index - 1;
                                }
                            }
                            else if (!(sc.Value[i] == ' ' && innerText == string.Empty))
                            {
                                //Inner Text
                                if (fullName != "script") innerText += sc.Value[i];
                            }
                        }
                    }

                }
                sc.TrimStart();
                return node;
            }
            else
            {
                throw new Exception("Invalid Start Tag. A node has to start with [<].");
            }
        }
    
        private bool IsValidTagChar(char c) { return c == '_' || c == '-' || char.IsLetterOrDigit(c); }
        public static string DecodeXml(string escapedTxt)
        {
            if (escapedTxt.Length > 4)
            {
                string newTxt = string.Empty;
                bool isInSequence = false;
                string sequenceTxt = string.Empty;
                for (int i = 0; i < escapedTxt.Length; i++)
                {
                    if (!isInSequence)
                    {
                        if (escapedTxt[i] == '&')
                        {
                            isInSequence = true;
                        }
                        else
                        {
                            newTxt += escapedTxt[i];
                        }
                    }
                    else
                    {
                        if (escapedTxt[i] == ';')
                        {
                            isInSequence = false;
                            switch (sequenceTxt)
                            {
                                case "quot":
                                    newTxt += '\"';
                                    break;
                                case "apos":
                                    newTxt += '\'';
                                    break;
                                case "lt":
                                    newTxt += '<';
                                    break;
                                case "gt":
                                    newTxt += '>';
                                    break;
                                case "amp":
                                    newTxt += '&';
                                    break;
                                default:
                                    newTxt += '&' + sequenceTxt + ';';
                                    break;
                            }
                            sequenceTxt = string.Empty;
                        }
                        else
                        {
                            sequenceTxt += escapedTxt[i];
                        }
                    }

                }
                return newTxt;
            }
            else
            {
                return escapedTxt;
            }
        }
        public static string EncodeXml(string unescapedTxt)
        {
            string newTxt = string.Empty;
            if (unescapedTxt.Length > 0)
            {
                for (int i = 0; i < unescapedTxt.Length; i++)
                {
                    switch (unescapedTxt[i])
                    {
                        case '&':
                            newTxt += "&amp;";
                            break;
                        case '\"':
                            newTxt += "&quot;";
                            break;
                        case '\'':
                            newTxt += "&apos;";
                            break;
                        case '<':
                            newTxt += "&lt;";
                            break;
                        case '>':
                            newTxt += "&gt;";
                            break;
                        default:
                            newTxt += unescapedTxt[i];
                            break;
                    }
                }
            }
            return newTxt;
        }
        private XObject FindParent(XObject child, XName parentName)
        {
            if (child.Parent != null)
            {
                if (CompareXName(child.Parent.Name, parentName))
                {
                    return child.Parent;
                }
                else
                {
                    return FindParent(child.Parent, parentName);
                }
            }
            else
            {
                return null;
            }
        }

        private class StringCounter
        {
            public int Index = 0;
            public string Value = string.Empty;
            public bool IsAtEnd { get { return this.Index >= this.Value.Length - 1; } }
            public char StartValue { get { return this.Value[this.Index]; } }
            public StringCounter(string value)
            {
                this.Value = value;
            }
            public StringCounter NewIndex(int index)
            {
                this.Index = index;
                return this;
            }
            public void TrimStart()
            {
                for (int i = this.Index; i < this.Value.Length; i++)
                {
                    if (this.Value[i] != ' ' && this.Value[i] != '\n' && this.Value[i] != '\r' && this.Value[i] != '\t') break;
                    this.Index = i + 1;
                }
            }
            public bool StartsWith(string value)
            {
                if (value != string.Empty)
                {
                    for (int i = this.Index; i < this.Value.Length; i++)
                    {
                        if (i - this.Index < value.Length)
                        {
                            if (this.Value[i] != value[i - this.Index]) return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

}

namespace System.Xml.Linq { internal abstract class PlaceHolder { } }