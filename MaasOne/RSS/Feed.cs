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


namespace MaasOne.RSS
{

	/// <summary>
	/// Provides RSS feed data.
	/// </summary>
	/// <remarks></remarks>
	public class Feed
	{

		/// <summary>
		/// Specify one or more categories that the channel belongs to.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Category Category { get; set; }
		/// <summary>
		/// Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. 
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Cloud Cloud { get; set; }
		/// <summary>
		/// Copyright notice for content in the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Copyright { get; set; }
		/// <summary>
		/// 	Phrase or sentence describing the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Description { get; set; }
		/// <summary>
		/// A URL that points to the documentation (http://www.rssboard.org/rss-specification) for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Documentation { get; set; }
		/// <summary>
		/// A string indicating the program used to generate the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Generator { get; set; }
		/// <summary>
		/// Specifies a GIF, JPEG or PNG image that can be displayed with the channel. 
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Image Image { get; set; }
		/// <summary>
		/// A channel may contain any number of items. An item may represent a "story" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public List<FeedItem> Items { get; set; }
		/// <summary>
		/// The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public System.Globalization.CultureInfo Language { get; set; }
		/// <summary>
		/// The last time the content of the channel changed.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public DateTime LastBuildDate { get; set; }
		/// <summary>
		/// The URL to the HTML website corresponding to the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Link { get; set; }
		/// <summary>
		/// Email address for person responsible for editorial content.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public System.Net.Mail.MailAddress ManagingEditor { get; set; }
		public string Name { get; set; }
		/// <summary>
		/// The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public DateTime PublishDate { get; set; }
		/// <summary>
		/// The PICS (http://www.w3.org/PICS/) rating for the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Rating { get; set; }
		/// <summary>
		/// A hint for aggregators telling them which hours they can skip. This element contains up to 24 hour sub-elements whose value is a number between 0 and 23, representing a time in GMT, when aggregators, if they support the feature, may not read the channel on hours listed in the SkipHours element. The hour beginning at midnight is hour zero.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public int[] Skiphours { get; set; }
		/// <summary>
		/// A hint for aggregators telling them which days they can skip. This element contains up to seven day sub-elements whose value is Monday, Tuesday, Wednesday, Thursday, Friday, Saturday or Sunday. Aggregators may not read the channel during days listed in the SkipDays element.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public DayOfWeek[] Skipdays { get; set; }
		/// <summary>
		/// Specifies a text input box that can be displayed with the channel. The purpose of the TextInput element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public TextInputBox TextInput { get; set; }
		/// <summary>
		/// The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Title { get; set; }
		/// <summary>
		/// ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public int Ttl { get; set; }
		/// <summary>
		/// Email address for person responsible for technical issues relating to channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public System.Net.Mail.MailAddress Webmaster { get; set; }

		public Feed()
		{
			this.Clear();
		}

		public void Clear()
		{
			this.Description = string.Empty;
			this.Title = string.Empty;
			this.Language = null;
			this.Documentation = null;
			this.PublishDate = new DateTime();;
			this.Copyright = string.Empty;
			this.Webmaster = null;
			this.LastBuildDate = new DateTime();;
			this.Generator = string.Empty;
			this.Rating = string.Empty;
			this.ManagingEditor = null;
			this.Link = null;
			this.Name = string.Empty;
			this.Skipdays = new DayOfWeek[] {};
			this.Skiphours = new int[] {};
			this.Items = new List<FeedItem>();
			this.Image = null;
			this.Category = null;
		}
		public void CopyValues(Feed original)
		{
			if (original.Category != null) {
				this.Category = new Category();
				this.Category.Domain = original.Category.Domain;
				this.Category.Name = original.Category.Name;
			} else {
				this.Category = null;
			}
			this.Description = original.Description;
			this.Copyright = original.Copyright;
			this.Documentation = original.Documentation;
			this.Generator = original.Generator;
			if (original.Image != null) {
				this.Image = new Image();
				this.Image.Description = original.Image.Description;
				this.Image.Height = original.Image.Height;
				this.Image.Link = original.Image.Link;
				this.Image.Title = original.Image.Title;
				this.Image.URL = original.Image.URL;
				this.Image.Width = original.Image.Width;
			} else {
				this.Image = null;
			}
			if (original.Items != null) {
				this.Items = new List<FeedItem>();
				foreach (FeedItem item in original.Items) {
					FeedItem newItem = new FeedItem();
					newItem.CopyValues(item);
					this.Items.Add(newItem);
				}
			} else {
				this.Items = null;
			}
			this.Language = original.Language;
			this.LastBuildDate = original.LastBuildDate;
			this.Link = original.Link;
			this.ManagingEditor = original.ManagingEditor;
			this.Name = original.Name;
			this.PublishDate = original.PublishDate;
			this.Rating = original.Rating;
			this.Skipdays = MyHelper.EnumToArray(original.Skipdays);
			this.Skiphours = MyHelper.EnumToArray(original.Skiphours);
			this.Title = original.Title;
			this.Ttl = original.Ttl;
			this.Webmaster = original.Webmaster;
		}
		public override string ToString()
		{
			return this.Title;
		}
	}




	/// <summary>
	/// Stores data of a rss feed item.
	/// </summary>
	/// <remarks></remarks>
	public class FeedItem
	{

		/// <summary>
		/// The title of the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Title { get; set; }
		/// <summary>
		/// The URL of the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Link { get; set; }
		/// <summary>
		/// The item synopsis.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Description { get; set; }
		/// <summary>
		/// Email address of the author of the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Author { get; set; }
		/// <summary>
		/// Includes the item in one or more categories.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Category Category { get; set; }
		/// <summary>
		/// URL of a page for comments relating to the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Comments { get; set; }
		/// <summary>
		/// Describes a media object that is attached to the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Enclosure Enclosure { get; set; }
		/// <summary>
		/// A string that uniquely identifies the item.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public GUID GUID { get; set; }
		public System.DateTime InsertDate { get; set; }
		/// <summary>
		/// Indicates when the item was published.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public System.DateTime PublishDate { get; set; }
		/// <summary>
		/// The RSS channel that the item came from.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Source Source { get; set; }

		public FeedItem()
		{
			this.Clear();
		}

		public virtual void Clear()
		{
			this.Title = string.Empty;
			this.Link = null;
			this.Description = string.Empty;
			this.Author = string.Empty;
			this.InsertDate = new DateTime();;
			this.PublishDate = new DateTime();;
			this.Comments = null;
			this.GUID = null;
			this.Category = null;
			this.Enclosure = null;
			this.Source = null;
		}
		public virtual void CopyValues(FeedItem n)
		{
			this.Author = n.Author;
			if (n.Category != null) {
				this.Category = new Category();
				this.Category.Domain = n.Category.Domain;
				this.Category.Name = n.Category.Name;
			} else {
				this.Category = null;
			}
			this.Comments = n.Comments;
			this.Description = n.Description;
			if (n.Enclosure != null) {
				this.Enclosure = new Enclosure();
				this.Enclosure.Length = n.Enclosure.Length;
				this.Enclosure.Type = n.Enclosure.Type;
				this.Enclosure.Url = n.Enclosure.Url;
			} else {
				this.Enclosure = null;
			}
			this.GUID = n.GUID;
			this.InsertDate = n.InsertDate;
			this.Link = n.Link;
			this.PublishDate = n.PublishDate;
			this.Source = n.Source;
			this.Title = n.Title;
		}

		public override string ToString()
		{
			return this.Title;
		}
	}



	/// <summary>
	/// Provides information about a globally unique identifier. 
	/// </summary>
	/// <remarks></remarks>
	public class GUID
	{
		/// <summary>
		/// It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>There are no rules for the syntax of a guid. Aggregators must view them as a string. It's up to the source of the feed to establish the uniqueness of the string.</remarks>
		public string ID { get; set; }
		/// <summary>
		///  If the GUID element has an attribute named isPermaLink with a value of true, the reader may assume that it is a permalink to the item, that is, a url that can be opened in a Web browser, that points to the full item described by the FeedItem element.
		/// </summary>
		/// <value>If its value is FALSE, the guid may not be assumed to be a url, or a url to anything in particular.</value>
		/// <returns></returns>
		/// <remarks>IsPermaLink is optional, its default value is TRUE.</remarks>
		public bool IsPermaLink { get; set; }
		public GUID()
		{
			this.ID = string.Empty;
			this.IsPermaLink = true;
		}
		public override string ToString()
		{
			return this.ID;
		}
	}



	/// <summary>
	/// Enclosure describes a media object that is attached to the item. 
	/// </summary>
	/// <remarks></remarks>
	public class Enclosure
	{
		/// <summary>
		/// The location of the object.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Url { get; set; }
		/// <summary>
		/// The MIME type of the object.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Type { get; set; }
		/// <summary>
		/// The size in Bytes of the object.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public long Length { get; set; }
		public Enclosure()
		{
			this.Url = null;
			this.Type = string.Empty;
			this.Length = 0;
		}
		public override string ToString()
		{
			return this.Type;
		}
	}



	/// <summary>
	/// Provides information about the RSS channel where the item comes from.
	/// </summary>
	/// <remarks></remarks>
	public class Source
	{
		/// <summary>
		/// The title of the RSS channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Title { get; set; }
		/// <summary>
		/// The url of the RSS channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Url { get; set; }
		public Source()
		{
			this.Url = null;
			this.Title = string.Empty;
		}
		public override string ToString()
		{
			return this.Title;
		}
	}



	/// <summary>
	/// Provides information about catigorization of the item.
	/// </summary>
	/// <remarks></remarks>
	public class Category
	{
		/// <summary>
		/// The name of the category.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Name { get; set; }
		public Uri Domain { get; set; }
		public Category()
		{
			this.Name = string.Empty;
			this.Domain = null;
		}
		public override string ToString()
		{
			return this.Name;
		}
	}




	/// <summary>
	/// Provides RSS feed image data.
	/// </summary>
	/// <remarks></remarks>
	public class Image
	{
		/// <summary>
		/// Gets or sets the URL of a GIF, JPEG or PNG image that represents the channel.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri URL { get; set; }
		/// <summary>
		/// Gets or sets the URL of the site, when the channel is rendered, the image is a link to the site.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>In practice the image TITLE and LINK should have the same value as the channel's TITLE and LINK.</remarks>
		public Uri Link { get; set; }
		public string Title { get; set; }
		/// <summary>
		/// Gets or sets the text that is included in the TITLE attribute of the link formed around the image in the HTML rendering.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Description { get; set; }
		/// <summary>
		/// Gets or sets the width of the image in pixels.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>Maximum value is 144, default value is 88.</remarks>
		public int Width { get; set; }
		/// <summary>
		/// Gets or sets the height of the image in pixels.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>Maximum value is 400, default value is 31.</remarks>
		public int Height { get; set; }
	}



	/// <summary>
	/// It's purpose is to allow processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds.
	/// </summary>
	/// <remarks></remarks>
	public class Cloud
	{
		public Uri Domain { get; set; }
		public string Path { get; set; }
		public string RegisterProcedure { get; set; }
		public string Protocol { get; set; }
		public int Port { get; set; }
	}



	/// <summary>
	/// Represents a text input box for doing something.
	/// </summary>
	/// <remarks>The purpose of the TextInputBox element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.</remarks>
	public class TextInputBox
	{
		/// <summary>
		/// The label of the Submit button in the text input area.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Title { get; set; }
		/// <summary>
		/// Explains the text input area.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Description { get; set; }
		/// <summary>
		/// The name of the text object in the text input area.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Name { get; set; }
		/// <summary>
		/// The URL of the CGI script that processes text input requests.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Uri Link { get; set; }
	}



}
