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


namespace MaasOne.Finance.YahooPortfolio
{


    public enum PortfolioColumnType
    {
        symbol,
        name,
        currency,
        exchange,
        price,
        time,
        change,
        percent_change,
        volume,
        avg_daily_volume,
        more_info,
        sparkline,
        bid_price,
        bid_size,
        ask_price,
        ask_size,
        last_trade_size,
        float_shares,
        prev_close,
        open,
        day_high,
        day_low,
        fiftytwo_week_high,
        fiftytwo_week_low,
        change_fiftytwo_week_high,
        change_fiftytwo_week_low,
        percent_change_fiftytwo_week_high,
        percent_change_fiftytwo_week_low,
        short_ratio,
        shares_out,
        pre_mkt_price,
        pre_mkt_time,
        after_mkt_price,
        after_mkt_time,
        pre_mkt_change,
        pre_mkt_percent_change,
        after_mkt_change,
        after_mkt_percent_change,
        eps,
        pe_ratio,
        pe_ratio_next_year,
        dividend_pay_date,
        ex_dividend_date,
        dividend_per_share,
        dividend_yield,
        market_cap,
        book_value,
        price_per_book,
        price_per_sales,
        ebitda,
        fifty_day_ma,
        fifty_day_ma_change,
        fifty_day_ma_percent_change,
        twohundred_day_ma,
        twohundred_day_ma_change,
        twohundred_day_ma_percent_change,
        one_year_target,
        eps_est_current_year,
        eps_est_next_year,
        eps_est_next_quarter,
        price_per_eps_est_current_year,
        price_per_eps_est_next_year,
        peg_ratio,
        shares_owned,
        price_paid,
        commission,
        holdings_value,
        day_value_change,
        day_value_percent_change,
        holdings_gain,
        holdings_percent_gain,
        trade_date,
        annualized_gain,
        high_limit,
        low_limit,
        price_and_time,
        change_and_percent,
        day_range,
        fiftytwo_week_range,
        pre_after_mkt_price_and_time,
        pre_mkt_price_and_time,
        pre_mkt_change_and_percent,
        after_mkt_price_and_time,
        after_mkt_change_and_percent,      
        day_value_change_and_percent,
        holdings_gain_and_percent,
        comment
    }


}
