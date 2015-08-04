//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/6/25 9:54:08
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWeb.Model
{
	
	[Serializable()]
	public class BG_Unit_Dbbak
	{
	
		private int dbID; 
		private string dbName = String.Empty;
		private DateTime dbCreationTime;

		
		public BG_Unit_Dbbak() { }
		
		
		public int DbID
		{
			get { return this.dbID; }
			set { this.dbID = value; }
		}
		
        
		
		
		
		public string DbName
		{
			get { return this.dbName; }
			set { this.dbName = value; }
		}		
		
		
		public DateTime DbCreationTime
		{
			get { return this.dbCreationTime; }
			set { this.dbCreationTime = value; }
		}		
		
	}
}
