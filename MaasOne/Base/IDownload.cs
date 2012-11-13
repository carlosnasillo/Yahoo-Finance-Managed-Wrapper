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


namespace MaasOne.Base
{
    public delegate void AsyncDownloadCompletedEventHandler(IDownload sender, IDownloadCompletedEventArgs e);

    public partial interface IDownload
    {
        event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedEvent;

        SettingsBase Settings { get; }

        /// <summary>
        /// Gets the user arguments of each download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If you just need a single object, you should use UserArgs(Int32) property, because of higher performance.</remarks>
        object[] UserArgs();
        /// <summary>
        /// Gets the user argument of a download process at a specific index position.
        /// </summary>
        /// <param name="index">The index position of the download</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        object UserArgs(int index);

        /// <summary>
        /// Gets the number of actually running asynchronous downloads.
        /// </summary>
        /// <value></value>
        /// <returns>The number of active downloads</returns>
        /// <remarks></remarks>
        int AsyncDownloadsCount{get;}

        /// <summary>
        /// Cancels all running downloads.
        /// </summary>
        /// <returns>TRUE, if all downloads were canceled. FALSE, if something goes wrong.</returns>
        /// <remarks></remarks>
        bool CancelAsyncAll();
        /// <summary>
        /// Cancels all running downloads containing the passed user argument.
        /// </summary>
        /// <param name="userArgs">The user argument that the download object contains.</param>
        /// <returns>The number of canceled downloads.</returns>
        /// <remarks></remarks>
        int CancelAsync(object userArgs);
        /// <summary>
        /// Cancels a running download at specific index position.
        /// </summary>
        /// <param name="index">The index position of the download process in queue.</param>
        /// <remarks></remarks>
        void CancelAsyncAt(int index);

        /// <summary>
        /// Proves if the download processes contains the passed user argument.
        /// </summary>
        /// <param name="userArgs">Individual user argument that the download contains.</param>
        /// <returns>The number of download processes containing the passed user argument.</returns>
        /// <remarks></remarks>
        int Contains(object userArgs);

        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        void DownloadAsync();
        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        /// <param name="userArgs">Individual user arguments.</param>
        void DownloadAsync(object userArgs);

    }

    public interface IResponse
    {
        ConnectionInfo Connection { get; }
        object GetObjectResult();
    }

    public interface IDownloadCompletedEventArgs
    {
        IResponse GetResponse();
        object UserArgs { get; }
        SettingsBase Settings { get; }
    }
}
