//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/8/6 15:34:17
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWeb.Model
{
	
	[Serializable()]
	public class BG_Cashier
	{
	
		private int cashierid; 
		private int piid;
		private decimal bgMon;
		private decimal cZMon;
		private decimal qTMon;
		private decimal bQMon;
		private int depID;
		private DateTime cTime;
		private decimal cashierBalance;

		
		public BG_Cashier() { }
		
		
		public int Cashierid
		{
			get { return this.cashierid; }
			set { this.cashierid = value; }
		}
		
        
		
		
		
		public int Piid
		{
			get { return this.piid; }
			set { this.piid = value; }
		}		
		
		
		public decimal BgMon
		{
			get { return this.bgMon; }
			set { this.bgMon = value; }
		}		
		
		
		public decimal CZMon
		{
			get { return this.cZMon; }
			set { this.cZMon = value; }
		}		
		
		
		public decimal QTMon
		{
			get { return this.qTMon; }
			set { this.qTMon = value; }
		}		
		
		
		public decimal BQMon
		{
			get { return this.bQMon; }
			set { this.bQMon = value; }
		}		
		
		
		public int DepID
		{
			get { return this.depID; }
			set { this.depID = value; }
		}		
		
		
		public DateTime CTime
		{
			get { return this.cTime; }
			set { this.cTime = value; }
		}		
		
		
		public decimal CashierBalance
		{
			get { return this.cashierBalance; }
			set { this.cashierBalance = value; }
		}		
		
	}
}
