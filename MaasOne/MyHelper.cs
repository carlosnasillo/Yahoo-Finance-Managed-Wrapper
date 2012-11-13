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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using MaasOne.Xml;
using System.Globalization;
using System.Diagnostics;
using System.Xml.Linq;


namespace MaasOne
{
    public abstract class MyHelper
    {

        public static readonly System.Globalization.CultureInfo ConverterCulture = new System.Globalization.CultureInfo("en-US");

        public static string StreamToString(System.IO.Stream stream, System.Text.Encoding encoding = null)
        {
            string res = string.Empty;
            if (stream != null)
            {
                System.Text.Encoding enc = encoding;
                if (enc == null)
                    enc = System.Text.Encoding.UTF8;
                if (stream.CanSeek) stream.Seek(0, System.IO.SeekOrigin.Begin);
                if (stream.CanRead)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream, enc))
                    {
                        res = sr.ReadToEnd();
                    }
                }
            }
            return res;
        }
        public static byte[] StreamToBytes(System.IO.Stream s)
        {
            byte[] result = new byte[] { };
            if (s != null)
            {
                result = new byte[Convert.ToInt32(s.Length) + 1];
                s.Read(result, 0, Convert.ToInt32(s.Length));
            }
            return result;
        }
        public static System.IO.MemoryStream CopyStream(System.IO.Stream source)
        {
            System.IO.MemoryStream copy = new System.IO.MemoryStream();
            if (source != null && source.CanRead)
            {
                byte[] buffer = new byte[Convert.ToInt32(2048) + 1];
                while (true)
                {
                    int read = source.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }
                    copy.Write(buffer, 0, read);
                }
            }
            copy.Position = 0;
            return copy;
        }

        public static T[] EnumToArray<T>(IEnumerable<T> values)
        {
            if (values != null)
            {
                if (values is T[])
                {
                    return (T[])values;
                }
                else
                {
                    return new List<T>(values).ToArray();
                }
            }
            else
            {
                return new T[] { };
            }
        }
        public static string CharEnumToString(IEnumerable<char> arr)
        {
            string s = string.Empty;
            if (arr != null)
            {
                foreach (char c in arr)
                {
                    s += c;
                }
            }
            return s;
        }
        public static T GetEnumItemAt<T>(IEnumerable<T> values, int index)
        {
            int cnt = 0;
            foreach (T itm in values)
            {
                if (cnt == index) return itm;
                cnt++;
            }
            return default(T);
        }

        public static object StringToObject(string str, System.Globalization.CultureInfo ci)
        {
            string value = str.Replace("%", "").Replace("\"", "").Replace("<b>", "").Replace("</b>", "").Replace("N/A", "").Trim();
            if (value != string.Empty)
            {
                if (value == "-") { return string.Empty; }
                else if (value.Contains(" - "))
                {
                    String[] values = value.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    List<object> results = new List<object>();
                    foreach (String v in values)
                    {
                        results.Add(StringToObject(v, ci));
                    }
                    if (results.Count == 0) { return string.Empty; }
                    else if (results.Count == 0) { return results[0]; }
                    else { return results.ToArray(); }
                }
                else
                {
                    double dbl = 0;
                    if (double.TryParse(value, System.Globalization.NumberStyles.Any, ci, out dbl))
                    {
                        return dbl;
                    }
                    else
                    {
                        long lng = 0;
                        if (long.TryParse(value, out lng))
                        {
                            return lng;
                        }
                        else
                        {
                            System.DateTime dte = default(System.DateTime);
                            if (System.DateTime.TryParse(value, ci, System.Globalization.DateTimeStyles.AdjustToUniversal, out dte))
                            {
                                return dte;
                            }
                            else
                            {
                                return value;
                            }
                        }
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ObjectToString(object value, System.Globalization.CultureInfo ci)
        {
            if (value != null)
            {
                if (value is double)
                {
                    return Convert.ToDouble(value).ToString(ci);
                }
                else if (value is System.DateTime)
                {
                    return Convert.ToDateTime(value).ToString(ci);
                }
                else if (value is object[])
                {
                    object[] values = (object[])value;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (object obj in values)
                    {
                        sb.Append(ObjectToString(obj, ci));
                        if (!object.ReferenceEquals(obj, values[values.Length - 1]))
                            sb.Append(" - ");
                    }
                    return sb.ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static XDocument ParseXmlDocument(string text) { return XmlParser.Parse(text); }
        public static XDocument ParseXmlDocument(System.IO.Stream xml)
        {
            if (xml != null && xml.CanRead)
            {
                try
                {
                    return ParseXmlDocument(StreamToString(xml, System.Text.Encoding.UTF8));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static string GetXmlAttributeValue(XElement node, string attName)
        {
            if (node != null)
            {
                XAttribute att = node.Attribute(XName.Get(attName));
                if (att != null) return att.Value;
            }
            return string.Empty;
        }

        public static string YqlUrl(string statement, bool json)
        {
            string format = "json";
            if (json == false)
                format = "xml";
            return "http://query.yahooapis.com/v1/public/yql?q=" + Uri.EscapeDataString(statement) + "&format=" + format + "&diagnostics=false&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        }
        public static string YqlUrl(string fields, string table, string whereParam, IResultIndexSettings opt, bool json)
        {
            return YqlUrl(YqlStatement(fields, table, whereParam, opt), json);
        }
        public static string YqlStatement(string fields, string table, string whereParam, IResultIndexSettings opt)
        {
            System.Text.StringBuilder stmt = new System.Text.StringBuilder();
            stmt.Append("select ");
            stmt.Append(fields);
            stmt.Append(" from ");
            stmt.Append(table);
            if (opt != null && opt.Count > 0)
            {
                stmt.Append("(");
                stmt.Append(opt.Index.ToString());
                stmt.Append(",");
                stmt.Append(opt.Count.ToString());
                stmt.Append(")");
            }
            if (whereParam.Trim() != string.Empty)
            {
                stmt.Append(" where ");
                stmt.Append(whereParam);
            }
            return stmt.ToString();
        }
        public static string CleanYqlParam(string id)
        {
            return id.Replace("\"", "").Replace("'", "").Trim();
        }
     
        public static string[][] CsvTextToStringTable(string csv, char delimiter)
        {
            string[] rows = csv.Split(new String[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> lst = new List<string[]>();
            foreach (string row in rows)
            {
                if (row.Trim() != string.Empty)
                    lst.Add(CsvRowToStringArray(row.Trim(), delimiter));
            }
            return lst.ToArray();
        }

        public static string[] CsvRowToStringArray(string row, char delimiter, bool withQuoteMarks = true)
        {
            if (withQuoteMarks)
            {
                List<string> lstParts = new List<string>();
                int actualIndex = 0;
                int tempStartIndex = 0;
                bool waitForNextQuoteMark = false;

                while (!(actualIndex == row.Length))
                {
                    if (row[actualIndex] == '\"')
                    {
                        waitForNextQuoteMark = !waitForNextQuoteMark;
                    }
                    else if (row[actualIndex] == delimiter)
                    {
                        if (!waitForNextQuoteMark)
                        {
                            lstParts.Add(ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex)));
                            tempStartIndex = actualIndex + 1;
                        }
                    }
                    actualIndex += 1;
                    if (actualIndex == row.Length)
                    {
                        string s = ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex));
                        lstParts.Add(s);
                    }
                }
                return lstParts.ToArray();
            }
            else
            {
                return row.Split(delimiter);
            }
        }
        private static string ClearCsvString(string csv)
        {
            if (csv.Length > 0)
            {
                string result = csv;
                if (result.StartsWith("\""))
                    result = result.Substring(1);
                if (result.EndsWith("\""))
                    result = result.Substring(0, result.Length - 1);
                return result.Replace("\"\"", "\"");
            }
            else
            {
                return csv;
            }
        }

        private MyHelper() { }
        static MyHelper() { }
    }
}
