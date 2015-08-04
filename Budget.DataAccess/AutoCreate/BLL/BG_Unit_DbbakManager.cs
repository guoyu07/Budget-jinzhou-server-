//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/6/25 9:54:08
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BudgetWeb.DAL;
using BudgetWeb.Model;

namespace BudgetWeb.BLL
{

    public static partial class BG_Unit_DbbakManager
    {
        public static BG_Unit_Dbbak AddBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
        {
            return BG_Unit_DbbakService.AddBG_Unit_Dbbak(bG_Unit_Dbbak);
        }

        public static bool DeleteBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
        {
            return BG_Unit_DbbakService.DeleteBG_Unit_Dbbak(bG_Unit_Dbbak);
        }

        public static bool DeleteBG_Unit_DbbakByID(int dbID)
        {
            return BG_Unit_DbbakService.DeleteBG_Unit_DbbakByDbID(dbID);
        }

		public static bool ModifyBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
        {
            return BG_Unit_DbbakService.ModifyBG_Unit_Dbbak(bG_Unit_Dbbak);
        }
        
        public static DataTable GetAllBG_Unit_Dbbak()
        {
            return BG_Unit_DbbakService.GetAllBG_Unit_Dbbak();
        }

        public static BG_Unit_Dbbak GetBG_Unit_DbbakByDbID(int dbID)
        {
            return BG_Unit_DbbakService.GetBG_Unit_DbbakByDbID(dbID);
        }

    }
}
