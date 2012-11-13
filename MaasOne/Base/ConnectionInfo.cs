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
using System.Net;


namespace MaasOne.Base
{
    /// <summary>
    /// Provides information about a download process.
    /// </summary>
    /// <remarks></remarks>
    public partial class ConnectionInfo : ICloneable
    {


        private Exception mException;
        private int mTimeout = 0;
        private int mSizeInBytes = 0;
        private DateTime mStartTime;
        private DateTime mEndTime;

        private TimeSpan mTimeSpan;
        /// <summary>
        /// Indicates the connection status.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConnectionState State
        {
            get
            {
                if (mException == null)
                {
                    return ConnectionState.Success;
                }
                else
                {
                    if (mException is System.Net.WebException)
                    {
                        System.Net.WebException exp = (System.Net.WebException)mException;
                        if (exp.Status == WebExceptionStatus.RequestCanceled) { return ConnectionState.Canceled; }
                        else if (exp.Status == TimeoutWebClient<object>.GetTimeoutStatus()) { return ConnectionState.Timeout; }
                        else { return ConnectionState.ErrorOccured; }
                    }
                    else
                    {
                        return ConnectionState.ErrorOccured;
                    }

                }
            }
        }
        /// <summary>
        /// If an exception occurs during download process, the exception object will be stored here. If no exception occurs, this property is null/Nothing.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Exception Exception
        {
            get { return mException; }
        }
        /// <summary>
        /// The size of downloaded data in bytes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int SizeInBytes
        {
            get { return mSizeInBytes; }
        }
        /// <summary>
        /// The setted timeout for download process in milliseconds.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get { return mTimeout; }
        }
        /// <summary>
        /// The start time of download process, independent to individual preparation of passed parameters for start downloading.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime StartTime
        {
            get { return mStartTime; }
        }
        /// <summary>
        /// The end time of the download process, independent to post-processing actions like parsing.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime EndTime
        {
            get { return mEndTime; }
        }
        /// <summary>
        /// The time span of start and end time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan TimeSpan
        {
            get { return mTimeSpan; }
        }
        public double KBPerSecond
        {
            get
            {
                if (this.TimeSpan.TotalMilliseconds != 0)
                {
                    return mSizeInBytes / this.TimeSpan.TotalMilliseconds;
                }
                else
                {
                    return 0;
                }
            }
        }


        public ConnectionInfo(Exception exception, int timeout, int size, DateTime startTime, DateTime endTime)
        {
            mException = exception;
            mTimeout = timeout;
            mSizeInBytes = size;
            mEndTime = endTime;
            mStartTime = startTime;
            mTimeSpan = mEndTime - mStartTime;
        }

        public virtual object Clone()
        {
            return new ConnectionInfo(this.Exception, this.Timeout, this.SizeInBytes, this.StartTime, this.EndTime);
        }

    }
}
