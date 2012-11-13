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
using System.Net;
using MaasOne.Base;
using MaasOne.Xml;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;
using MaasOne.Finance.YahooFinance.Support;


namespace MaasOne.Finance.YahooPortfolio
{
    public partial class YPortfolioManager
    {

        public Response<PortfolioInfoResult> DownloadPortfolioInfo()
        {
            if (this.IsLoggedIn)
            {
                PortfolioInfoDownload dl = new PortfolioInfoDownload();
                dl.Settings.Account = this;
                return dl.Download();
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }

        public Response<Portfolio> CreatePortfolio(string name) { return this.CreatePortfolio(name, null, null); }
        public Response<Portfolio> CreatePortfolio(string name, IEnumerable<YID> items, IEnumerable<YIndexID> indices) { return this.CreatePortfolio(name, WorldMarket.GetDefaultCurrencyByID("USD"), false, false, items, indices); }
        public Response<Portfolio> CreatePortfolio(string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                Response<XDocument> resp = upl.Upload(this.GetCreatePortfolioDownloadSettings(name, currency, symbolSorting, symbolCollapsing, items, indices));
                return ((DefaultResponse<XDocument>)resp).CreateNew(new PortfolioDownload().ConvertHtmlDoc(resp.Result));
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }


        public void EditPortfolio(string portfolioID, string name) { this.EditPortfolio(portfolioID, name, null, null); }
        public void EditPortfolio(string portfolioID, string name, IEnumerable<YID> items, IEnumerable<YIndexID> indices) { this.EditPortfolio(portfolioID, name, WorldMarket.GetDefaultCurrencyByID("USD"), false, false, items, indices); }
        public void EditPortfolio(string portfolioID, string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items = null, IEnumerable<YIndexID> indices = null)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.Upload(this.GetEditPortfolioDownloadSettings(portfolioID, name, currency, symbolSorting, symbolCollapsing, items, indices));
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }

        public Response<PortfolioInfoResult> DeletePortfolio(string portfolioID)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                DefaultResponse<XDocument> resp = (DefaultResponse<XDocument>)upl.Upload(this.GetDeletePortfolioDownloadSettings(portfolioID));
                return resp.CreateNew(new PortfolioInfoDownload().ConvertHtml(resp.Result));
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }

        public Response<Portfolio> DownloadPortfolio(PortfolioInfo portfolio)
        {
            return this.DownloadPortfolio(portfolio, 0);
        }
        public Response<Portfolio> DownloadPortfolio(PortfolioInfo portfolio, int viewIndex)
        {
            return this.DownloadPortfolio(portfolio.ID, viewIndex);
        }
        public Response<Portfolio> DownloadPortfolio(string portfolioID)
        {
            return this.DownloadPortfolio(portfolioID, 0);
        }
        public Response<Portfolio> DownloadPortfolio(string portfolioID, int viewIndex)
        {
            if (this.IsLoggedIn)
            {
                PortfolioDownload dl = new PortfolioDownload();
                dl.Settings.Account = this;
                dl.Settings.ViewIndex = viewIndex;
                dl.Settings.PortfolioID = portfolioID;
                return dl.Download();
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        public Response<Portfolio> AddPortfolioItem(PortfolioInfo portfolio, string itemID) { return this.AddPortfolioItem(portfolio.ID, itemID); }
        public Response<Portfolio> AddPortfolioItem(string portfolioID, string itemID)
        {
            Html2XmlDownload html = new Html2XmlDownload();
            html.Settings.Account = this;
            html.Settings.Url = string.Format("http://finance.yahoo.com/portfolio/add_symbols?portfolio_id={0}&portfolio_view_id=v1&quotes={1}", portfolioID, itemID);
            Response<XDocument> resp = html.Download();
            return ((DefaultResponse<XDocument>)resp).CreateNew(new PortfolioDownload().ConvertHtmlDoc(resp.Result));
        }

        public void DeletePortfolioItem(string portfolioID, string itemID) { this.DeletePortfolioItem(portfolioID, itemID, 0); }
        public void DeletePortfolioItem(string portfolioID, string itemID, int idIndex)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.Upload(this.GetDeletePortfolioItemDownloadSettings(portfolioID, itemID, idIndex));
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }

        public void EditHoldings(string portfolioID, Holding[] holdings)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.Upload(this.GetEditHoldingsDownloadSettings(portfolioID, holdings));
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }

        public Response<HoldingsResult> DownloadHoldings(string portfolioID, int viewIndex)
        {
            if (this.IsLoggedIn)
            {
                HoldingsDownload dl = new HoldingsDownload();
                dl.Settings.Account = this;
                dl.Settings.PortfolioID = portfolioID;
                return dl.Download();
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }


    }




}
