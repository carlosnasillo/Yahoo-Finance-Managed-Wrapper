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


namespace MaasOne.Geo
{


	public enum PlaceType
	{
		[Description("An undefined place")]
		Undefined = 0,
		[Description("A building that matches the house number from an address")]
		GeocodedBuilding = 1,
		[Description("A street segment that matches the street from an address")]
		GeocodedStreet_Segment = 2,
		[Description("A building that has a house number near to the house number from an address")]
		Nearby_Building = 3,
		[Description("A street that matches the street from an address")]
		GeocodedStreet = 4,
		[Description("An intersection of streets that matches a query string")]
		GeocodedIntersection = 5,
		[Description("A street")]
		Street = 6,
		[Description("A populated settlement such as a city, town, village")]
		Town = 7,
		[Description("One of the primary administrative areas within a country")]
		State = 8,
		[Description("One of the secondary administrative areas within a country")]
		County = 9,
		[Description("One of the tertiary administrative areas within a country")]
		Local_Administrative_Area = 10,
		[Description("A partial or full postal code")]
		Postal_Code = 11,
		[Description("One of the countries or dependent territories defined by the ISO 3166-1 standard")]
		Country = 12,
		[Description("An island")]
		Island = 13,
		[Description("An airport")]
		Airport = 14,
		[Description("A water feature such as a river, canal, lake, ey, ocean")]
		Drainage = 15,
		[Description("A land feature such as a park, mountain, beach")]
		Land_Feature = 16,
		[Description("A uncategorized place")]
		Miscellaneous = 17,
		[Description("An area affiliated with a nationality")]
		Nationality = 18,
		[Description("An area covering multiple countries")]
		Supername = 19,
		[Description("A point of interest such as a school, hospital, tourist attraction")]
		Point_of_Interest = 20,
		[Description("An area covering portions of several countries")]
		Region = 21,
		[Description("A subdivision of a town such as a suburb or neighborhood")]
		Suburb = 22,
		[Description("A sports team")]
		Sports_Team = 23,
		[Description("A place known by a colloquial name")]
		Colloquial = 24,
		[Description("An area known within a specific context such as MSA or area code")]
		Zone = 25,
		[Description("A historical primary administrative area within a country")]
		Historical_State = 26,
		[Description("A historical secondary administrative area within a country")]
		Historical_County = 27,
		[Description("One of the major land masses on the Earth")]
		Continent = 29,
		[Description("An area defined by the Olson standard (tz database)")]
		Time_Zone = 31,
		[Description("An intersection of streets that is nearby to the streets in a query string")]
		Nearby_Intersection = 32,
		[Description("A housing development or subdivision known by name")]
		Estate = 33,
		[Description("A historical populated settlement that is no longer known by its original name")]
		Historical_Town = 35,
		[Description("An aggregate place")]
		Aggregate = 36,
		[Description("One of the five major bodies of water on the Earth")]
		Ocean = 37,
		[Description("An area of open water smaller than an ocean")]
		Sea = 38
	}

}
