//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/4/30 10:40:31
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BudgetWeb.DAL;
using BudgetWeb.Model;

namespace BudgetWeb.BLL
{

    public static partial class BG_CaliberManager
    {
        public static BG_Caliber AddBG_Caliber(BG_Caliber bG_Caliber)
        {
            return BG_CaliberService.AddBG_Caliber(bG_Caliber);
        }

        public static bool DeleteBG_Caliber(BG_Caliber bG_Caliber)
        {
            return BG_CaliberService.DeleteBG_Caliber(bG_Caliber);
        }

        public static bool DeleteBG_CaliberByID(int caliberID)
        {
            return BG_CaliberService.DeleteBG_CaliberByCaliberID(caliberID);
        }

		public static bool ModifyBG_Caliber(BG_Caliber bG_Caliber)
        {
            return BG_CaliberService.ModifyBG_Caliber(bG_Caliber);
        }
        
        public static DataTable GetAllBG_Caliber()
        {
            return BG_CaliberService.GetAllBG_Caliber();
        }

        public static BG_Caliber GetBG_CaliberByCaliberID(int caliberID)
        {
            return BG_CaliberService.GetBG_CaliberByCaliberID(caliberID);
        }

    }
}
