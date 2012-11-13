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
using System.Data;


namespace MaasOne.Finance.YahooPortfolio
{
    public partial class Portfolio
    {

        public DataTable GetTable()
        {           
            DataTable dt = new DataTable();
            if (mRows.Length > 0)
            {
                Dictionary<PortfolioColumnType, int> colDict = new Dictionary<PortfolioColumnType, int>(); foreach (PortfolioColumnType ct in Enum.GetValues(typeof(PortfolioColumnType))) colDict.Add(ct, -1);

                foreach (PortfolioColumnType colType in mColumns)
                {
                    colDict[colType] += 1;
                    string colName = colType.ToString();
                    if (colDict[colType] > 0) colName += "_" + colDict[colType].ToString();
                    dt.Columns.Add(colName, typeof(object)).Caption = Portfolio.GetColumnTypeTitle(colType, System.Globalization.CultureInfo.CurrentUICulture);
                }

                foreach (PortfolioDataRow pfRow in mRows)
                {
                    DataRow r = dt.NewRow();
                    for (int i = 0; i < mColumns.Length; i++)
                    {
                        r[i] = pfRow[mColumns[i]];
                    }
                    dt.Rows.Add(r);
                }

            }
            return dt;
        }

    }
}
