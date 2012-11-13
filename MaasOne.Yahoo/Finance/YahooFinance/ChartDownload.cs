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


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading technical analysing chart images.
    /// </summary>
    /// <remarks></remarks>
    public partial class ChartDownload : Base.DownloadClient<ChartResult>
    {

        /// <summary>
        /// Gets or sets the chart image download options.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting null/Nothing, a default instance will be setted and used for downloading.</remarks>
        public ChartDownloadSettings Settings { get { return (ChartDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ChartDownload()
        {
            this.Settings = new ChartDownloadSettings();
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
            ChartDownloadSettings settings = (ChartDownloadSettings)this.Settings.Clone();
            settings.ID = unmanagedID;
            base.DownloadAsync(settings, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of an chart image.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userArgs">Individual user argument</param>
        public void DownloadAsync(ChartDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override ChartResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            return new ChartResult(MyHelper.CopyStream(stream));
        }


    }



    /// <summary>
    /// Stores the result data.
    /// </summary>
    public class ChartResult
    {


        private System.IO.MemoryStream mItem = null;
        public System.IO.MemoryStream Item
        {
            get { return mItem; }
        }
        internal ChartResult(System.IO.MemoryStream item)
        {
            mItem = item;
        }
    }



    /// <summary>
    /// Provides properties for setting options of chart download.
    /// </summary>
    /// <remarks></remarks>
    public class ChartDownloadSettings : Base.SettingsBase
    {

        private string mID = string.Empty;
        private Culture mCulture = null;
        private int mImageWidth = 300;
        private int mImageHeight = 180;
        private bool mSimplifiedImage = false;
        private ChartImageSize mImageSize = ChartImageSize.Middle;
        private ChartTimeSpan mTimeSpan = ChartTimeSpan.c1D;
        private ChartType mType = ChartType.Line;
        private ChartScale mScale = ChartScale.Logarithmic;
        private List<MovingAverageInterval> mMovingAverages = new List<MovingAverageInterval>();
        private List<MovingAverageInterval> mEMovingAverages = new List<MovingAverageInterval>();
        private List<TechnicalIndicator> mTechnicalIndicators = new List<TechnicalIndicator>();
        private List<string> mComparingIDs = new List<string>();

        public string ID
        {
            get
            {
                return mID;
            }
            set
            {
                mID = value;
            }
        }
        /// <summary>
        /// Gets or sets the used culture for scale descriptions. Can only be used with Server [USA].
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Culture Culture
        {
            get { return mCulture; }
            set { mCulture = value; }
        }
        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ImageWidth
        {
            get { return mImageWidth; }
            set { mImageWidth = value; }
        }
        /// <summary>
        /// Gets the height of the image
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ImageHeight
        {
            get { return mImageHeight; }
            set { mImageHeight = value; }
        }
        /// <summary>
        /// Gets a bool value if the image is simplified (1 day period; only ImageWidth, ImageHeight and Culture options available; Other options will be ignored)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SimplifiedImage
        {
            get { return mSimplifiedImage; }
            set { mSimplifiedImage = value; }
        }
        /// <summary>
        /// Gets the size of the image (only available if SimplifiedImage = FALSE)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartImageSize ImageSize
        {
            get { return mImageSize; }
            set { mImageSize = value; }
        }
        /// <summary>
        /// Gets the span of the reviewed period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartTimeSpan TimeSpan
        {
            get { return mTimeSpan; }
            set { mTimeSpan = value; }
        }
        /// <summary>
        /// Gets the chart type of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartType Type
        {
            get { return mType; }
            set { mType = value; }
        }
        /// <summary>
        /// Gets the scaling of the image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ChartScale Scale
        {
            get { return mScale; }
            set { mScale = value; }
        }
        /// <summary>
        /// Gets the list of moving average indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<MovingAverageInterval> MovingAverages
        {
            get { return mMovingAverages; }
            set { mMovingAverages = value; }
        }
        /// <summary>
        /// Gets the list of exponential moving average indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<MovingAverageInterval> ExponentialMovingAverages
        {
            get { return mEMovingAverages; }
            set { mEMovingAverages = value; }
        }
        /// <summary>
        /// Gets the list of technical indicators.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<TechnicalIndicator> TechnicalIndicators
        {
            get { return mTechnicalIndicators; }
            set { mTechnicalIndicators = value; }
        }
        /// <summary>
        /// Gets the ID list of all compared stocks/indices.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> ComparingIDs
        {
            get { return mComparingIDs; }
            set { mComparingIDs = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ChartDownloadSettings()
        {
            mCulture = Culture.DefaultCultures.UnitedStates_English;
        }
        public ChartDownloadSettings(string id)
        {
            mCulture = Culture.DefaultCultures.UnitedStates_English;
            this.ID = id;
        }
        protected override string GetUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            url.Append("http://");
            url.Append("chart.finance.yahoo.com/");

            if (this.SimplifiedImage) { url.Append("t?"); }
            else if (this.ImageSize == ChartImageSize.Small) { url.Append("h?"); }
            else { url.Append("z?"); }

            url.Append("s=");
            url.Append(MyHelper.CleanYqlParam(FinanceHelper.CleanIndexID(this.ID)));

            if (this.SimplifiedImage)
            {
                url.Append("&width=" + this.ImageWidth.ToString());
                url.Append("&height=" + this.ImageHeight.ToString());
            }
            else if (this.ImageSize != ChartImageSize.Small)
            {
                url.Append("&t=");
                url.Append(FinanceHelper.GetChartTimeSpan(this.TimeSpan));
                url.Append("&z=");
                url.Append(FinanceHelper.GetChartImageSize(this.ImageSize));
                url.Append("&q=");
                url.Append(FinanceHelper.GetChartType(this.Type));
                url.Append("&l=");
                url.Append(FinanceHelper.GetChartScale(this.Scale));
                if (this.MovingAverages.Count > 0 | this.ExponentialMovingAverages.Count > 0 | this.TechnicalIndicators.Count > 0)
                {
                    url.Append("&p=");
                    foreach (MovingAverageInterval ma in this.MovingAverages)
                    {
                        url.Append('m');
                        url.Append(FinanceHelper.GetMovingAverageInterval(ma));
                        url.Append(',');
                    }
                    foreach (MovingAverageInterval ma in this.ExponentialMovingAverages)
                    {
                        url.Append('e');
                        url.Append(FinanceHelper.GetMovingAverageInterval(ma));
                        url.Append(',');
                    }
                    foreach (TechnicalIndicator ti in this.TechnicalIndicators)
                    {
                        url.Append(FinanceHelper.GetTechnicalIndicatorsI(ti));
                    }
                }
                if (this.TechnicalIndicators.Count > 0)
                {
                    url.Append("&a=");
                    foreach (TechnicalIndicator ti in this.TechnicalIndicators)
                    {
                        url.Append(FinanceHelper.GetTechnicalIndicatorsII(ti));
                    }
                }
                if (this.ComparingIDs.Count > 0)
                {
                    url.Append("&c=");
                    foreach (string csid in this.ComparingIDs)
                    {
                        url.Append(MyHelper.CleanYqlParam(FinanceHelper.CleanIndexID(csid)));
                        url.Append(',');
                    }
                }
                if (this.Culture == null)
                {
                    this.Culture = Culture.DefaultCultures.UnitedStates_English;
                }
            }
            url.Append(YahooHelper.CultureToParameters(this.Culture));
            return url.ToString();
        }

        public override object Clone()
        {
            ChartDownloadSettings cln = new ChartDownloadSettings();
            cln.ID = this.ID;
            cln.SimplifiedImage = this.SimplifiedImage;
            cln.ImageWidth = this.ImageWidth;
            cln.ImageHeight = this.ImageHeight;
            cln.ImageSize = this.ImageSize;
            cln.TimeSpan = this.TimeSpan;
            cln.Type = this.Type;
            cln.Scale = this.Scale;
            cln.Culture = this.Culture;
            cln.MovingAverages.AddRange((MovingAverageInterval[])this.MovingAverages.ToArray().Clone());
            cln.ExponentialMovingAverages.AddRange((MovingAverageInterval[])this.ExponentialMovingAverages.ToArray().Clone());
            cln.TechnicalIndicators.AddRange((TechnicalIndicator[])this.TechnicalIndicators.ToArray().Clone());
            cln.ComparingIDs.AddRange((string[])this.ComparingIDs.ToArray().Clone());
            return cln;
        }



    }






}
