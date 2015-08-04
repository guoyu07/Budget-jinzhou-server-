using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BudgetWeb.DAL;
using Common;

namespace BudgetWeb.BLL
{
    public class BG_CaliberLogic
    {
        public static DataTable GetAllBG_CaliberMon(int DepID, int year)
        {
            DataTable dt = null;
            try
            {
                string sql = string.Format("select * from  BG_Caliber  left join   (select *  from   BG_Amount where DepID={0} and BGAMYear={1}) as  a On CBID =CaliberID left join (select[BGAMMon] as [BGAMLastMon],CBID from [dbo].[BG_Amount] where BGAMYear={1}-1 and  DepID={0})  as b on   b.CBID =CaliberID", DepID, year);
                dt = DBUnity.AdapterToTab(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }

        public static bool IsLeafEnd(int caliberID)
        {
            bool flag = false;
            int t = 1;
            try
            {
                string sql = string.Format(" select count(*) from BG_Caliber where ParentID={0}", caliberID);
                try
                {
                    t = common.IntSafeConvert(DBUnity.ExecuteScalar(CommandType.Text, sql, null));
                }
                catch
                {

                    t = 1;
                }
                if (t == 0)
                {
                    flag = true;
                }

            }
            catch
            {
                flag = false;
            }
            return flag;
        }


        public static DataTable GetAllBG_CaliberMon(List<int> listt, int year)
        {
            DataTable dt = null;
            try
            {
                string sql = "";
                if (listt.Count > 0)
                {
                    if (listt.Count == 1)
                    {
                        sql = string.Format("select * from  BG_Caliber  left join   (select *  from   BG_Amount where DepID={0} and BGAMYear={1}) as  a On CBID =CaliberID left join (select[BGAMMon] as [BGAMLastMon],CBID from [dbo].[BG_Amount] where BGAMYear={1}-1 and  DepID={0})  as b on   b.CBID =CaliberID", listt[0], year);
                    }
                    else
                    {
                        string depidselect = "";
                        for (int i = 0; i < listt.Count; i++)
                        {
                            depidselect += listt[i] + ",";
                        }
                        depidselect=depidselect.TrimEnd(',');
                        sql = string.Format("select * from (select * from  BG_Caliber left join (select CBID as CBIDa ,sum(BGAMMon) as BGAMMon ,sum(BGAMIncome) as BGAMIncome    from   BG_Amount where BGAMYear={0} and DepID in ({2})  group by CBID) as  a{1} On a{1}.CBIDa =CaliberID left join (select CBID as CBIDb,sum([BGAMMon]) as [BGAMLastMon],CBID from [dbo].[BG_Amount] where BGAMYear={0}-1 and DepID in ({2}) group by CBID)  as b{1} on   b{1}.CBIDb =CaliberID ) as c{1} ", year, "i",depidselect);
                        for (int i = 0; i < listt.Count; i++)
                        {
                            sql += " left join  ";
                            sql += string.Format("   (select *  from   BG_Amount where DepID={0} and BGAMYear={1}) as  a{2} On a{2}.CBID =CaliberID left join (select[BGAMMon] as [BGAMLastMon],CBID from [dbo].[BG_Amount] where BGAMYear={1}-1 and  DepID={0})  as b{2} on   b{2}.CBID =CaliberID", listt[i], year, i);
                        }
                    }
                }
                sql += " order by CaliberID ";
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
