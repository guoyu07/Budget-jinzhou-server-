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
using System.Data.SqlClient;
using BudgetWeb.Model;

namespace BudgetWeb.DAL
{
	public static partial class BG_Unit_DbbakService
	{
        public static BG_Unit_Dbbak AddBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
		{
            string sql =
				"INSERT BG_Unit_Dbbak (DbName, DbCreationTime)" +
				"VALUES (@DbName, @DbCreationTime)";
				
			sql += " ; SELECT @@IDENTITY";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@DbName", bG_Unit_Dbbak.DbName),
					new SqlParameter("@DbCreationTime", bG_Unit_Dbbak.DbCreationTime)
				};
			
                string IdStr = DBUnity.ExecuteScalar(CommandType.Text, sql, para);
                int newId = Convert.ToInt32(IdStr);
                return GetBG_Unit_DbbakByDbID(newId);

            }
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
                throw e;
            }
		}
		
        public static bool DeleteBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
		{
			return DeleteBG_Unit_DbbakByDbID( bG_Unit_Dbbak.DbID );
		}

        public static bool DeleteBG_Unit_DbbakByDbID(int dbID)
		{
            string sql = "DELETE BG_Unit_Dbbak WHERE DbID = @DbID";

            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@DbID", dbID)
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
					


        public static bool ModifyBG_Unit_Dbbak(BG_Unit_Dbbak bG_Unit_Dbbak)
        {
            string sql =
                "UPDATE BG_Unit_Dbbak " +
                "SET " +
	                "DbName = @DbName, " +
	                "DbCreationTime = @DbCreationTime " +
                "WHERE DbID = @DbID";


            try
            {
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@DbID", bG_Unit_Dbbak.DbID),
					new SqlParameter("@DbName", bG_Unit_Dbbak.DbName),
					new SqlParameter("@DbCreationTime", bG_Unit_Dbbak.DbCreationTime)
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


        public static DataTable GetAllBG_Unit_Dbbak()
        {
            string sqlAll = "SELECT * FROM BG_Unit_Dbbak";
			return GetBG_Unit_DbbakBySql( sqlAll );
        }
		

        public static BG_Unit_Dbbak GetBG_Unit_DbbakByDbID(int dbID)
        {
            string sql = "SELECT * FROM BG_Unit_Dbbak WHERE DbID = @DbID";

            try
            {
                SqlParameter para = new SqlParameter("@DbID", dbID);
                DataTable dt = DBUnity.AdapterToTab(sql, para);
                
                if(dt.Rows.Count > 0)
                {
                    BG_Unit_Dbbak bG_Unit_Dbbak = new BG_Unit_Dbbak();

                    bG_Unit_Dbbak.DbID = dt.Rows[0]["DbID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["DbID"];
                    bG_Unit_Dbbak.DbName = dt.Rows[0]["DbName"] == DBNull.Value ? "" : (string)dt.Rows[0]["DbName"];
                    bG_Unit_Dbbak.DbCreationTime = dt.Rows[0]["DbCreationTime"] == DBNull.Value ? DateTime.MinValue : (DateTime)dt.Rows[0]["DbCreationTime"];
                    
                    return bG_Unit_Dbbak;
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
	

      

        private static DataTable GetBG_Unit_DbbakBySql(string safeSql)
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
		
        private static DataTable GetBG_Unit_DbbakBySql(string sql, params SqlParameter[] values)
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
