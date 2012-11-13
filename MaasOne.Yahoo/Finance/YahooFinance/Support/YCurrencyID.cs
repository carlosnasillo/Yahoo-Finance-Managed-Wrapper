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
using System.ComponentModel;


namespace MaasOne.Finance.YahooFinance.Support
{
    /// <summary>
    /// Stores informations about base and dependency currency. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YCurrencyID : IID, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CurrencyInfo mBaseCurrency;
        private CurrencyInfo mDepCurrency;

        /// <summary>
        ///The Yahoo ID of the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0}{1}=X", mBaseCurrency.ID, mDepCurrency.ID);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// The currency with the ratio value of 1
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo BaseCurrency
        {
            get { return mBaseCurrency; }
            set { mBaseCurrency = value; this.OnPropertyChanged("BaseCurrency"); }
        }
        /// <summary>
        /// The currency of the dependent value
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo DepCurrency
        {
            get { return mDepCurrency; }
            set { mDepCurrency = value; this.OnPropertyChanged("DepCurrency"); }
        }
        /// <summary>
        /// The display name of the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DisplayName
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0} to {1}", mBaseCurrency.Description, mDepCurrency.Description);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public YCurrencyID()
        {
            mBaseCurrency = null;
            mDepCurrency = null;
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="baseCur"></param>
        /// <param name="depCur"></param>
        /// <remarks></remarks>
        public YCurrencyID(CurrencyInfo baseCur, CurrencyInfo depCur)
        {
            this.BaseCurrency = baseCur;
            this.DepCurrency = depCur;
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id"></param>
        /// <remarks></remarks>
        public YCurrencyID(string id)
        {
            YCurrencyID newRel = FinanceHelper.YCurrencyIDFromString(id);
            if (newRel != null)
            {
                this.BaseCurrency = newRel.BaseCurrency;
                this.DepCurrency = newRel.DepCurrency;
            }
            else
            {
                throw new ArgumentException("The id is not valid", "id");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns the ID of the currency relation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ID;
        }
    }
}
