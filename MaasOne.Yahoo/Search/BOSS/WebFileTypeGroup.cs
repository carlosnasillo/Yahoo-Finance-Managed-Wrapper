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


namespace MaasOne.Search.BOSS
{


	public class FileTypeGroup
	{

		private List<WebFileType> mFileTypes = new List<WebFileType>();
		public List<WebFileType> FileTypes {
			get { return mFileTypes; }
		}


		#region "Shared"

		public static FileTypeGroup GetMsOffice()
		{
			FileTypeGroup grp = new FileTypeGroup();
			grp.FileTypes.Add(WebFileType.Xl);
			grp.FileTypes.Add(WebFileType.MsWord);
			grp.FileTypes.Add(WebFileType.Ppt);
			return grp;
		}
		public static FileTypeGroup GetNonHtml()
		{
			FileTypeGroup grp = new FileTypeGroup();
			grp.FileTypes.Add(WebFileType.Text);
			grp.FileTypes.Add(WebFileType.Pdf);
			grp.FileTypes.Add(WebFileType.Xl);
			grp.FileTypes.Add(WebFileType.MsWord);
			grp.FileTypes.Add(WebFileType.Ppt);
			return grp;
		}

		#endregion

	}


}
