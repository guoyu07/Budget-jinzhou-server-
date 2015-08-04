using BudgetWeb.DAL;
using BudgetWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common;

namespace BudgetWeb.BLL
{
    public class BG_SupplementaryLogic
    {
        public static DataTable GetBG_SupplementaryByyear(int year)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlStr = "select * from dbo.BG_Supplementary where Year={0}";
                sqlStr = string.Format(sqlStr, year);
                dt = DBUnity.AdapterToTab(sqlStr);
            }
            catch
            {
                dt=null;
            }
           
            return dt;
        }

     

        public static bool IsSuppByYear(int year)
        {
            bool flag = false;
            string sqlStr = "select count(*) from dbo.BG_Supplementary where Year={0}";
            sqlStr = string.Format(sqlStr, year);
            try
            {
                int t = common.IntSafeConvert(DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null));
                if (    t>0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (System.Exception ex)
            {
                flag = false;
            }
            
            return flag;
        }
    }

}
