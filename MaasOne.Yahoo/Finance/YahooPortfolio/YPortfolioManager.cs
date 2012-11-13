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
using System.IO;
using MaasOne.Base;
using MaasOne.Xml;
using MaasOne.Finance;
using MaasOne.Finance.YahooFinance;
using MaasOne.Finance.YahooFinance.Support;
using System.Xml.Linq;

namespace MaasOne.Finance.YahooPortfolio
{
    public partial class YPortfolioManager : YAccountManager
    {
        public delegate void AsyncPortfolioDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs<Portfolio> e);
        public delegate void AsyncPortfolioInfoDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs<PortfolioInfoResult> e);
        public delegate void AsyncHoldingsDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs<HoldingsResult> e);
        public delegate void AsyncPostUploadCompletedEventHandler(object sender, DownloadEventArgs e);


        public event AsyncPortfolioDownloadCompletedEventHandler AsyncPortfolioDownloadCompleted;
        public event AsyncPortfolioDownloadCompletedEventHandler AsyncEditHoldingsCompleted;
        public event AsyncPortfolioDownloadCompletedEventHandler AsyncAddPortfolioItemCompleted;
        public event AsyncPortfolioDownloadCompletedEventHandler AsyncCreatePortfolioCompleted;
        public event AsyncPortfolioDownloadCompletedEventHandler AsyncEditPortfolioViewCompleted;
        public event AsyncPortfolioDownloadCompletedEventHandler AsyncDeletePortfolioViewCompleted;

        public event AsyncPostUploadCompletedEventHandler AsyncEditPortfolioCompleted;
        public event AsyncPostUploadCompletedEventHandler AsyncDeletePortfolioItemCompleted;

        public event AsyncPortfolioInfoDownloadCompletedEventHandler AsyncPortfolioInfoDownloadCompleted;
        public event AsyncPortfolioInfoDownloadCompletedEventHandler AsyncDeletePortfolioCompleted;

        public event AsyncHoldingsDownloadCompletedEventHandler AsyncHoldingsDownloadCompleted;



        public YPortfolioManager() { }


        public void DownloadPortfolioInfoAsync(object userArgs)
        {
            if (this.IsLoggedIn)
            {
                PortfolioInfoDownload dl = new PortfolioInfoDownload();
                dl.Settings.Account = this;
                dl.AsyncDownloadCompleted += this.PortfolioInfoDownload_Completed;
                dl.DownloadAsync(userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private void PortfolioInfoDownload_Completed(DownloadClient<PortfolioInfoResult> sender, DownloadCompletedEventArgs<PortfolioInfoResult> e)
        {
            sender.AsyncDownloadCompleted -= this.PortfolioInfoDownload_Completed;
            if (this.AsyncPortfolioInfoDownloadCompleted != null) this.AsyncPortfolioInfoDownloadCompleted(this, e);
        }


        public void CreatePortfolioAsync(string name, object userArgs) { this.CreatePortfolioAsync(name, null, null, userArgs); }
        public void CreatePortfolioAsync(string name, IEnumerable<YID> items, IEnumerable<YIndexID> indices, object userArgs) { this.CreatePortfolioAsync(name, WorldMarket.GetDefaultCurrencyByID("USD"), false, false, items, indices, userArgs); }
        public void CreatePortfolioAsync(string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.CreatePortfolio_DownloadCompleted;
                upl.UploadAsync(this.GetCreatePortfolioDownloadSettings(name, currency, symbolSorting, symbolCollapsing, items, indices), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetCreatePortfolioDownloadSettings(string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices)
        {
            List<KeyValuePair<string, string>> lst = this.GetPortfolioDict(name, currency, symbolSorting, symbolCollapsing, items, indices, "");
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com/portfolio/new";
            settings.RefererUrlPart = "/portfolio/save-new";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            settings.DownloadResponse = true;
            return settings;
        }
        private void CreatePortfolio_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.CreatePortfolio_DownloadCompleted;
            Portfolio pf = new PortfolioDownload().ConvertHtmlDoc(e.Response.Result);
            if (this.AsyncCreatePortfolioCompleted != null) this.AsyncCreatePortfolioCompleted(this, e.CreateNew(pf));
        }

        public void EditPortfolioAsync(string portfolioID, string name, object userArgs) { this.EditPortfolioAsync(portfolioID, name, null, null, userArgs); }
        public void EditPortfolioAsync(string portfolioID, string name, IEnumerable<YID> items, IEnumerable<YIndexID> indices, object userArgs) { this.EditPortfolioAsync(portfolioID, name, WorldMarket.GetDefaultCurrencyByID("USD"), false, false, items, indices, userArgs); }
        public void EditPortfolioAsync(string portfolioID, string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.EditPortfolio_DownloadCompleted;
                upl.UploadAsync(this.GetEditPortfolioDownloadSettings(portfolioID, name, currency, symbolSorting, symbolCollapsing, items, indices), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetEditPortfolioDownloadSettings(string portfolioID, string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices)
        {
            List<KeyValuePair<string, string>> lst = this.GetPortfolioDict(name, currency, symbolSorting, symbolCollapsing, items, indices, portfolioID);
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com/portfolio/" + portfolioID + "/edit";
            settings.RefererUrlPart = "/portfolio/" + portfolioID + "/save-edit";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            return settings;
        }
        private void EditPortfolio_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.EditPortfolio_DownloadCompleted;
            if (this.AsyncEditPortfolioCompleted != null) this.AsyncEditPortfolioCompleted(this, new DownloadEventArgs(e.UserArgs));
        }


        public void DeletePortfolioAsync(string portfolioID, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.DeletePortfolio_DownloadCompleted;
                upl.UploadAsync(this.GetDeletePortfolioDownloadSettings(portfolioID), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetDeletePortfolioDownloadSettings(string portfolioID)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".yfiuseajax", "0"));
            lst.Add(new KeyValuePair<string, string>(".yfisrc", ""));
            lst.Add(new KeyValuePair<string, string>(".yfidone", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_id", portfolioID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_symbol", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_lot", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_view_id", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_quotes_symbols", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_yes", ""));
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com/portfolio/confirm/delete/" + portfolioID;
            settings.RefererUrlPart = "/portfolio/" + portfolioID + "/delete";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            settings.DownloadResponse = true;
            return settings;
        }
        private void DeletePortfolio_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.DeletePortfolio_DownloadCompleted;
            if (this.AsyncDeletePortfolioCompleted != null) this.AsyncDeletePortfolioCompleted(this, e.CreateNew(new PortfolioInfoDownload().ConvertHtml(e.Response.Result)));
        }


        public void AddPortfolioViewAsync(string portfolioID, string name, IEnumerable<PortfolioColumnType> viewFields, object userArgs) { this.EditPortfolioViewAsync(portfolioID, -1, name, viewFields, userArgs); }
        public void EditPortfolioViewAsync(string portfolioID, int viewIndex, string name, IEnumerable<PortfolioColumnType> viewFields, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.EditPortfolioView_DownloadCompleted;
                upl.UploadAsync(this.GetEditPortfolioViewDownloadSettings(portfolioID, viewIndex, name, viewFields), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetEditPortfolioViewDownloadSettings(string portfolioID, int viewIndex, string name, IEnumerable<PortfolioColumnType> viewFields)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".yficrumb", ""));
            lst.Add(new KeyValuePair<string, string>("id", (viewIndex > -1 ? "v" + (viewIndex + 1) : "")));
            lst.Add(new KeyValuePair<string, string>("from_view_id", ""));
            lst.Add(new KeyValuePair<string, string>("portfolio_id", portfolioID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_fields_name", name));
            PortfolioColumnType[] enm = MyHelper.EnumToArray(viewFields);
            for (int i = 0; i < 14; i++)
            {
                if (enm.Length > i) { lst.Add(new KeyValuePair<string, string>("yfi_pf_fields[]", enm[i].ToString())); }
                else { lst.Add(new KeyValuePair<string, string>("yfi_pf_fields[]", "")); }
            }
            lst.Add(new KeyValuePair<string, string>("save", ""));

            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com/quotes/view/" + (viewIndex > -1 ? "v" + (viewIndex + 1) : "new");
            settings.RefererUrlPart = "/quotes/view/edit-save";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            settings.DownloadResponse = true;
            return settings;
        }
        private void EditPortfolioView_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.EditPortfolioView_DownloadCompleted;

            Portfolio pf = new PortfolioDownload().ConvertHtmlDoc(e.Response.Result);

            if (this.AsyncEditPortfolioViewCompleted != null) this.AsyncEditPortfolioViewCompleted(this, e.CreateNew<Portfolio>(pf));
        }

        public void DeletePortfolioViewAsync(int viewIndex, object userArgs) { this.DeletePortfolioViewAsync(viewIndex, userArgs); }
        public void DeletePortfolioViewAsync(string portfolioID, int viewIndex, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.DeletePortfolioView_DownloadCompleted;
                upl.UploadAsync(this.GetDeletePortfolioViewDownloadSettings(portfolioID, viewIndex), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetDeletePortfolioViewDownloadSettings(string portfolioID, int viewIndex)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".yficrumb", ""));
            lst.Add(new KeyValuePair<string, string>(".yfiuseajax", "0"));
            lst.Add(new KeyValuePair<string, string>(".yfisrc", ""));
            lst.Add(new KeyValuePair<string, string>(".yfidone", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_id", portfolioID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_symbol", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_lot", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_view_id", "v" + (viewIndex + 1).ToString()));
            lst.Add(new KeyValuePair<string, string>("yfi_quotes_symbols", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_yes", ""));

            string view = "v" + (viewIndex + 1);
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com/portfolio/confirm/delete-view?view_id=" + view + "&portfolio_id=" + portfolioID;
            settings.RefererUrlPart = "/quotes/view/" + view + "/delete";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            settings.DownloadResponse = true;
            return settings;
        }
        private void DeletePortfolioView_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.DeletePortfolioView_DownloadCompleted;
            Portfolio pf = new PortfolioDownload().ConvertHtmlDoc(e.Response.Result);
            if (this.AsyncDeletePortfolioViewCompleted != null) this.AsyncDeletePortfolioViewCompleted(this, e.CreateNew<Portfolio>(pf));
        }

        public void DownloadPortfolioAsync(PortfolioInfo portfolio, object userArgs) { this.DownloadPortfolioAsync(portfolio, 0, userArgs); }
        public void DownloadPortfolioAsync(PortfolioInfo portfolio, int viewIndex, object userArgs) { this.DownloadPortfolioAsync(portfolio, viewIndex, false, false, userArgs); }
        public void DownloadPortfolioAsync(PortfolioInfo portfolio, int viewIndex, bool dlRealTime, bool dlFundamentals, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                PortfolioDownload dl = new PortfolioDownload();
                dl.Settings.Account = this;
                dl.Settings.ViewIndex = viewIndex;
                dl.Settings.PortfolioID = portfolio.ID;
                dl.Settings.DownloadFundamentalsView = dlFundamentals;
                dl.Settings.DownloadRealTimeView = dlRealTime;
                dl.AsyncDownloadCompleted += this.PortfolioDownload_DownloadCompleted;
                dl.DownloadAsync(userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private void PortfolioDownload_DownloadCompleted(DownloadClient<Portfolio> sender, DownloadCompletedEventArgs<Portfolio> e)
        {
            sender.AsyncDownloadCompleted -= this.PortfolioDownload_DownloadCompleted;
            if (this.AsyncPortfolioDownloadCompleted != null) this.AsyncPortfolioDownloadCompleted(this, e);
        }


        public void AddPortfolioItemAsync(PortfolioInfo portfolio, string itemID, object userArgs) { this.AddPortfolioItemAsync(portfolio.ID, itemID, userArgs); }
        public void AddPortfolioItemAsync(string portfolioID, string itemID, object userArgs)
        {
            Html2XmlDownload html = new Html2XmlDownload();
            html.Settings.Account = this;
            html.Settings.Url = string.Format("http://finance.yahoo.com/portfolio/add_symbols?portfolio_id={0}&portfolio_view_id=v1&quotes={1}", portfolioID, itemID);
            html.AsyncDownloadCompleted += this.AddPortfolioItem_Completed;
            html.DownloadAsync(new AddPfItemAsyncArgs(userArgs) { PortfolioID = portfolioID });

        }
        private void AddPortfolioItem_Completed(DownloadClient<XDocument> sender, DownloadCompletedEventArgs<XDocument> e)
        {
            AddPfItemAsyncArgs args = (AddPfItemAsyncArgs)e.UserArgs;
            Portfolio pf = new PortfolioDownload().ConvertHtmlDoc(e.Response.Result);
            if (this.AsyncAddPortfolioItemCompleted != null) this.AsyncAddPortfolioItemCompleted(this, ((DefaultDownloadCompletedEventArgs<XDocument>)e).CreateNew(pf));
        }


        public void DeletePortfolioItemAsync(string portfolioID, string itemID, object userArgs) { this.DeletePortfolioItemAsync(portfolioID, itemID, 0, userArgs); }
        public void DeletePortfolioItemAsync(string portfolioID, string itemID, int idIndex, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.DeletePortfolioItem_DownloadCompleted;
                upl.UploadAsync(this.GetDeletePortfolioItemDownloadSettings(portfolioID, itemID, idIndex), userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetDeletePortfolioItemDownloadSettings(string portfolioID, string itemID, int idIndex)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".yfiuseajax", "1"));
            lst.Add(new KeyValuePair<string, string>(".yfisrc", "/"));
            lst.Add(new KeyValuePair<string, string>(".yfidone", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_id", portfolioID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_symbol", itemID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_lot", idIndex.ToString()));
            lst.Add(new KeyValuePair<string, string>("yfi_yes", "1"));
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com" + "/portfolio/" + portfolioID + "/view/v1";
            settings.RefererUrlPart = "/portfolio/" + portfolioID + "/holdings/delete/" + Uri.EscapeDataString(itemID);
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "action=\"/portfolio/delete_symbols\"";
            settings.DownloadResponse = true;
            return settings;
        }
        private void DeletePortfolioItem_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.DeletePortfolioItem_DownloadCompleted;
            if (this.AsyncDeletePortfolioItemCompleted != null) this.AsyncDeletePortfolioItemCompleted(this, new DownloadEventArgs(e.UserArgs));



        }



        public void EditHoldingsAsync(string portfolioID, Holding[] holdings, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                WebFormUpload upl = new WebFormUpload();
                upl.AsyncUploadCompleted += this.EditHoldings_DownloadCompleted;
                upl.UploadAsync(this.GetEditHoldingsDownloadSettings(portfolioID, holdings), new EditHoldingsAyncArgs(userArgs) { Info = portfolioID });
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private WebFormDownloadSettings GetEditHoldingsDownloadSettings(string portfolioID, Holding[] holdings)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            lst.Add(new KeyValuePair<string, string>("yfi_pf_action", "add_holdings"));
            lst.Add(new KeyValuePair<string, string>(".yfisrc", "/"));
            lst.Add(new KeyValuePair<string, string>(".yfidone", ""));
            lst.Add(new KeyValuePair<string, string>(".yficurr", ""));
            lst.Add(new KeyValuePair<string, string>(".yficrumb", ""));
            foreach (Holding holding in holdings)
            {
                lst.Add(new KeyValuePair<string, string>("yfi_pf_symbol[]", holding.ID));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_lot[]", holding.Lot.ToString()));
                lst.Add(new KeyValuePair<string, string>("date[]", ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_trade_date_day[]", holding.TradeDate.HasValue ? holding.TradeDate.Value.Day.ToString() : DateTime.Today.Day.ToString()));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_trade_date_month[]", holding.TradeDate.HasValue ? holding.TradeDate.Value.Month.ToString() : DateTime.Today.Month.ToString()));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_trade_date_year[]", holding.TradeDate.HasValue ? holding.TradeDate.Value.Year.ToString() : DateTime.Today.Year.ToString()));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_shares_owned[]", holding.Shares != 0 ? holding.Shares.ToString() : ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_price_paid[]", holding.PricePaid != 0 ? holding.PricePaid.ToString(ci) : ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_commission[]", holding.Commission != 0 ? holding.Commission.ToString(ci) : ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_low_limit[]", holding.LowLimit != 0 ? holding.LowLimit.ToString(ci) : ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_high_limit[]", holding.HighLimit != 0 ? holding.HighLimit.ToString(ci) : ""));
                lst.Add(new KeyValuePair<string, string>("yfi_pf_comment[]", holding.Notes != string.Empty ? holding.Notes : ""));
            }
            lst.Add(new KeyValuePair<string, string>("save", "Save"));
            string urlPart = "/portfolio/" + portfolioID + "/holdings";
            WebFormDownloadSettings settings = new WebFormDownloadSettings();
            settings.Account = this;
            settings.Url = "http://finance.yahoo.com" + urlPart + "/edit";
            settings.RefererUrlPart = urlPart + "/update";
            settings.AdditionalWebForms = lst;
            settings.SearchForWebForms = null;
            settings.FormActionPattern = "";
            settings.DownloadResponse = true;
            return settings;
        }
        private void EditHoldings_DownloadCompleted(WebFormUpload sender, DefaultDownloadCompletedEventArgs<XDocument> e)
        {
            sender.AsyncUploadCompleted -= this.EditHoldings_DownloadCompleted;
            EditHoldingsAyncArgs args = (EditHoldingsAyncArgs)e.UserArgs;
            if (this.AsyncEditHoldingsCompleted != null) AsyncEditHoldingsCompleted(this, ((DefaultDownloadCompletedEventArgs<XDocument>)e).CreateNew(new PortfolioDownload().ConvertHtmlDoc(e.Response.Result), args.UserArgs));
        }
        private class EditHoldingsAyncArgs : DownloadEventArgs { public string Info = string.Empty; public EditHoldingsAyncArgs(object userArgs) : base(userArgs) { } }


        public void DownloadHoldingsAsync(string portfolioID, int viewIndex, object userArgs)
        {
            if (this.IsLoggedIn)
            {
                HoldingsDownload dl = new HoldingsDownload();
                dl.Settings.Account = this;
                dl.Settings.PortfolioID = portfolioID;
                dl.AsyncDownloadCompleted += this.HoldingsDownload_Completed;
                dl.DownloadAsync(userArgs);
            }
            else { throw new NotSupportedException("The user is not logged in."); }
        }
        private void HoldingsDownload_Completed(DownloadClient<HoldingsResult> sender, DownloadCompletedEventArgs<HoldingsResult> e)
        {
            sender.AsyncDownloadCompleted -= this.HoldingsDownload_Completed;
            if (this.AsyncHoldingsDownloadCompleted != null) this.AsyncHoldingsDownloadCompleted(this, e);
        }


        private List<KeyValuePair<string, string>> GetPortfolioDict(string name, CurrencyInfo currency, bool symbolSorting, bool symbolCollapsing, IEnumerable<YID> items, IEnumerable<YIndexID> indices, string pfID)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>(".yfiuseajax", "0"));
            lst.Add(new KeyValuePair<string, string>(".yfisrc", ""));
            lst.Add(new KeyValuePair<string, string>(".yfidone", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_id", pfID));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_name", name));
            lst.Add(new KeyValuePair<string, string>("quotes", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_symbols", ""));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_default_view", "v1"));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_view", "v1"));
            lst.Add(new KeyValuePair<string, string>("yfi_pf_currency", currency.ID));
            if (symbolCollapsing) lst.Add(new KeyValuePair<string, string>("yfi_pf_pref_lots", "collapse"));
            if (symbolSorting) lst.Add(new KeyValuePair<string, string>("yfi_pf_pref_sort", "symbol_ascending"));
            if (items != null)
            {
                string ids = string.Empty;
                foreach (YID id in items)
                {
                    ids += "+" + id.ID;
                }
                if (ids != string.Empty)
                {
                    lst.Add(new KeyValuePair<string, string>("yfi_pf_symbols", ids));
                }
            }
            if (indices != null)
            {
                foreach (YIndexID index in indices)
                {
                    lst.Add(new KeyValuePair<string, string>("yfi_pf_indices[]", index.ID.Replace("@", "")));
                }
            }
            lst.Add(new KeyValuePair<string, string>("yfi_pf_save", "Save"));
            return lst;
        }


        private class AddPfItemAsyncArgs : DownloadEventArgs
        {
            public string PortfolioID { get; set; }

            public AddPfItemAsyncArgs(object userArgs)
                : base(userArgs)
            {

            }
        }

    }







}
