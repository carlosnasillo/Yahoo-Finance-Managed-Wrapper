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


namespace MaasOne
{

    /// <summary>
    /// Provides properties for decoding text from streams.
    /// </summary>
    public interface ITextEncodingSettings
    {
        /// <summary>
        /// The used text encoding.
        /// </summary>
        System.Text.Encoding TextEncoding { get; set; }
    }

    /// <summary>
    /// Provides properties for a webservice query.
    /// </summary>
    public interface IQuerySettings
    {
        /// <summary>
        /// The query text.
        /// </summary>
        string Query { get; set; }
    }



    /// <summary>
    /// Provides properties to set the start index and count number for a query in results queue.
    /// </summary>
    public interface IResultIndexSettings
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }

}
