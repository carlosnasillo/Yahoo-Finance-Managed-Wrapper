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


namespace MaasOne.Geo.PlaceFinder
{


	public enum PlaceFinderError
	{
		NoError = 0,
		FeatureNotSupported = 1,
		InternalProblemDetected = 10,
		NoInputParameters = 100,
		AddressDataNotRecognizedAsValidUTF8 = 102,
		InsufficientAddressData = 103,
		UnknownLanguage = 104,
		NoCountryDetected = 105,
		CountryNotSupported = 106
	}


	public enum AddressQualitiy
	{
		Coordinate = 99,
		POI = 90,
		AddressMatchWithStreetMatch = 87,
		AddressMismatchWithStreetMatch = 86,
		AddressMatchWithStreetMismatch = 85,
		AddressMismatchWithStreetMismatch = 84,
		IntersectionWithStreetMatch = 82,
		IntersectionWithStreetMismatch = 80,
		/// <summary>
		/// Zip+4 in US
		/// </summary>
		/// <remarks></remarks>
		PostalUnit_Segment_Zip4_in_US = 75,
		/// <summary>
		/// Zip+4 in US
		/// </summary>
		/// <remarks></remarks>
		PostalUnit_Segment_StreetIgnored = 74,
		StreetMatch = 72,
		StreetMatch_AddressIgnored = 71,
		StreetMismatch = 70,
		/// <summary>
		/// Zip+2 in US
		/// </summary>
		/// <remarks></remarks>
		PostalZone_Sector_StreetIgnored = 64,
		AOI = 63,
		Airport = 62,
		/// <summary>
		/// Zip Code in US
		/// </summary>
		/// <remarks></remarks>
		PostalDistrict = 60,
		/// <summary>
		/// Zip Code in US
		/// </summary>
		/// <remarks></remarks>
		PostalDistrict_StreetIgnored = 59,
		Neighborhood = 50,
		Neighborhood_StreetIgnored = 49,
		CityTownLocality = 40,
		CityTownLocality_NeighborhoodIgnored = 39,
		County = 30,
		County_CityTownLocalityIgnored = 29,
		StateProvince = 20,
		StateProvince_CountyIgnored = 19,
		Country = 10,
		Country_StateProvinceIgnored = 9,
		NotAnAddress = 0
	}

}
