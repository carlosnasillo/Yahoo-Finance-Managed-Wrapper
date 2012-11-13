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


namespace MaasOne.Geo.GeoPlanet
{
    public partial class PlacesDownload
    {


        public Base.Response<PlacesResult> Download(string keyword)
        {
            return this.Download(keyword, PlaceType.Undefined);
        }
        public Base.Response<PlacesResult> Download(string keyword, PlaceType type)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("places", this.KeywordQuery(keyword, type)));
        }
        public Base.Response<PlacesResult> Download(IEnumerable<long> woeids)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("places", this.WoeidQuery(woeids)));
        }
        public Base.Response<PlacesResult> DownloadParent(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/parent"));
        }
        public Base.Response<PlacesResult> DownloadChildren(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/children"));
        }
        public Base.Response<PlacesResult> DownloadChildren(long woeid, PlaceType type)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/children.type(" + Convert.ToInt32(type).ToString() + ")"));
        }
        public Base.Response<PlacesResult> DownloadChildren(long woeid, int degree)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/children.degree(" + degree.ToString() + ")"));
        }
        public Base.Response<PlacesResult> DownloadAncestors(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/ancestors"));
        }
        public Base.Response<PlacesResult> DownloadCommonAncestor(int first, IEnumerable<long> woeids)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", this.ComAncestorQuery(first, woeids)));
        }
        public Base.Response<PlacesResult> DownloadBelongtos(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/belongtos"));
        }
        public Base.Response<PlacesResult> DownloadBelongtos(long woeid, PlaceType type)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/belongtos.type(" + Convert.ToInt32(type).ToString() + ")"));
        }
        public Base.Response<PlacesResult> DownloadDescendants(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/descendants"));
        }
        public Base.Response<PlacesResult> DownloadDescendants(long woeid, PlaceType type)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/descendants.type(" + Convert.ToInt32(type).ToString() + ")"));
        }
        public Base.Response<PlacesResult> DownloadNeighbors(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/neighbors"));
        }
        public Base.Response<PlacesResult> DownloadNeighbors(long woeid, int degree)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/descendants.degree(" + degree.ToString() + ")"));
        }
        public Base.Response<PlacesResult> DownloadSiblings(long woeid)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("place/", woeid.ToString() + "/siblings"));
        }
        public Base.Response<PlacesResult> DownloadContinents()
        {
            return this.DownloadPlaces(this.Settings.NewQuery("continents", ""));
        }
        public Base.Response<PlacesResult> DownloadOceans()
        {
            return this.DownloadPlaces(this.Settings.NewQuery("oceans", ""));
        }
        public Base.Response<PlacesResult> DownloadSeas()
        {
            return this.DownloadPlaces(this.Settings.NewQuery("seas", ""));
        }
        public Base.Response<PlacesResult> DownloadCountries()
        {
            return this.DownloadPlaces(this.Settings.NewQuery("countries", ""));
        }
        public Base.Response<PlacesResult> DownloadStates(PlacesData country)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("states/", country.WOEID.ToString()));
        }
        public Base.Response<PlacesResult> DownloadCounty(PlacesData state)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("counties/", state.WOEID.ToString()));
        }
        public Base.Response<PlacesResult> DownloadDistricts(PlacesData county)
        {
            return this.DownloadPlaces(this.Settings.NewQuery("districts/", county.WOEID.ToString()));
        }        

        private Base.Response<PlacesResult> DownloadPlaces(PlacesDownloadSettings settings)
        {
            return (Base.Response<PlacesResult>)base.Download(settings);
        }

    }
}
