//============================================================
// Producnt name:		Auto Generate
// Version: 			1.0
// Coded by:			Wu Di (wd_kk@qq.com)
// Auto generated at: 	2015/4/30 10:40:30
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BudgetWeb.Model;

namespace BudgetWeb.DAL
{
	public static partial class BG_CaliberService
	{
        public static BG_Caliber AddBG_Caliber(BG_Caliber bG_Caliber)
		{
            string sql =
                "INSERT BG_Caliber (CaliberName, ParentID,CbLever)" +
				"VALUES (@CaliberName, @ParentID)";
				
			sql += " ; SELECT @@IDENTITY";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@CaliberName", bG_Caliber.CaliberName),
					new SqlParameter("@ParentID", bG_Caliber.ParentID),
                    new SqlParameter("@CbLever", bG_Caliber.CbLever)
				};
			
                string IdStr = DBUnity.ExecuteScalar(CommandType.Text, sql, para);
                int newId = Convert.ToInt32(IdStr);
                return GetBG_CaliberByCaliberID(newId);

            }
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
                throw e;
            }
		}
		
        public static bool DeleteBG_Caliber(BG_Caliber bG_Caliber)
		{
			return DeleteBG_CaliberByCaliberID( bG_Caliber.CaliberID );
		}

        public static bool DeleteBG_CaliberByCaliberID(int caliberID)
		{
            string sql = "DELETE BG_Caliber WHERE CaliberID = @CaliberID";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@CaliberID", caliberID)
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
					


        public static bool ModifyBG_Caliber(BG_Caliber bG_Caliber)
        {
            string sql =
                "UPDATE BG_Caliber " +
                "SET " +
	                "CaliberName = @CaliberName, " +
                    "ParentID = @ParentID ," + "CbLever = @CbLever " +
                "WHERE CaliberID = @CaliberID";


            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@CaliberID", bG_Caliber.CaliberID),
					new SqlParameter("@CaliberName", bG_Caliber.CaliberName),
					new SqlParameter("@ParentID", bG_Caliber.ParentID),
                    new SqlParameter("@CbLever", bG_Caliber.CbLever)
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


        public static DataTable GetAllBG_Caliber()
        {
            string sqlAll = "SELECT * FROM BG_Caliber";
			return GetBG_CaliberBySql( sqlAll );
        }
		

        public static BG_Caliber GetBG_CaliberByCaliberID(int caliberID)
        {
            string sql = "SELECT * FROM BG_Caliber WHERE CaliberID = @CaliberID";

            try
            {
                SqlParameter para = new SqlParameter("@CaliberID", caliberID);
                DataTable dt = DBUnity.AdapterToTab(sql, para);
                
                if(dt.Rows.Count > 0)
                {
                    BG_Caliber bG_Caliber = new BG_Caliber();

                    bG_Caliber.CaliberID = dt.Rows[0]["CaliberID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["CaliberID"];
                    bG_Caliber.CaliberName = dt.Rows[0]["CaliberName"] == DBNull.Value ? "" : (string)dt.Rows[0]["CaliberName"];
                    bG_Caliber.ParentID = dt.Rows[0]["ParentID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["ParentID"];
                    bG_Caliber.CbLever = dt.Rows[0]["CbLever"] == DBNull.Value ? 0 : (int)dt.Rows[0]["CbLever"];
                    return bG_Caliber;
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
	

      

        private static DataTable GetBG_CaliberBySql(string safeSql)
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
		
        private static DataTable GetBG_CaliberBySql(string sql, params SqlParameter[] values)
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
