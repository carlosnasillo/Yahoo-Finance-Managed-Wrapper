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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Net;
using MaasOne;


namespace MaasOne.Base
{
    /// <summary>
    /// Provides methods and properties for downloading data and controlling these download processes. This class must be inherited.
    /// </summary>
    /// <remarks>This class can not be instanced without inheritation</remarks>
    public abstract partial class DownloadClient<T> : IDownload
    {

        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompleted;
        public delegate void AsyncDownloadCompletedEventHandler(DownloadClient<T> sender, DownloadCompletedEventArgs<T> e);

        public event MaasOne.Base.AsyncDownloadCompletedEventHandler AsyncDownloadCompletedEvent;


        private readonly List<TimeoutWebClient<T>> mWebClients = new List<TimeoutWebClient<T>>();

        /// <summary>
        /// Gets the user arguments of each download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If you just need a single object, you should use UserArgs(Int32) property, because of higher performance.</remarks>
        public object[] UserArgs()
        {
            object[] res = new object[mWebClients.Count - 1];
            if (mWebClients.Count > 0)
            {
                for (int i = 0; i < mWebClients.Count; i++)
                {
                    res[i] = this.GetDeepUserArgs(mWebClients[i].UserArgs);
                }
            }
            return res;
        }
        /// <summary>
        /// Gets the user argument of a download process at a specific index position.
        /// </summary>
        /// <param name="index">The index position of the download</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object UserArgs(int index)
        {
            return this.GetDeepUserArgs(mWebClients[index].UserArgs);
        }
        private int mTimeout = 30000;
        /// <summary>
        /// Gets or sets the time in miliseconds when the download will cancel if the time reached before it is completed.
        /// </summary>
        /// <value>Timeout in miliseconds. If value is '-1', the next download wont have a timeout.</value>
        /// <returns>The timeout in miliseconds.</returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get { return mTimeout; }
            set
            {
                if (value >= 0)
                {
                    mTimeout = value;
                }
                else
                {
                    mTimeout = 0;
                }
            }
        }
        /// <summary>
        /// Gets the number of actually running asynchronous downloads.
        /// </summary>
        /// <value></value>
        /// <returns>The number of active downloads</returns>
        /// <remarks></remarks>
        public int AsyncDownloadsCount
        {
            get { return mWebClients.Count; }
        }
        /// <summary>
        /// Cancels all running downloads.
        /// </summary>
        /// <returns>TRUE, if all downloads were canceled. FALSE, if something goes wrong.</returns>
        /// <remarks></remarks>
        public bool CancelAsyncAll()
        {
            if (mWebClients.Count > 0)
            {
                for (int i = mWebClients.Count - 1; i >= 0; i += -1)
                {
                    mWebClients[i].CancelAsync();
                }
            }
            return mWebClients.Count == 0;
        }
        /// <summary>
        /// Cancels all running downloads containing the passed user argument.
        /// </summary>
        /// <param name="userArgs">The user argument that the download object contains.</param>
        /// <returns>The number of canceled downloads.</returns>
        /// <remarks></remarks>
        public int CancelAsync(object userArgs)
        {
            if (mWebClients.Count > 0)
            {
                int count = 0;
                for (int i = mWebClients.Count; i >= 0; i += -1)
                {
                    if (this.DeepUserArgsEqual(mWebClients[i].UserArgs, userArgs))
                    {
                        mWebClients[i].CancelAsync();
                        count += 1;
                    }
                }
                return count;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Cancels a running download at specific index position.
        /// </summary>
        /// <param name="index">The index position of the download process in queue.</param>
        /// <remarks></remarks>
        public void CancelAsyncAt(int index)
        {
            mWebClients[index].CancelAsync();
        }
        /// <summary>
        /// Proves if the download processes contains the passed user argument.
        /// </summary>
        /// <param name="userArgs">Individual user argument that the download contains.</param>
        /// <returns>The number of download processes containing the passed user argument.</returns>
        /// <remarks></remarks>
        public int Contains(object userArgs)
        {
            int count = 0;
            foreach (TimeoutWebClient<T> wc in mWebClients)
            {
                if (this.DeepUserArgsEqual(wc.UserArgs, userArgs))
                {
                    count += 1;
                }
            }
            return count;
        }

        private SettingsBase mSettings = null;
        public SettingsBase Settings { get { return mSettings; } }
        protected void SetSettings(SettingsBase value)
        {
            mSettings = value;
        }

        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        public void DownloadAsync()
        {
            this.DownloadAsync(null);
        }
        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        /// <param name="userArgs">Individual user arguments.</param>
        public void DownloadAsync(object userArgs)
        {
            if (this.Settings == null) { throw new ArgumentNullException("Settings", "The settings for downloading with " + this.GetType().Name + " are null."); }
            this.DownloadAsync((SettingsBase)this.Settings.Clone(), userArgs);
        }

        protected void DownloadAsync(SettingsBase settings, object userArgs)
        {
            if (settings == null) { throw new ArgumentNullException("settings", "The settings for downloading with " + this.GetType().Name + " are null."); }
            TimeoutWebClient<T> wc = new TimeoutWebClient<T>(mTimeout);
            wc.AsyncDownloadCompleted += this.AsyncDownload_Completed;
            mWebClients.Add(wc);
            wc.DownloadAsync(settings, userArgs);
        }

        private void AsyncDownload_Completed(TimeoutWebClient<T> sender, StreamDownloadCompletedEventArgs<T> e)
        {
            using (TimeoutWebClient<T> snd = sender)
            {
                snd.AsyncDownloadCompleted -= this.AsyncDownload_Completed;
                if (mWebClients.Contains(sender)) mWebClients.Remove(sender);
                using (System.IO.Stream stream = (e.Response.Result != null ? e.Response.Result : new System.IO.MemoryStream()))
                {
                    ConnectionInfo conn = e.Response.Connection;
                    T result = default(T);
                    try
                    {
                        result = this.ConvertResult(e.Response.Connection, stream, e.UserSettings);
                    }
                    catch (Exception ex)
                    {
                        conn = new ConnectionInfo(new ConversionException("An exception was thrown during result conversion process. See InnerException for more details.", ex), conn.Timeout, conn.SizeInBytes, conn.StartTime, conn.EndTime);
                    }
                    finally
                    {
                        Response<T> resp = this.ConvertResponse(new DefaultResponse<T>(conn, result));
                        DownloadCompletedEventArgs<T> args = this.ConvertDownloadCompletedEventArgs(new DefaultDownloadCompletedEventArgs<T>(e.UserArgs, resp, e.UserSettings));
                        if (args != null && (AsyncDownloadCompleted != null || AsyncDownloadCompletedEvent != null))
                        {
                            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(this);
                            if (AsyncDownloadCompleted != null) asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncDownloadCompleted(this, (DownloadCompletedEventArgs<T>)obj); }), args);
                            if (AsyncDownloadCompletedEvent != null) asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncDownloadCompletedEvent(this, (IDownloadCompletedEventArgs)obj); }), args);
                        }
                    }
                }
            }
        }

        protected virtual DownloadCompletedEventArgs<T> ConvertDownloadCompletedEventArgs(DefaultDownloadCompletedEventArgs<T> e) { return e; }
        protected virtual Response<T> ConvertResponse(DefaultResponse<T> response) { return response; }
        protected abstract T ConvertResult(ConnectionInfo connInfo, System.IO.Stream stream, SettingsBase settings);

        private bool DeepUserArgsEqual(object argsX, object argsY)
        {
            return object.ReferenceEquals(this.GetDeepUserArgs(argsX), this.GetDeepUserArgs(argsY));
        }
        private object GetDeepUserArgs(object args)
        {
            if (args != null && args is DownloadEventArgs) { return this.GetDeepUserArgs(((DownloadEventArgs)args).UserArgs); }
            else { return args; }
        }

    }




}
