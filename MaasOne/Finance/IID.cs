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


namespace MaasOne.Finance
{
    /// <summary>
    /// Interface for Yahoo! ID. Can be used for downloading informations from Yahoo! Finance.
    /// </summary>
    /// <remarks></remarks>
    public interface IID
    {
        /// <summary>
        /// The valid Yahoo! ID.
        /// </summary>
        /// <value></value>
        /// <returns>The full ID built by the implementing class.</returns>
        /// <remarks></remarks>
        string ID { get; }
    }

    /// <summary>
    /// Interface for Yahoo! ID. Inherits from IID and provides a settable ID.
    /// </summary>
    /// <remarks></remarks>
    public interface ISettableID : IID
    {
        /// <summary>
        /// Provides the possibility to set the ID from outside of the class.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        void SetID(string id);
    }
}
