using BudgetWeb.DAL;
using BudgetWeb.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BudgetWeb.BLL
{
    public class BG_AmountLogic
    {

        public static BG_Amount GetBG_AmountByYear(int CBID,int Year,int depid)
        {
            string sql = string.Format("select * from  BG_Amount where CBID=@CBID and BGAMYear=@BGAMYear and DepID=@DepID", CBID, Year, depid);
            try
            {
                SqlParameter[] para = new SqlParameter[]
				{ 
					new SqlParameter("@BGAMYear", Year), 
					new SqlParameter("@CBID", CBID),
                    new SqlParameter("@DepID", depid)
				}; 
                DataTable dt = DBUnity.AdapterToTab(sql, para);

                if (dt.Rows.Count > 0)
                {
                    BG_Amount bG_Amount = new BG_Amount();

                    bG_Amount.BGAMID = dt.Rows[0]["BGAMID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["BGAMID"];
                    bG_Amount.BGAMMon = dt.Rows[0]["BGAMMon"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["BGAMMon"];
                    bG_Amount.BGAMYear = dt.Rows[0]["BGAMYear"] == DBNull.Value ? 0 : (int)dt.Rows[0]["BGAMYear"];
                    bG_Amount.DepID = dt.Rows[0]["DepID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["DepID"];
                    bG_Amount.CBID = dt.Rows[0]["CBID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["CBID"];

                    return bG_Amount;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
