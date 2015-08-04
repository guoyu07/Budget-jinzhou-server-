using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetWeb.Model;
using BudgetWeb.DAL;
using System.Data;
using System.Data.SqlClient;
using Common;

namespace BudgetWeb.BLL
{
    public class BG_ApplyReimburLogic
    {
        //public static DataTable GetARMon(int ppid, int DepID, string ARTime)
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        string str = string.Format("select sum(ARMon) as ARMon from BG_ApplyReimbur where ppid={0} and DepId={1} and  convert(varchar(7),ARTime,120)='{2}' and ARListSta='审核通过'", ppid, DepID, ARTime);
        //        dt = DBUnity.AdapterToTab(str);
        //    }
        //    catch
        //    {
        //        dt = null;
        //    }
        //    return dt;
        //} 
       
        public static decimal GetARMon(int ppid, int DepID, string YearMonth)
        {
            decimal tt = 0;
            try
            {
                string str = string.Format("select BQMon from V_Cashier_PayIncome where piid={0} and Depid={1} and Convert(varchar(7),Ctime,120)<'{2}'", ppid, DepID, YearMonth);
                tt = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, str, null));
                 
            }
            catch
            {
                tt = 0;
            }
            return tt;
        }

        public static DataTable GetARMonByARTime(string ARTime, int depid)
        {
            DataTable dt = null;
            try
            {
                string str = string.Format("select sum(ARMon) as ARMon from  dbo.BG_ApplyReimbur  where ARListSta='审核通过' and  Convert(varchar(7),ARTime,120)='{0}' and DepId={1}  ", ARTime, depid);
                dt = DBUnity.AdapterToTab(str);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static decimal  GetARUseMon(int ppid, int DepID, int Year)
        { 
            string str = string.Format("select sum(ARMon) as ARMon from BG_ApplyReimbur where ppid={0} and DepId={1} and  Year(ARTime)={2} and ARListSta='审核通过'", ppid, DepID, Year);
             decimal t = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, str, null));
            return t;
        }

        public static bool ISApplyBackMon(string ARTime, int DepID)
        {
            bool flag = false;
            try
            {
                string str = string.Format("select sum(ARMon) as ARMon from BG_ApplyReimbur where ppid={0} and DepId={1} and  convert(varchar(7),ARTime,120)='{2}'  and ARListSta='退回'", DepID, ARTime);
                int t = common.IntSafeConvert(DBUnity.ExecuteScalar(CommandType.Text,str,null));
                if (t>0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static decimal ApplyMon(string ARTime, int DepID)
        {
            decimal t = 0;
            try
            {
                string str = string.Format("select sum(ARMon) as ARMon from BG_ApplyReimbur where DepId={0} and  convert(varchar(7),ARTime,120)<='{1}'", DepID, ARTime);
                t =ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, str, null).ToString());
            }
            catch
            {
                t = 0;
            }
            return t;
        }

        public static decimal ApplyBackMon(string ARTime, int DepID)
        {
            decimal t = 0;
            try
            {
                string str = string.Format("select sum(ARMon) as ARMon from BG_ApplyReimbur where DepId={0} and  convert(varchar(7),ARTime,120)<='{1}' and ARListSta='退回'", DepID, ARTime);
                t = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, str, null).ToString());
            }
            catch
            {
                t = 0;
            }
            return t;
        }
         



        public static DataTable GetAllDT(string yearMonth,int DepID)
        {
            string StartYearMonth = yearMonth.Split('-')[0] + "-00";
            DataTable dt = null;
            try
            {
                string sql = string.Format("select PIEcoSubName,BG_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney  from [dbo].[BG_MonPayPlan] left join    [dbo].[BG_BudgetAllocation] on [BG_MonPayPlan].PIID=[BG_BudgetAllocation].PIID and [BG_BudgetAllocation].DepID=[BG_MonPayPlan].[DeptID] left join [dbo].[BG_MonPayPlanRemark] on [BG_MonPayPlanRemark].[MATime]=[BG_MonPayPlan].MPTime and [BG_MonPayPlanRemark].[DeptID]=[dbo].[BG_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_MonPayPlan].PIID left join [dbo].[RM_Receipts] on [RM_Receipts].[rpdep] in  (select DepName from [dbo].[BG_Department] where  DepID= [BG_MonPayPlan].[DeptID] and RPRemark3=[BG_PayIncome].PIEcoSubName) and convert(varchar(7),[RM_Receipts].[rptime],120)= convert(varchar(7),[BG_MonPayPlan].[MPTime],120) where [BG_MonPayPlan].[DeptID]={1} and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过' Group by PIEcoSubName,BG_MonPayPlan.PIID", yearMonth, DepID, StartYearMonth);
                dt = DBUnity.AdapterToTab(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
    }
}
