//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/4/30 10:40:29
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWeb.Model
{
	
	[Serializable()]
	public class BG_Caliber
	{
	
		private int caliberID; 
		private string caliberName = String.Empty;
		private int parentID;
        private int cbLever;

        public int CbLever
        {
            get { return cbLever; }
            set { cbLever = value; }
        }
		
		public BG_Caliber() { }
		
		
		public int CaliberID
		{
			get { return this.caliberID; }
			set { this.caliberID = value; }
		}
		
        
		
		
		
		public string CaliberName
		{
			get { return this.caliberName; }
			set { this.caliberName = value; }
		}		
		
		
		public int ParentID
		{
			get { return this.parentID; }
			set { this.parentID = value; }
		}		
		
	}
}
