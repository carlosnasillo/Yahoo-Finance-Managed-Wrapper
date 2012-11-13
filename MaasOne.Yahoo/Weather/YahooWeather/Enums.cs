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


	public enum DegreesUnit
	{
		Celsius = 0,
		Fahrenheit = 1
	}


	public enum DistanceUnit
	{
		Kilometer = 0,
		Miles = 1
	}

	public enum PressureUnit
	{
		Milibars = 0,
		PoundsPerSquareInch = 1
	}

	public enum PressureState
	{
		Steady = 0,
		Rising = 1,
		Falling = 2
	}


	public enum ConditionType
	{
		Tornado = 0,
		Tropical_Storm = 1,
		Hurricane = 2,
		Severe_Thunderstorms = 3,
		Thunderstorms = 4,
		Mixed_Rain_And_Snow = 5,
		Mixed_Rain_And_Sleet = 6,
		Mixed_Snow_And_Sleet = 7,
		Freezing_Drizzle = 8,
		Drizzle = 9,
		Freezing_Rain = 10,
		Showers = 11,
		Heavy_Showers = 12,
		Snow_Flurries = 13,
		Light_Snow_Showers = 14,
		Blowing_Snow = 15,
		Snow = 16,
		Hail = 17,
		Sleet = 18,
		Dust = 19,
		Foggy = 20,
		Haze = 21,
		Smoky = 22,
		Blustery = 23,
		Windy = 24,
		Cold = 25,
		Cloudy = 26,
		Mostly_Cloudy_Night = 27,
		Mostly_Cloudy_Day = 28,
		Partly_Cloudy_Night = 29,
		Partly_Cloudy_Day = 30,
		Clear_Night = 31,
		Sunny = 32,
		Fair_Night = 33,
		Fair_Day = 34,
		Mixed_Rain_And_Hail = 35,
		Hot = 36,
		Isolated_Thunderstorms = 37,
		Scattered_Thunderstorms = 38,
		Isolated_Showers = 39,
		Scattered_Showers = 40,
		Heavy_Snow = 41,
		Scattered_Snow_Showers = 42,
		Scattered_Snow = 43,
		Partly_Cloudy = 44,
		Thundershowers = 45,
		Snow_Showers = 46,
		Isolated_Thundershowers = 47,
		Not_Available = 3200
	}



}
