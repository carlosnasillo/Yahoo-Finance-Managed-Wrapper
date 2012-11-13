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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using MaasOne.Xml;


namespace MaasOne.RSS.ImportExport
{

    public class XML
    {
        public static Feed ToFeed(XElement channel)
        {
            if (channel.Name.LocalName.ToLower() == "channel")
            {
                Feed feed = new Feed();
                foreach (XElement prpNode in channel.Elements())
                {                                       
                    try
                    {
                        switch (prpNode.Name.LocalName.ToLower())
                        {
                            case "category":
                                feed.Category = new Category();
                                feed.Category.Name = prpNode.Value;
                                if (MyHelper.EnumToArray(prpNode.Attributes()).Length == 1)
                                    feed.Category.Domain = new Uri(prpNode.Attribute(XName.Get("domain")).Value);
                                break;
                            case "cloud":
                                feed.Cloud = new Cloud();
                                foreach (XAttribute att in prpNode.Attributes())
                                {
                                    switch (att.Name.LocalName.ToLower())
                                    {
                                        case "domain":
                                            feed.Cloud.Domain = new Uri(att.Value);
                                            break;
                                        case "path":
                                            feed.Cloud.Path = att.Value;
                                            break;
                                        case "registerprocedure":
                                            feed.Cloud.RegisterProcedure = att.Value;
                                            break;
                                        case "protocol":
                                            feed.Cloud.Protocol = att.Value;
                                            break;
                                        case "port":
                                            int n;
                                            if (int.TryParse(att.Value, out n)) feed.Cloud.Port = n;
                                            break;
                                    }
                                }

                                break;
                            case "copyright":
                                feed.Copyright = prpNode.Value;
                                break;
                            case "description":
                                feed.Description = prpNode.Value;
                                break;
                            case "docs":
                                feed.Documentation = new Uri(prpNode.Value);
                                break;
                            case "generator":
                                feed.Generator = prpNode.Value;
                                break;
                            case "link":
                                feed.Link = new Uri(prpNode.Value);
                                break;
                            case "language":
                                feed.Language = new System.Globalization.CultureInfo(prpNode.Value);
                                break;
                            case "lastbuilddate":
                                feed.LastBuildDate = RFC822DateFromString(prpNode.Value);
                                break;
                            case "managingeditor":
                                feed.ManagingEditor = Rss2MailToMailAddress(prpNode.Value);
                                break;
                            case "name":
                                feed.Name = prpNode.Value;
                                break;
                            case "image":
                                feed.Image = new Image();
                                foreach (XElement nodeChild in prpNode.Elements())
                                {
                                    switch (nodeChild.Name.LocalName.ToLower())
                                    {
                                        case "url":
                                            feed.Image.URL = new Uri(nodeChild.Value);
                                            break;
                                        case "link":
                                            feed.Image.Link = new Uri(nodeChild.Value);
                                            break;
                                        case "title":
                                            feed.Image.Title = nodeChild.Value;
                                            break;
                                        case "description":
                                            feed.Image.Description = nodeChild.Value;
                                            break;
                                        case "width":
                                            int n;
                                            if (int.TryParse(nodeChild.Value, out n)) feed.Image.Width = n;
                                            break;
                                        case "height":
                                            if (int.TryParse(nodeChild.Value, out n)) feed.Image.Height = n;
                                            break;
                                    }
                                }

                                break;
                            case "item":
                                FeedItem rssItem = ToFeedItem(prpNode);
                                if (rssItem != null)
                                    feed.Items.Add(rssItem);
                                break;
                            case "pubdate":
                                feed.PublishDate = RFC822DateFromString(prpNode.Value);
                                break;
                            case "rating":
                                feed.Rating = prpNode.Value;
                                break;
                            case "skiphours":
                                List<int> lst1 = new List<int>();
                                foreach (XElement nodeChild in prpNode.Elements())
                                {
                                    if (nodeChild.Name.LocalName.ToLower() == "hour")
                                    {
                                        int @int = 0;
                                        if (int.TryParse(nodeChild.Value, out @int))
                                            lst1.Add(@int);
                                    }
                                }

                                feed.Skiphours = lst1.ToArray();
                                break;
                            case "skipdays":
                                List<DayOfWeek> lst2 = new List<DayOfWeek>();
                                foreach (XElement nodeChild in prpNode.Elements())
                                {
                                    if (nodeChild.Name.LocalName.ToLower() == "day")
                                    {
                                        for (int i = 0; i <= (int)DayOfWeek.Saturday; i++)
                                        {
                                            if (((DayOfWeek)i).ToString().ToUpper() == nodeChild.Value.ToUpper())
                                            {
                                                lst2.Add((DayOfWeek)i);
                                                break; // TODO: might not be correct. Was : Exit For
                                            }
                                        }
                                    }
                                }

                                feed.Skipdays = lst2.ToArray();
                                break;
                            case "textinput":
                                feed.TextInput = new TextInputBox();
                                foreach (XElement nodeChild in prpNode.Elements())
                                {
                                    switch (nodeChild.Name.LocalName.ToLower())
                                    {
                                        case "name":
                                            feed.TextInput.Name = nodeChild.Value;
                                            break;
                                        case "link":
                                            feed.TextInput.Link = new Uri(nodeChild.Value);
                                            break;
                                        case "title":
                                            feed.TextInput.Title = nodeChild.Value;
                                            break;
                                        case "description":
                                            feed.TextInput.Description = nodeChild.Value;
                                            break;
                                    }
                                }


                                break;
                            case "title":
                                feed.Title = prpNode.Value;
                                break;
                            case "ttl":
                                feed.Ttl = Convert.ToInt32(prpNode.Value);

                                break;
                            case "webmaster":
                                feed.Webmaster = Rss2MailToMailAddress(prpNode.Value);
                                break;
                            default:
                                break;
                            //Debug.WriteLine(node.Name)
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return feed;
            }
            else
            {
                return null;
            }
        }
        public static FeedItem ToFeedItem(XElement item)
        {
            if (item.Name.LocalName == "item")
            {
                FeedItem rssItem = new FeedItem();
                foreach (XElement itemNode in item.Elements())
                {
                    try
                    {
                        switch (itemNode.Name.LocalName.ToLower())
                        {
                            case "title":
                                rssItem.Title = itemNode.Value;
                                break;
                            case "link":
                                rssItem.Link = new Uri(itemNode.Value);
                                break;
                            case "description":
                                rssItem.Description = itemNode.Value;
                                break;
                            case "author":
                                rssItem.Author = itemNode.Value;
                                break;
                            case "category":
                                rssItem.Category = new Category();
                                rssItem.Category.Name = itemNode.Value;
                                if (MyHelper.EnumToArray(itemNode.Attributes()).Length == 1)
                                    rssItem.Category.Domain = new Uri(itemNode.Attribute(XName.Get("domain")).Value);
                                break;
                            case "comments":
                                rssItem.Comments = new Uri(itemNode.Value);
                                break;
                            case "enclosure":
                                rssItem.Enclosure = new Enclosure();
                                rssItem.Enclosure.Url = new Uri(itemNode.Attribute(XName.Get("url")).Value);
                                long l;
                                if (long.TryParse(itemNode.Attribute(XName.Get("length")).Value, out l)) rssItem.Enclosure.Length = l;
                                rssItem.Enclosure.Type = itemNode.Attribute(XName.Get("type")).Value;
                                break;
                            case "guid":
                                rssItem.GUID = new GUID();
                                rssItem.GUID.ID = itemNode.Value;
                                if (MyHelper.EnumToArray(itemNode.Attributes()).Length == 1)
                                    rssItem.GUID.IsPermaLink = Convert.ToBoolean(itemNode.Attribute(XName.Get("isPermaLink")).Value);
                                break;
                            case "pubdate":
                                rssItem.PublishDate = RFC822DateFromString(itemNode.Value);
                                break;
                            case "insertdate":
                                rssItem.InsertDate = RFC822DateFromString(itemNode.Value);
                                break;
                            case "source":
                                rssItem.Source = new Source();
                                rssItem.Source.Title = itemNode.Value;
                                rssItem.Source.Url = new Uri(itemNode.Attribute(XName.Get("url")).Value);
                                break;
                            default:
                                break;
                            //Debug.WriteLine(itemNode.Name.LocalName)
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return rssItem;
            }
            return null;
        }
        private static System.DateTime RFC822DateFromString(string d)
        {
            System.DateTime result = default(System.DateTime);
            if (d != string.Empty)
            {
                int spacePos = d.LastIndexOf(" ");
                string timezone = d.Substring(spacePos + 1);

                if (System.DateTime.TryParse(d, out result))
                {
                    if (timezone == "Z" | timezone == "GMT")
                        result = result.ToUniversalTime();
                    return result;
                }
                else
                {
                    result = Convert.ToDateTime(d.Substring(0, spacePos));
                    if (d[spacePos + 1] == '+')
                    {
                        result = result.AddHours(-1 * Convert.ToInt32(d.Substring(spacePos + 2, 2)));
                        result = result.AddMinutes(-1 * Convert.ToInt32(d.Substring(spacePos + 4, 2)));
                    }
                    else if (d[spacePos + 1] == '-')
                    {
                        int h = Convert.ToInt32(d.Substring(spacePos + 2, 2));
                        result = result.AddHours(h);
                        int m = Convert.ToInt32(d.Substring(spacePos + 4, 2));
                        result = result.AddMinutes(m);
                    }
                    else
                    {
                        switch (timezone)
                        {
                            case "A":
                                result = result.AddHours(1);
                                break;
                            case "B":
                                result = result.AddHours(2);
                                break;
                            case "C":
                                result = result.AddHours(3);
                                break;
                            case "D":
                                result = result.AddHours(4);
                                break;
                            case "EDT":
                                result = result.AddHours(4);
                                break;
                            case "E":
                                result = result.AddHours(5);
                                break;
                            case "EST":
                                result = result.AddHours(5);
                                break;
                            case "CDT":
                                result = result.AddHours(5);
                                break;
                            case "F":
                                result = result.AddHours(6);
                                break;
                            case "CST":
                                result = result.AddHours(6);
                                break;
                            case "MDT":
                                result = result.AddHours(6);
                                break;
                            case "G":
                                result = result.AddHours(7);
                                break;
                            case "MST":
                                result = result.AddHours(7);
                                break;
                            case "PDT":
                                result = result.AddHours(7);
                                break;
                            case "H":
                                result = result.AddHours(8);
                                break;
                            case "PST":
                                result = result.AddHours(8);
                                break;
                            case "I":
                                result = result.AddHours(9);
                                break;
                            case "K":
                                result = result.AddHours(10);
                                break;
                            case "L":
                                result = result.AddHours(11);
                                break;
                            case "M":
                                result = result.AddHours(12);
                                break;
                            case "N":
                                result = result.AddHours(-1);
                                break;
                            case "O":
                                result = result.AddHours(-2);
                                break;
                            case "P":
                                result = result.AddHours(-3);
                                break;
                            case "Q":
                                result = result.AddHours(-4);
                                break;
                            case "R":
                                result = result.AddHours(-5);
                                break;
                            case "S":
                                result = result.AddHours(-6);
                                break;
                            case "T":
                                result = result.AddHours(-7);
                                break;
                            case "U":
                                result = result.AddHours(-8);
                                break;
                            case "V":
                                result = result.AddHours(-9);
                                break;
                            case "W":
                                result = result.AddHours(-10);
                                break;
                            case "X":
                                result = result.AddHours(-11);
                                break;
                            case "Y":
                                result = result.AddHours(-12);
                                break;
                        }
                    }
                }
            }
            return result;
        }
        private static System.Net.Mail.MailAddress Rss2MailToMailAddress(string address)
        {
            if (address != string.Empty)
            {
                try
                {
                    int ind = address.IndexOf('(');
                    if (ind > -1)
                    {
                        string adr = address.Substring(0, ind).Trim();
                        string name = address.Substring(ind).Replace("(", "").Replace(")", "").Trim();
                        return new System.Net.Mail.MailAddress(adr, name);
                    }
                    else
                    {
                        return new System.Net.Mail.MailAddress(address);
                    }
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

        public static void ToXml(System.Xml.XmlWriter writer, Feed feed)
        {
            writer.WriteStartElement("channel");

            writer.WriteElementString("title", feed.Title);
            writer.WriteElementString("link", (feed.Link != null ? feed.Link.AbsoluteUri : "").ToString());
            writer.WriteElementString("description", feed.Description);

            if (feed.Language != null)
                writer.WriteElementString("language", feed.Language.Name);
            if (feed.Copyright != string.Empty)
                writer.WriteElementString("copyright", feed.Copyright);
            if (feed.ManagingEditor != null)
                writer.WriteElementString("managingEditor", feed.ManagingEditor.Address + " (" + feed.ManagingEditor.DisplayName + ")");
            if (feed.Webmaster != null)
                writer.WriteElementString("webMaster", feed.Webmaster.Address + " (" + feed.Webmaster.DisplayName + ")");
            if (feed.PublishDate != new DateTime())
                writer.WriteElementString("pubDate", feed.PublishDate.ToLongDateString() + " " + feed.PublishDate.ToLongTimeString());
            if (feed.LastBuildDate != new DateTime())
                writer.WriteElementString("lastBuildDate", feed.LastBuildDate.ToLongDateString() + " " + feed.LastBuildDate.ToLongTimeString());
            if (feed.Category != null)
            {
                writer.WriteStartElement("category");
                if (feed.Category.Domain != null)
                    writer.WriteAttributeString("domain", feed.Category.Domain.AbsoluteUri);
                writer.WriteString(feed.Category.Name);
                writer.WriteEndElement();
            }
            if (feed.Generator != string.Empty)
                writer.WriteElementString("generator", feed.Generator);
            if (feed.Documentation != null)
                writer.WriteElementString("docs", feed.Documentation.AbsoluteUri);
            if (feed.Cloud != null)
            {
                writer.WriteStartElement("cloud");
                if (feed.Cloud.Domain != null)
                    writer.WriteAttributeString("domain", feed.Cloud.Domain.AbsoluteUri);
                if (feed.Cloud.Port != 0)
                    writer.WriteAttributeString("port", feed.Cloud.Port.ToString());
                if (feed.Cloud.Path != string.Empty)
                    writer.WriteAttributeString("path", feed.Cloud.Path);
                if (feed.Cloud.RegisterProcedure != string.Empty)
                    writer.WriteAttributeString("registerProcedure", feed.Cloud.RegisterProcedure);
                if (feed.Cloud.Protocol != string.Empty)
                    writer.WriteAttributeString("protocoll", feed.Cloud.Protocol);
                writer.WriteEndElement();
            }
            if (feed.Ttl != 0)
                writer.WriteElementString("ttl", feed.Ttl.ToString());
            if (feed.Image != null)
            {
                writer.WriteStartElement("image");
                if (feed.Image.URL != null)
                    writer.WriteElementString("url", feed.Image.URL.AbsoluteUri);
                if (feed.Image.Title != string.Empty)
                    writer.WriteElementString("title", feed.Image.Title);
                if (feed.Image.Description != string.Empty)
                    writer.WriteElementString("description", feed.Image.Description);
                if (feed.Image.Link != null)
                    writer.WriteElementString("link", feed.Image.Link.AbsoluteUri);
                if (feed.Image.Width != 0)
                    writer.WriteElementString("width", feed.Image.Width.ToString());
                if (feed.Image.Height != 0)
                    writer.WriteElementString("height", feed.Image.Height.ToString());
                writer.WriteEndElement();
            }
            if (feed.Rating != string.Empty)
                writer.WriteElementString("rating", feed.Rating);
            if (feed.TextInput != null)
            {
                writer.WriteStartElement("textInput");
                if (feed.TextInput.Name != string.Empty)
                    writer.WriteElementString("url", feed.TextInput.Name);
                if (feed.TextInput.Title != string.Empty)
                    writer.WriteElementString("title", feed.TextInput.Title);
                if (feed.TextInput.Description != string.Empty)
                    writer.WriteElementString("description", feed.TextInput.Description);
                if (feed.TextInput.Link != null)
                    writer.WriteElementString("link", feed.TextInput.Link.AbsoluteUri);
                writer.WriteEndElement();
            }
            if (feed.Skiphours != null && feed.Skiphours.Length > 0)
            {
                writer.WriteStartElement("skipHours");
                foreach (int h in feed.Skiphours)
                {
                    writer.WriteElementString("hour", h.ToString());
                }
                writer.WriteEndElement();
            }
            if (feed.Skipdays != null && feed.Skipdays.Length > 0)
            {
                writer.WriteStartElement("skipDays");
                foreach (DayOfWeek d in feed.Skipdays)
                {
                    writer.WriteElementString("hour", d.ToString());
                }
                writer.WriteEndElement();
            }

            foreach (FeedItem item in feed.Items)
            {
                WriteFeedItem(writer, item);
            }

            writer.WriteEndElement();
        }
        private static void WriteFeedItem(System.Xml.XmlWriter writer, FeedItem item)
        {
            writer.WriteStartElement("item");

            if (item.Title != string.Empty)
                writer.WriteElementString("title", item.Title);
            if (item.Link != null)
                writer.WriteElementString("link", item.Link.AbsoluteUri);
            if (item.Description != string.Empty)
                writer.WriteElementString("description", item.Description);
            if (item.Author != string.Empty)
                writer.WriteElementString("author", item.Author);
            if (item.Category != null)
            {
                writer.WriteStartElement("category");
                if (item.Category.Domain != null)
                    writer.WriteAttributeString("domain", item.Category.Domain.AbsoluteUri);
                writer.WriteString(item.Category.Name);
                writer.WriteEndElement();
            }
            if (item.Comments != null)
                writer.WriteElementString("comments", item.Comments.AbsoluteUri);
            if (item.Enclosure != null)
            {
                writer.WriteStartElement("enclosure");
                if (item.Enclosure.Url != null)
                    writer.WriteAttributeString("url", item.Enclosure.Url.AbsoluteUri);
                if (item.Enclosure.Length != 0)
                    writer.WriteAttributeString("length", item.Enclosure.Length.ToString());
                if (item.Enclosure.Type != string.Empty)
                    writer.WriteAttributeString("type", item.Enclosure.Type);
                writer.WriteEndElement();
            }
            if (item.GUID != null)
            {
                writer.WriteStartElement("guid");
                writer.WriteAttributeString("isPermaLink", item.GUID.IsPermaLink.ToString().ToLower());
                writer.WriteString(item.GUID.ID);
                writer.WriteEndElement();
            }
            if (item.Comments != null)
                writer.WriteElementString("comments", item.Comments.AbsoluteUri);
            if (item.PublishDate != new DateTime())
                writer.WriteElementString("pubDate", item.PublishDate.ToLongDateString() + " " + item.PublishDate.ToLongTimeString());
            if (item.Source != null)
            {
                writer.WriteStartElement("source");
                if (item.Source.Url != null)
                    writer.WriteAttributeString("url", item.Source.Url.AbsoluteUri);
                writer.WriteString(item.Source.Title);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

    }

}
