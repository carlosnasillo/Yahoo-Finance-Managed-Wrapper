// ******************************************************************************
// ** 
// **  MaasOne WebServices
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


namespace MaasOne.Geo
{



    public struct Coordinates
    {
        private double mLongitude;
        private double mLatitude;

        public double Longitude
        {
            get { return mLongitude; }
            set { mLongitude = value; }
        }
        public double Latitude
        {
            get { return mLatitude; }
            set { mLatitude = value; }
        }

        public Coordinates(double longitude, double latitude)
        {
            mLongitude = longitude;
            mLatitude = latitude;
        }

        public override string ToString()
        {
            return this.Latitude.ToString() + " " + this.Longitude;
        }

        public static bool operator ==(Coordinates obj1, Coordinates obj2)
        {
            return obj1.Longitude == obj2.Longitude & obj1.Latitude == obj2.Latitude;
        }
        public static bool operator !=(Coordinates obj1, Coordinates obj2)
        {
            return obj1.Longitude != obj2.Longitude | obj1.Latitude != obj2.Latitude;
        }

        public override bool Equals(object obj)
        {
            Coordinates obj2 = (Coordinates)obj;
            return this.Longitude == obj2.Longitude & this.Latitude == obj2.Latitude;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public struct CoordinatesOffSet
    {
        public double LongitudeOffSet { get; set; }
        public double Latitude { get; set; }
    }


    public struct CoordinatesRectangle
    {
        private Coordinates mSouthWest;
        private Coordinates mNorthEast;
        public Coordinates SouthWest
        {
            get { return mSouthWest; }
            set { mSouthWest = value; }
        }
        public Coordinates SouthEast
        {
            get
            {
                if ((mSouthWest.Longitude < mNorthEast.Longitude & mNorthEast.Latitude > mSouthWest.Latitude))
                {
                    return new Coordinates(mNorthEast.Longitude, mSouthWest.Latitude);
                }
                else
                {
                    return new Coordinates();
                }
            }
        }
        public Coordinates NorthEast
        {
            get { return mNorthEast; }
            set { mNorthEast = value; }
        }
        public Coordinates NorthWest
        {
            get
            {
                if ((mSouthWest.Longitude < mNorthEast.Longitude & mNorthEast.Latitude > mSouthWest.Latitude))
                {
                    return new Coordinates(mSouthWest.Longitude, mNorthEast.Latitude);
                }
                else
                {
                    return new Coordinates();
                }
            }
        }
        public CoordinatesRectangle(Coordinates sw, Coordinates ne)
        {
            mSouthWest = sw;
            mNorthEast = ne;
        }
        public static bool operator ==(CoordinatesRectangle obj1, CoordinatesRectangle obj2)
        {
            return obj1.NorthEast == obj2.NorthEast & obj1.SouthWest == obj2.SouthWest;
        }
        public static bool operator !=(CoordinatesRectangle obj1, CoordinatesRectangle obj2)
        {
            return obj1.NorthEast != obj2.NorthEast | obj1.SouthWest != obj2.SouthWest;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }



}
