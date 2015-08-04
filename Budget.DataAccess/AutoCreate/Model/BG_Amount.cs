//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/5/8 9:59:38
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWeb.Model
{
	
	[Serializable()]
	public class BG_Amount
	{
	
		private int bGAMID; 
		private decimal bGAMMon;
		private decimal bGAMIncome;
		private int bGAMYear;
		private int depID;
		private int cBID;

		
		public BG_Amount() { }
		
		
		public int BGAMID
		{
			get { return this.bGAMID; }
			set { this.bGAMID = value; }
		}
		
        
		
		
		
		public decimal BGAMMon
		{
			get { return this.bGAMMon; }
			set { this.bGAMMon = value; }
		}		
		
		
		public decimal BGAMIncome
		{
			get { return this.bGAMIncome; }
			set { this.bGAMIncome = value; }
		}		
		
		
		public int BGAMYear
		{
			get { return this.bGAMYear; }
			set { this.bGAMYear = value; }
		}		
		
		
		public int DepID
		{
			get { return this.depID; }
			set { this.depID = value; }
		}		
		
		
		public int CBID
		{
			get { return this.cBID; }
			set { this.cBID = value; }
		}		
		
	}
}
