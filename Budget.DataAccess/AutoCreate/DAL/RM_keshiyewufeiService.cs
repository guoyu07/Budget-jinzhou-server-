using Budget.DataAccess.AutoCreate.Model;
using BudgetWeb.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Budget.DataAccess.AutoCreate.DAL
{
    public static partial class RM_keshiyewufeiService
    {
        public static RM_keshiyewufei AddRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            string sql =
                "INSERT RM_keshiyewufei (Depname, BQJE, SQJE, ZJJE, KYJE, Remark, Remark1,LSJL)" +
                "VALUES (@Depname, @BQJE, @SQJE, @ZJJE, @KYJE, @Remark, @Remark1,@LSJL)";

            sql += " ; SELECT  SCOPE_IDENTITY()";

            try
            {
                SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@Depname", rM_keshiyewufei.Depname),
					new SqlParameter("@BQJE", rM_keshiyewufei.BQJE),
					new SqlParameter("@SQJE", rM_keshiyewufei.SQJE),
					new SqlParameter("@ZJJE", rM_keshiyewufei.ZJJE),
					new SqlParameter("@KYJE", rM_keshiyewufei.KYJE),
					new SqlParameter("@Remark", rM_keshiyewufei.Remark),
					new SqlParameter("@Remark1", rM_keshiyewufei.Remark1),
                    new SqlParameter("@LSJL", rM_keshiyewufei.LSJL)
				};

                string IdStr = DBUnity.ExecuteScalar(CommandType.Text, sql, para);
                int newId = Convert.ToInt32(IdStr);
                return GetRM_keshiyewufeiByKSID(newId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public static bool DeleteRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            return DeleteRM_keshiyewufeiByKSID(rM_keshiyewufei.KSID);
        }

        public static bool DeleteRM_keshiyewufeiByKSID(int kSID)
        {
            string sql = "DELETE RM_keshiyewufei WHERE KSID = @KSID";

            try
            {
                SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@KSID", kSID)
				};

                int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, para);

                if (t > 0)
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



        public static bool ModifyRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            string sql =
                "UPDATE RM_keshiyewufei " +
                "SET " +
                    "Depname = @Depname, " +
                    "BQJE = @BQJE, " +
                    "SQJE = @SQJE, " +
                    "ZJJE = @ZJJE, " +
                    "KYJE = @KYJE, " +
                    "Remark = @Remark, " +
                    "Remark1 = @Remark1, " +
                     "LSJL = @LSJL " +
                "WHERE KSID = @KSID";


            try
            {
                SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@KSID", rM_keshiyewufei.KSID),
					new SqlParameter("@Depname", rM_keshiyewufei.Depname),
					new SqlParameter("@BQJE", rM_keshiyewufei.BQJE),
					new SqlParameter("@SQJE", rM_keshiyewufei.SQJE),
					new SqlParameter("@ZJJE", rM_keshiyewufei.ZJJE),
					new SqlParameter("@KYJE", rM_keshiyewufei.KYJE),
					new SqlParameter("@Remark", rM_keshiyewufei.Remark),
					new SqlParameter("@Remark1", rM_keshiyewufei.Remark1),
                    new SqlParameter("@LSJL", rM_keshiyewufei.LSJL)
				};

                int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, para);
                if (t > 0)
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


        public static DataTable GetAllRM_keshiyewufei()
        {
            string sqlAll = "SELECT * FROM RM_keshiyewufei";
            return GetRM_keshiyewufeiBySql(sqlAll);
        }


        public static RM_keshiyewufei GetRM_keshiyewufeiByKSID(int kSID)
        {
            string sql = "SELECT * FROM RM_keshiyewufei WHERE KSID = @KSID";

            try
            {
                SqlParameter para = new SqlParameter("@KSID", kSID);
                DataTable dt = DBUnity.AdapterToTab(sql, para);

                if (dt.Rows.Count > 0)
                {
                    RM_keshiyewufei rM_keshiyewufei = new RM_keshiyewufei();

                    rM_keshiyewufei.KSID = dt.Rows[0]["KSID"] == DBNull.Value ? 0 : (int)dt.Rows[0]["KSID"];
                    rM_keshiyewufei.Depname = dt.Rows[0]["Depname"] == DBNull.Value ? "" : (string)dt.Rows[0]["Depname"];
                    rM_keshiyewufei.BQJE = dt.Rows[0]["BQJE"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["BQJE"];
                    rM_keshiyewufei.SQJE = dt.Rows[0]["SQJE"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["SQJE"];
                    rM_keshiyewufei.ZJJE = dt.Rows[0]["ZJJE"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["ZJJE"];
                    rM_keshiyewufei.KYJE = dt.Rows[0]["KYJE"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["KYJE"];
                    rM_keshiyewufei.Remark = dt.Rows[0]["Remark"] == DBNull.Value ? 0 : (decimal)dt.Rows[0]["Remark"];
                    rM_keshiyewufei.Remark1 = dt.Rows[0]["Remark1"] == DBNull.Value ? "" : (string)dt.Rows[0]["Remark1"];
                    rM_keshiyewufei.LSJL = dt.Rows[0]["LSJL"] == DBNull.Value ? "" : (string)dt.Rows[0]["LSJL"];
                    return rM_keshiyewufei;
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




        private static DataTable GetRM_keshiyewufeiBySql(string safeSql)
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

        private static DataTable GetRM_keshiyewufeiBySql(string sql, params SqlParameter[] values)
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
