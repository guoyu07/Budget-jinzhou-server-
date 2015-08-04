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
using System.Data.SqlClient;
using BudgetWeb.Model;

namespace BudgetWeb.DAL
{
	public static partial class BG_AmountService
	{
        public static BG_Amount AddBG_Amount(BG_Amount bG_Amount)
		{
            string sql =
				"INSERT BG_Amount (BGAMMon, BGAMIncome, BGAMYear, DepID, CBID)" +
				"VALUES (@BGAMMon, @BGAMIncome, @BGAMYear, @DepID, @CBID)";
				
			sql += " ; SELECT @@IDENTITY";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@BGAMMon", bG_Amount.BGAMMon),
					new SqlParameter("@BGAMIncome", bG_Amount.BGAMIncome),
					new SqlParameter("@BGAMYear", bG_Amount.BGAMYear),
					new SqlParameter("@DepID", bG_Amount.DepID),
					new SqlParameter("@CBID", bG_Amount.CBID)
				};
			
                string IdStr = DBUnity.ExecuteScalar(CommandType.Text, sql, para);
                int newId = Convert.ToInt32(IdStr);
                return GetBG_AmountByBGAMID(newId);

            }
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
                throw e;
            }
		}
		
        public static bool DeleteBG_Amount(BG_Amount bG_Amount)
		{
			return DeleteBG_AmountByBGAMID( bG_Amount.BGAMID );
		}

        public static bool DeleteBG_AmountByBGAMID(int bGAMID)
		{
            string sql = "DELETE BG_Amount WHERE BGAMID = @BGAMID";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@BGAMID", bGAMID)
				};
			
                int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, para);
                
                if(t>0)
                {
                    return true;
                }
                else
                {
                    return false;    
                }
            }
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
				throw e;
            }
		}
					


        public static bool ModifyBG_Amount(BG_Amount bG_Amount)
        {
            string sql =
                "UPDATE BG_Amount " +
                "SET " +
	                "BGAMMon = @BGAMMon, " +
	                "BGAMIncome = @BGAMIncome, " +
	                "BGAMYear = @BGAMYear, " +
	                "DepID = @DepID, " +
	                "CBID = @CBID " +
                "WHERE BGAMID = @BGAMID";


            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@BGAMID", bG_Amount.BGAMID),
					new SqlParameter("@BGAMMon", bG_Amount.BGAMMon),
					new SqlParameter("@BGAMIncome", bG_Amount.BGAMIncome),
					new SqlParameter("@BGAMYear", bG_Amount.BGAMYear),
					new SqlParameter("@DepID", bG_Amount.DepID),
					new SqlParameter("@CBID", bG_Amount.CBID)
				};

                int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, para);
                if(t>0)
                {
                    return true;
                }
                else
                {
                    return false;    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
				throw e;
            }

        }		


        public static DataTable GetAllBG_Amount()
        {
            string sqlAll = "SELECT * FROM BG_Amount";
			return GetBG_AmountBySql( sqlAll );
        }
		

        public static BG_Amount GetBG_AmountByBGAMID(int bGAMID)
        {
            string sql = "SELECT * FROM BG_Amount WHERE BGAMID = @BGAMID";

            try
            {
                SqlParameter para = new SqlParameter("@BGAMID", bGAMID);
                DataTable dt = DBUnity.AdapterToTab(sql, para);
                
                if(dt.Rows.Count > 0)
                {
                    BG_Amount bG_Amount = new BG_Amount();

                    bG_Amount.BGAMID = dt.Rows[0]["BGAMID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["BGAMID"];
                    bG_Amount.BGAMMon = dt.Rows[0]["BGAMMon"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["BGAMMon"];
                    bG_Amount.BGAMIncome = dt.Rows[0]["BGAMIncome"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["BGAMIncome"];
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
	

      

        private static DataTable GetBG_AmountBySql(string safeSql)
        {

			try
			{
				DataTable dt = DBUnity.AdapterToTab(safeSql);
                return dt;
			}
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

        }
		
        private static DataTable GetBG_AmountBySql(string sql, params SqlParameter[] values)
        {

			try
			{
				DataTable dt = DBUnity.AdapterToTab(sql, values);
                return dt;
			}
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
			
        }
		
	}
}
