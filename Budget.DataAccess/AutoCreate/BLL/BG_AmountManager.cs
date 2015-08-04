//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/5/8 9:59:38
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BudgetWeb.DAL;
using BudgetWeb.Model;

namespace BudgetWeb.BLL
{

    public static partial class BG_AmountManager
    {
        public static BG_Amount AddBG_Amount(BG_Amount bG_Amount)
        {
            return BG_AmountService.AddBG_Amount(bG_Amount);
        }

        public static bool DeleteBG_Amount(BG_Amount bG_Amount)
        {
            return BG_AmountService.DeleteBG_Amount(bG_Amount);
        }

        public static bool DeleteBG_AmountByID(int bGAMID)
        {
            return BG_AmountService.DeleteBG_AmountByBGAMID(bGAMID);
        }

		public static bool ModifyBG_Amount(BG_Amount bG_Amount)
        {
            return BG_AmountService.ModifyBG_Amount(bG_Amount);
        }
        
        public static DataTable GetAllBG_Amount()
        {
            return BG_AmountService.GetAllBG_Amount();
        }

        public static BG_Amount GetBG_AmountByBGAMID(int bGAMID)
        {
            return BG_AmountService.GetBG_AmountByBGAMID(bGAMID);
        }

    }
}
