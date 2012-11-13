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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using MaasOne.Xml;


namespace MaasOne.Weather.YahooWeather
{


	public class WeatherFeed : RSS.Feed
	{

		public DegreesUnit TemperatureUnit { get; set; }
		public DistanceUnit DistanceUnit { get; set; }
		public PressureUnit PressureUnit { get; set; }
		public DistanceUnit SpeedUnit { get; set; }

		public WindInfo Wind { get; set; }
		public AtmosphereInfo Atmosphere { get; set; }
		public AstronomyInfo Astronomy { get; set; }
		public LocationInfo Location { get; set; }


		public WeatherFeed() : base()
		{
			this.Clear();
		}
		public void Clear()
		{
			base.Clear();
			this.SetUnitSystem(true);
			this.Wind = null;
			this.Atmosphere = null;
			this.Astronomy = null;
			this.Location = null;
		}
		public void SetUnitSystem(bool metric)
		{
			if (metric) {
				this.TemperatureUnit = DegreesUnit.Celsius;
				this.DistanceUnit = DistanceUnit.Kilometer;
				this.PressureUnit = PressureUnit.Milibars;
				this.SpeedUnit = DistanceUnit.Kilometer;
			} else {
				this.TemperatureUnit = DegreesUnit.Fahrenheit;
				this.DistanceUnit = DistanceUnit.Miles;
				this.PressureUnit = PressureUnit.PoundsPerSquareInch;
				this.SpeedUnit = DistanceUnit.Miles;
			}
		}
		public WeatherFeedItem ItemOf(int index)
		{
			return base.Items[index] as WeatherFeedItem;
		}




	}



	public class WeatherFeedItem : RSS.FeedItem
	{

		public ActualCondition ActualCondition { get; set; }
		public List<Forecast> ForecastConditions { get; set; }
		public Geo.Coordinates Coordinates { get; set; }

		public WeatherFeedItem()
		{
			this.ActualCondition = new ActualCondition();
			this.ForecastConditions = new List<Forecast>();
            this.Coordinates = new Geo.Coordinates();
		}

	}


}
