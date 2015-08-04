using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BudgetWeb.DAL;
using BudgetWeb.Model;
using Common;
using System.Configuration;
using System.Data.SqlClient;

namespace BudgetWeb.BLL
{
    public class IncomeContrastpayLogic
    {
        private static string unitCode = ConfigurationManager.AppSettings["UnitCode"];

        public static string UnitCode
        {
            get { return IncomeContrastpayLogic.unitCode; }
            set { IncomeContrastpayLogic.unitCode = value; }
        }
        public static DataTable GetUnit(int pageindex, int Pagesize)
        {
            string sqlStr = " select UnitCode,UnitName from UnitTable ";
            DataTable dt = DBUnity.GetAspNetPager(sqlStr, pageindex, Pagesize);
            return dt;
        }

        public static DataTable GetICPDTByDepID_Time(int DepID, int Month)
        {
            string sqlStr = " select *  from ( select *  from (SELECT   dbo.BG_Unit_IncomeCPay.ICPID, dbo.BG_Unit_IncomeCPay.DepID, dbo.BG_Unit_IncomeCPay.InComeSouce, dbo.BG_Unit_IncomeCPay.InComeMon, dbo.BG_Unit_IncomeCPay.ICPTime, dbo.BG_Unit_Department.DepName FROM      dbo.BG_Unit_IncomeCPay LEFT OUTER JOIN dbo.BG_Unit_Department ON dbo.BG_Unit_IncomeCPay.DepID = dbo.BG_Unit_Department.DepID and BG_Unit_IncomeCPay.UnitCode=BG_Unit_Department.UnitCode  and BG_Unit_Department.UnitCode='{2}') as a where (({0}=0)or({0}<>0 and DepID={0})) and (({1}=0)or({1}<>0 and month(ICPTime)={1}))) as x left join   (select RPDep,sum(GKZC) as GKZC ,sum(QTZJ) as QTZJ, sum(XJZC) as XJZC   from [dbo].[RM_Unit_Receipts]  group by RPDep )as z on  x.DepName=z.RPDep";
            sqlStr = string.Format(sqlStr, DepID, Month, UnitCode);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }
        public static DataTable GetARMonByBAAYear(string BAAYear)
        {
            DataTable dt = null;
            try
            {
                string str = string.Format(" select DepID, Sum(BAAMon) as BAAMon from dbo.BG_Unit_BudgetAllocation where BAAYear={0} and UnitCode='{1}'  group by DepID", BAAYear, UnitCode);
                dt = DBUnity.AdapterToTab(str);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static string GetDepBydepid(string depid)
        {
            string dep = "";
            try
            {
                string sqlStr = "select DepName from BG_Unit_Department where DepID='{0}' and UnitCode='{1}' ";
                sqlStr = string.Format(sqlStr, depid, UnitCode);
                dep = DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null);

            }
            catch
            {
                dep = "";
            }
            return dep;
        }
        public static DataTable GetBG_SupplementaryDivide(int depid, int year)
        {
            string sqlStr = "select DepID , sum(SuppMon) as SuppMon from dbo.BG_Unit_BudgetAllocation where BAAYear={0} and DepID={1} and UnitCode='{2}' group by DepID ";
            sqlStr = string.Format(sqlStr, year, depid, UnitCode);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }
        public static DataTable GetICPStDTByDepID_Time(int DepID, int Month)
        {
            string sqlStr = "select *  from (SELECT   dbo.BG_Unit_IncomeCPay.ICPID, dbo.BG_Unit_IncomeCPay.DepID, dbo.BG_Unit_IncomeCPay.InComeSouce, dbo.BG_Unit_IncomeCPay.InComeMon, dbo.BG_Unit_IncomeCPay.ICPTime, dbo.BG_Unit_Department.DepName FROM      dbo.BG_Unit_IncomeCPay LEFT OUTER JOIN dbo.BG_Unit_Department ON dbo.BG_Unit_IncomeCPay.DepID = dbo.BG_Unit_Department.DepID and BG_Unit_IncomeCPay.UnitCode=BG_Unit_Department.UnitCode  and BG_Unit_IncomeCPay.UnitCode='{2}') as a where DepID={0} and month(ICPTime)={1} ";
            sqlStr = string.Format(sqlStr, DepID, Month, UnitCode);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }

        public static DataTable GetDepByfadepid(int fadepid)
        {
            DataTable dt = null;
            try
            {
                string sqlStr = "select *  from BG_Unit_Department where FaDepID ={0} and UnitCode='{1}'";
                sqlStr = string.Format(sqlStr, fadepid, UnitCode);
                dt = DBUnity.AdapterToTab(sqlStr);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static DataTable GetZXMonByZXTime(string ARTime, int depid)
        {
            string StartYearMonth = ARTime.Split('-')[0] + "-00";
            DataTable dt = null;
            try
            {
                string sql = string.Format("select SUM(RPMoney)/10000 from dbo.RM_Unit_Receipts where RPStatus='已完成' and rpdep  in ( select DepName  from [BG_Unit_Department] where  DepID={1} and UnitCode='{3}') and  convert(varchar(7),rptime,120) <='{0}' and convert(varchar(7),rptime,120) >'{2}' and UnitCode='{3}'", ARTime, depid, StartYearMonth, UnitCode);
                dt = DBUnity.AdapterToTab(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static DataTable GetAllDT(string yearMonth, int DepID)
        {
            string StartYearMonth = yearMonth.Split('-')[0] + "-00";
            DataTable dtResult = new DataTable();
            DataTable dtResult1 = new DataTable();
            DataTable dtResult2 = new DataTable();
            try
            {
                string sql = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney  PIEcoSubName  from ( select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成' ,'审核通过','提交') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney  FROM CTEGetParent where[PIEcoSubLev]=1  fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode);

                string sql1 = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney  PIEcoSubName  from ( select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成' ,'审核通过') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney  FROM CTEGetParent where[PIEcoSubLev]=1  fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode);

                string sql2 = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney  PIEcoSubName  from ( select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney  FROM CTEGetParent where[PIEcoSubLev]=1  fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode);
                dtResult = GetDtResult(sql, dtResult);
                dtResult1 = GetDtResult(sql, dtResult1);
                dtResult2 = GetDtResult(sql, dtResult2);
                dtResult.Columns.Add("RPMoney1");
                dtResult.Columns.Add("RPMoney2");
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtResult.Rows[i]["RPMoney1"] = dtResult1.Rows[i]["RPMoney"];
                    dtResult.Rows[i]["RPMoney2"] = dtResult2.Rows[i]["RPMoney"];
                }
            }
            catch
            {
                dtResult = null;
            }
            return dtResult;
        }

        private static DataTable GetDtResult(string sql, DataTable dtResult)
        {
            DataTable dtall = new DataTable();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(DBUnity.connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                conn.Close();
            }

            foreach (DataColumn dc in ds.Tables[0].Columns) //遍历所有的列
            {
                if (!dtall.Columns.Contains(dc.ColumnName))
                {
                    dtall.Columns.Add(dc.ColumnName);
                }
            }
            foreach (DataTable dt in ds.Tables)
            {
                DataRow dr = dtall.NewRow();
                for (int i = 0; i < dtall.Columns.Count; i++)
                {
                    dr[i] = dt.Rows[0][i];
                }
                dtall.Rows.Add(dr);
            }
            //var query = from t in dtall.AsEnumerable()
            //            group t by new { t1 = t.Field<int>("piid"), t2 = t.Field<string>("PIEcoSubName") } into m
            //            select new
            //            {
            //                piid = m.Key.t1,
            //                PIEcoSubName = m.Key.t2,
            //                totalMon = m.Sum(n => n.Field<decimal>("totalMon")),
            //                MPFunding = m.Sum(n => n.Field<decimal>("MPFunding")),
            //                RPMoney = m.Sum(n => n.Field<decimal>("RPMoney"))
            //            };
            //dtquery = query.ToList();
            dtResult = dtall.Clone();
            for (int i = 0; i < dtall.Rows.Count; )
            {
                DataRow dr = dtResult.NewRow();
                string piid = dtall.Rows[i]["piid"].ToString();
                string PIEcoSubName = dtall.Rows[i]["PIEcoSubName"].ToString();
                dr["piid"] = piid;
                dr["PIEcoSubName"] = PIEcoSubName;
                decimal totalMon = 0, MPFunding = 0, RPMoney = 0;
                //内层也是循环同一个表，当遇到不同的name时跳出内层循环
                for (; i < dtall.Rows.Count; )
                {
                    if (piid == dtall.Rows[i]["piid"].ToString() && PIEcoSubName == dtall.Rows[i]["PIEcoSubName"].ToString())
                    {
                        totalMon += ParToDecimal.ParToDel(dtall.Rows[i]["totalMon"].ToString());
                        MPFunding += ParToDecimal.ParToDel(dtall.Rows[i]["MPFunding"].ToString());
                        RPMoney += ParToDecimal.ParToDel(dtall.Rows[i]["RPMoney"].ToString());
                        dr["totalMon"] = totalMon;
                        dr["MPFunding"] = MPFunding;
                        dr["RPMoney"] = RPMoney;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                dtResult.Rows.Add(dr);
            }
            return dtResult;
        }

        public static DataTable GetAllDT(string yearMonth, int DepID, int pisubid)
        {
            string StartYearMonth = yearMonth.Split('-')[0] + "-00";
            DataTable dtall = new DataTable();
            DataTable dtall1 = new DataTable();
            DataTable dtall2 = new DataTable();
            try
            {
                string sql = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney,PIEcoSubName from (select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成' ,'审核通过','提交') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal DECLARE @ChildPIEcoSubName nvarchar(200) open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT  @ChildID as ChildID,piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney ,@ChildPIEcoSubName as ChildPIEcoSubName FROM CTEGetParent where[PIEcoSubLev]=1 and piid={4} fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode, pisubid);
                string sql1 = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney,PIEcoSubName from (select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成' ,'审核通过') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal DECLARE @ChildPIEcoSubName nvarchar(200) open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT  @ChildID as ChildID,piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney ,@ChildPIEcoSubName as ChildPIEcoSubName  FROM CTEGetParent where[PIEcoSubLev]=1 and piid={4}  fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode, pisubid);

                string sql2 = string.Format("declare unit_cursor cursor for select piid,totalMon,MPFunding,RPMoney,PIEcoSubName from (select PIEcoSubName,BG_Unit_MonPayPlan.PIID,(sum(BAAMon)  + sum(SUPPMon)) as totalMon,sum(MPFunding) as MPFunding,SUM(RPMoney)/10000 as RPMoney from [dbo].[BG_Unit_MonPayPlan] left join    [dbo].[BG_Unit_BudgetAllocation] on [BG_Unit_MonPayPlan].PIID=[BG_Unit_BudgetAllocation].PIID  and [BG_Unit_BudgetAllocation].DepID=[BG_Unit_MonPayPlan].[DeptID] and BG_Unit_MonPayPlan.UnitCode=BG_Unit_BudgetAllocation.UnitCode left join [dbo].[BG_Unit_MonPayPlanRemark] on [BG_Unit_MonPayPlanRemark].[MATime]=[BG_Unit_MonPayPlan].MPTime and [BG_Unit_MonPayPlanRemark].[DeptID]=[dbo].[BG_Unit_MonPayPlan].[DeptID] left join [dbo].[BG_PayIncome] on [BG_PayIncome].PIID=[BG_Unit_MonPayPlan].PIID   left join [dbo].[RM_Unit_Receipts]  on RM_Unit_Receipts.[rpdep] in  (select DepName from [dbo].[BG_Unit_Department] where  DepID= [BG_Unit_MonPayPlan].[DeptID] and RM_Unit_Receipts.RPRemark3=[BG_PayIncome].PIEcoSubName and UnitCode='{3}') and convert(varchar(7),RM_Unit_Receipts.[rptime],120)= convert(varchar(7),[BG_Unit_MonPayPlan].[MPTime],120) and RM_Unit_Receipts.ADType='费用支出' and RM_Unit_Receipts.RPStatus in('已完成') where (({1}=0)or ({1}<>0 and [BG_Unit_MonPayPlan].[DeptID]={1})) and BAAYear =SUBSTRING('{0}',0,5)   and convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)<='{0}' and  convert(varchar(7),[BG_Unit_MonPayPlan].MPTime,120)>'{2}' and  MASta='审核通过'  and  [BG_Unit_MonPayPlan].UnitCode='{3}' Group by PIEcoSubName,BG_Unit_MonPayPlan.PIID)as zb DECLARE @ChildID int DECLARE @totalMon decimal DECLARE @MPFunding decimal  DECLARE @RPMoney decimal DECLARE @ChildPIEcoSubName nvarchar(200) open unit_cursor;fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin DECLARE @CETParentID int select @CETParentID=[PIEcoSubParID] FROM [BG_PayIncome] where [PIID]=@ChildID; with CTEGetParent as (select * from [BG_PayIncome] where [PIID]=@CETParentID UNION ALL (SELECT a.* from [BG_PayIncome] as a inner join CTEGetParent as b on a.[PIID]=b.[PIEcoSubParID] ))SELECT  @ChildID as ChildID,piid,PIEcoSubName,@totalMon as totalMon,@MPFunding as MPFunding,@RPMoney as  RPMoney ,@ChildPIEcoSubName as ChildPIEcoSubName FROM CTEGetParent where[PIEcoSubLev]=1 and piid={4}  fetch  next from unit_cursor into @ChildID,@totalMon,@MPFunding,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor ", yearMonth, DepID, StartYearMonth, UnitCode, pisubid);
                dtall = GetDtAll(sql);
                dtall1 = GetDtAll(sql1);
                dtall2 = GetDtAll(sql2);
                dtall.Columns.Add("RPMoney1");
                dtall.Columns.Add("RPMoney2");
                for (int i = 0; i < dtall.Rows.Count; i++)
                {
                    dtall.Rows[i]["RPMoney1"] = dtall1.Rows[i]["RPMoney"];
                    dtall.Rows[i]["RPMoney2"] = dtall2.Rows[i]["RPMoney"];
                }
                //var query = from t in dtall.AsEnumerable()
                //            group t by new { t1 = t.Field<int>("piid"), t2 = t.Field<string>("PIEcoSubName") } into m
                //            select new
                //            {
                //                piid = m.Key.t1,
                //                PIEcoSubName = m.Key.t2,
                //                totalMon = m.Sum(n => n.Field<decimal>("totalMon")),
                //                MPFunding = m.Sum(n => n.Field<decimal>("MPFunding")),
                //                RPMoney = m.Sum(n => n.Field<decimal>("RPMoney"))
                //            };
                //dtquery = query.ToList();
                //dtResult = dtall.Clone();
                //for (int i = 0; i < dtall.Rows.Count; )
                //{
                //    DataRow dr = dtResult.NewRow();
                //    string piid = dtall.Rows[i]["piid"].ToString();
                //    string PIEcoSubName = dtall.Rows[i]["PIEcoSubName"].ToString();
                //    dr["piid"] = piid;
                //    dr["PIEcoSubName"] = PIEcoSubName;
                //    decimal totalMon = 0, MPFunding = 0, RPMoney = 0;
                //    //内层也是循环同一个表，当遇到不同的name时跳出内层循环
                //    for (; i < dtall.Rows.Count; )
                //    {
                //        if (piid == dtall.Rows[i]["piid"].ToString() && PIEcoSubName == dtall.Rows[i]["PIEcoSubName"].ToString())
                //        {
                //            totalMon += ParToDecimal.ParToDel(dtall.Rows[i]["totalMon"].ToString());
                //            MPFunding += ParToDecimal.ParToDel(dtall.Rows[i]["MPFunding"].ToString());
                //            RPMoney += ParToDecimal.ParToDel(dtall.Rows[i]["RPMoney"].ToString());
                //            dr["totalMon"] = totalMon;
                //            dr["MPFunding"] = MPFunding;
                //            dr["RPMoney"] = RPMoney;
                //            i++;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    dtResult.Rows.Add(dr);
                //}
            }
            catch
            {
                dtall = null;
            }
            return dtall;
        }

        private static DataTable GetDtAll(string sql)
        {
            DataTable dtall = new DataTable();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(DBUnity.connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                conn.Close();
            }

            foreach (DataColumn dc in ds.Tables[0].Columns) //遍历所有的列
            {
                if (!dtall.Columns.Contains(dc.ColumnName))
                {
                    dtall.Columns.Add(dc.ColumnName);
                }
            }
            if (ds.Tables.Count == 0)
            {
                return dtall;
            }
            foreach (DataTable dt in ds.Tables)
            {
                DataRow dr = dtall.NewRow();
                for (int i = 0; i < dtall.Columns.Count; i++)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dr[i] = dt.Rows[0][i];
                    }
                    else
                    {
                        dr = null;
                        break;
                    }
                }
                if (dr != null)
                {
                    dtall.Rows.Add(dr);
                }
            }
            return dtall;
        }


        public static int GetRecordCount()
        {
            int t = 0;
            try
            {
                string sqlStr = "select count(*) from UnitTable ";
                t = common.IntSafeConvert(DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null));
            }
            catch
            {
                t = 0;
            }
            return t;

        }

        public static DataTable GetUnitByName(string filter)
        {
            DataTable dt = null;
            try
            {
                string sql = string.Format(" select UnitCode,UnitName from UnitTable where UnitName like '%{0}%' ", filter);
                dt = DBUnity.AdapterToTab(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }

        public static string GetUnitByUnitCode()
        {
            string str = "";
            try
            {
                string sql = string.Format(" select UnitName from UnitTable where UnitCode='{0}' ", UnitCode);
                str = DBUnity.ExecuteScalar(CommandType.Text, sql, null);
            }
            catch
            {
                str = "选择单位";
            }
            return str;
        }

        public static DataTable GetAllDT(string yearMonth, string zclx)
        {
            string StartYearMonth = yearMonth.Split('-')[0] + "-00";
            DataTable dtResult = new DataTable();
            string total="",rpmoney = "", rpmoney1 = "", rpmoney2 = "";
            int remark = 0;
            if (zclx == "科室业务费")
            {
                remark = 0;
            }
            else { remark =1; }
            try
            {
                string sqltotal = string.Format("select SUM(KYJE)/10000 from dbo.RM_keshiyewufei where Remark1='{1}' and Remark='{0}'", remark, yearMonth.Split('-')[0]);

                string sql = string.Format("select SUM(RPMoney)/10000 from dbo.RM_Unit_Receipts where ADType='{1}' and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' and RPStatus in('已完成' ,'审核通过','提交')  and UnitCode='{3}'", yearMonth, zclx, StartYearMonth, UnitCode);

                string sql1 = string.Format("select SUM(RPMoney)/10000 from dbo.RM_Unit_Receipts where ADType='{1}' and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' and RPStatus in('已完成' ,'审核通过')  and UnitCode='{3}'", yearMonth, zclx, StartYearMonth, UnitCode);

                string sql2 = string.Format("select SUM(RPMoney)/10000 from dbo.RM_Unit_Receipts where ADType='{1}' and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' and RPStatus in('已完成')  and UnitCode='{3}'", yearMonth, zclx, StartYearMonth, UnitCode);
                total = DBUnity.ExecuteScalar(CommandType.Text, sqltotal, null);
                rpmoney = DBUnity.ExecuteScalar(CommandType.Text, sql, null);
                rpmoney1 = DBUnity.ExecuteScalar(CommandType.Text, sql, null);
                rpmoney2 = DBUnity.ExecuteScalar(CommandType.Text, sql, null);
                dtResult.Columns.Add("piid");
                dtResult.Columns.Add("totalMon");
                dtResult.Columns.Add("RPMoney");
                dtResult.Columns.Add("RPMoney1");
                dtResult.Columns.Add("RPMoney2");
                dtResult.Columns.Add("PIEcoSubName");
                DataRow dr = dtResult.NewRow();
                dr["piid"] = 0;
                dr["totalMon"] = total;
                dr["RPMoney"] = rpmoney;
                dr["RPMoney1"] = rpmoney1;
                dr["RPMoney2"] = rpmoney2;
                dr["PIEcoSubName"] = zclx;
                dtResult.Rows.Add(dr);
            }
            catch
            {
                dtResult = null;
            }
            return dtResult;
        }

        public static DataTable GetAllDT(string yearMonth, string zclx, int pisubid)
        {
            string StartYearMonth = yearMonth.Split('-')[0] + "-00";
            string year = yearMonth.Split('-')[0];
            int remark = 0;
            if (zclx == "局长基金")
            {
                remark = 1;
            }
            else { remark = 0; }
            DataTable dtall = new DataTable();
            DataTable dtall1 = new DataTable();
            DataTable dtall2 = new DataTable();
            try
            {
                string sql = string.Format("declare @ChildID int  declare @totalMon decimal  declare @RPMoney decimal  declare @ChildPIEcoSubName nvarchar(200)   declare unit_cursor cursor  for select 0 as  ChildID,KYJE as totalMon,0 as RPMoney,Depname as ChildPIEcoSubName   from RM_keshiyewufei  where Remark={4}   and Remark1={5} open unit_cursor; fetch  next from unit_cursor into  @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin select @ChildID as ChildID,@totalMon/10000 as totalMon,sum(RPMoney)/10000 as  RPMoney ,@ChildPIEcoSubName  as ChildPIEcoSubName from [dbo].[RM_Unit_Receipts] where [RPDep]=@ChildPIEcoSubName and [ADType]='{1}'  and UnitCode='{3}'  and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' fetch  next from unit_cursor into @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor", yearMonth, zclx, StartYearMonth, UnitCode, remark, year);
                string sql1 = string.Format("declare @ChildID int  declare @totalMon decimal  declare @RPMoney decimal  declare @ChildPIEcoSubName nvarchar(200)   declare unit_cursor cursor  for select 0 as  ChildID,KYJE as totalMon,0 as RPMoney,Depname as ChildPIEcoSubName   from RM_keshiyewufei  where Remark={4}   and Remark1={5} open unit_cursor; fetch  next from unit_cursor into  @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin select @ChildID as ChildID,@totalMon/10000 as totalMon,sum(RPMoney)/10000 as  RPMoney ,@ChildPIEcoSubName  as ChildPIEcoSubName from [dbo].[RM_Unit_Receipts] where [RPDep]=@ChildPIEcoSubName and [ADType]='{1}'  and UnitCode='{3}'  and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' fetch  next from unit_cursor into @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor", yearMonth, zclx, StartYearMonth, UnitCode, remark, year);

                string sql2 = string.Format("declare @ChildID int  declare @totalMon decimal  declare @RPMoney decimal  declare @ChildPIEcoSubName nvarchar(200)   declare unit_cursor cursor  for select 0 as  ChildID,KYJE as totalMon,0 as RPMoney,Depname as ChildPIEcoSubName   from RM_keshiyewufei  where Remark={4}  and Remark1={5} open unit_cursor; fetch  next from unit_cursor into  @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName ;  while(@@fetch_status = 0) begin select @ChildID as ChildID,@totalMon/10000 as totalMon,sum(RPMoney)/10000 as  RPMoney ,@ChildPIEcoSubName  as ChildPIEcoSubName from [dbo].[RM_Unit_Receipts] where [RPDep]=@ChildPIEcoSubName and [ADType]='{1}'  and UnitCode='{3}'  and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' fetch  next from unit_cursor into @ChildID,@totalMon,@RPMoney,@ChildPIEcoSubName;  end close unit_cursor; DEALLOCATE unit_cursor", yearMonth, zclx, StartYearMonth, UnitCode, remark, year);
                dtall = GetDtAll(sql);
                dtall1 = GetDtAll(sql1);
                dtall2 = GetDtAll(sql2);
                dtall.Columns.Add("RPMoney1");
                dtall.Columns.Add("RPMoney2");
                for (int i = 0; i < dtall.Rows.Count; i++)
                {
                    dtall.Rows[i]["RPMoney1"] = dtall1.Rows[i]["RPMoney"];
                    dtall.Rows[i]["RPMoney2"] = dtall2.Rows[i]["RPMoney"];
                }
                //var query = from t in dtall.AsEnumerable()
                //            group t by new { t1 = t.Field<int>("piid"), t2 = t.Field<string>("PIEcoSubName") } into m
                //            select new
                //            {
                //                piid = m.Key.t1,
                //                PIEcoSubName = m.Key.t2,
                //                totalMon = m.Sum(n => n.Field<decimal>("totalMon")),
                //                MPFunding = m.Sum(n => n.Field<decimal>("MPFunding")),
                //                RPMoney = m.Sum(n => n.Field<decimal>("RPMoney"))
                //            };
                //dtquery = query.ToList();
                //dtResult = dtall.Clone();
                //for (int i = 0; i < dtall.Rows.Count; )
                //{
                //    DataRow dr = dtResult.NewRow();
                //    string piid = dtall.Rows[i]["piid"].ToString();
                //    string PIEcoSubName = dtall.Rows[i]["PIEcoSubName"].ToString();
                //    dr["piid"] = piid;
                //    dr["PIEcoSubName"] = PIEcoSubName;
                //    decimal totalMon = 0, MPFunding = 0, RPMoney = 0;
                //    //内层也是循环同一个表，当遇到不同的name时跳出内层循环
                //    for (; i < dtall.Rows.Count; )
                //    {
                //        if (piid == dtall.Rows[i]["piid"].ToString() && PIEcoSubName == dtall.Rows[i]["PIEcoSubName"].ToString())
                //        {
                //            totalMon += ParToDecimal.ParToDel(dtall.Rows[i]["totalMon"].ToString());
                //            MPFunding += ParToDecimal.ParToDel(dtall.Rows[i]["MPFunding"].ToString());
                //            RPMoney += ParToDecimal.ParToDel(dtall.Rows[i]["RPMoney"].ToString());
                //            dr["totalMon"] = totalMon;
                //            dr["MPFunding"] = MPFunding;
                //            dr["RPMoney"] = RPMoney;
                //            i++;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    dtResult.Rows.Add(dr);
                //}
            }
            catch
            {
                dtall = null;
            }
            return dtall;
        }

        public static DataTable GetICPDTByDepID_Time(string zclx, int Month)
        {
            string sqlStr = " select *  from ( select *  from (SELECT   dbo.BG_Unit_IncomeCPay.ICPID, dbo.BG_Unit_IncomeCPay.DepID, dbo.BG_Unit_IncomeCPay.InComeSouce, dbo.BG_Unit_IncomeCPay.InComeMon, dbo.BG_Unit_IncomeCPay.ICPTime, dbo.BG_Unit_Department.DepName FROM      dbo.BG_Unit_IncomeCPay LEFT OUTER JOIN dbo.BG_Unit_Department ON dbo.BG_Unit_IncomeCPay.DepID = dbo.BG_Unit_Department.DepID and BG_Unit_IncomeCPay.UnitCode=BG_Unit_Department.UnitCode  and BG_Unit_Department.UnitCode='{2}') as a where ADType='{0}' and (({1}=0)or({1}<>0 and month(ICPTime)={1}))) as x left join   (select RPDep,sum(GKZC)/10000 as GKZC ,sum(QTZJ)/10000 as QTZJ, sum(XJZC)/10000 as XJZC   from [dbo].[RM_Unit_Receipts]  group by RPDep )as z on  x.DepName=z.RPDep";
            sqlStr = string.Format(sqlStr, zclx, Month, UnitCode);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }
      
        public static DataTable GetICPDTByDepID_TimeUnit(string zclx, int Month)
        {
            string sqlStr = " select *  from ( select *  from (SELECT   dbo.BG_Unit_IncomeCPay.ICPID, dbo.BG_Unit_IncomeCPay.DepID, dbo.BG_Unit_IncomeCPay.InComeSouce, dbo.BG_Unit_IncomeCPay.InComeMon, dbo.BG_Unit_IncomeCPay.ICPTime, dbo.BG_Unit_Department.DepName FROM      dbo.BG_Unit_IncomeCPay LEFT OUTER JOIN dbo.BG_Unit_Department ON dbo.BG_Unit_IncomeCPay.DepID = dbo.BG_Unit_Department.DepID and BG_Unit_IncomeCPay.UnitCode=BG_Unit_Department.UnitCode  and BG_Unit_Department.UnitCode='{2}') as a where (({1}=0)or({1}<>0 and month(ICPTime)={1}))) as x left join   (select RPDep,sum(GKZC)/10000 as GKZC ,sum(QTZJ)/10000 as QTZJ, sum(XJZC)/10000 as XJZC   from [dbo].[RM_Unit_Receipts] where  ADType='{0}' group by RPDep )as z on  x.DepName=z.RPDep  ";
            sqlStr = string.Format(sqlStr, zclx, Month, UnitCode);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }

        public static decimal GetARmon(string ARTime, string zclx)
        {
            decimal total = 0;
            int remark = 0;
            if (zclx == "科室业务费")
            {
                remark = 0;
            }
            else { remark = 1; }
            string sqlStr = string.Format("select sum(BQJE+SQJE)/10000 from dbo.RM_keshiyewufei where Remark1='{1}' and Remark='{0}'", remark, ARTime.Split('-')[0]);
            total = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null));
            return total;
        }

        public static decimal GetARmonZJ(string ARTime, string zclx)
        {
            decimal total = 0;
            int remark = 0;
            if (zclx == "科室业务费")
            {
                remark = 0;
            }
            else { remark = 1; }
            string sqlStr = string.Format("select  (sum(KYJE)-sum(BQJE+SQJE)) /10000 from dbo.RM_keshiyewufei where Remark1='{1}' and Remark='{0}'", remark, ARTime.Split('-')[0]);
            total = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null));
            return total;
        }


        public static decimal GetUserMon(string ARTime, string zclx)
        {
            string StartYearMonth = ARTime.Split('-')[0] + "-00";
            decimal total = 0;
            int remark = 0;
            if (zclx == "科室业务费")
            {
                remark = 0;
            }
            else { remark = 1; }
            string sqlStr = string.Format("select SUM(RPMoney)/10000 from dbo.RM_Unit_Receipts where ADType='{1}' and convert(varchar(7),[rptime],120)>'{2}' and convert(varchar(7),[rptime],120)<'{0}' and RPStatus in('已完成' ,'审核通过','提交')  and UnitCode='{3}'", ARTime, zclx, StartYearMonth, UnitCode);
            total = ParToDecimal.ParToDel(DBUnity.ExecuteScalar(CommandType.Text, sqlStr, null));
            return total;
        }

        public static DataTable GetAllocation(int year)
        {
            string sqlStr = " declare @zclx int declare unit_cursor cursor for select Remark  from [dbo].[RM_keshiyewufei] where Remark1=2015   group by Remark  open unit_cursor;fetch  next from unit_cursor into @zclx ;  while(@@fetch_status = 0) begin  select @zclx as DepID, case when @zclx=0 then '科室业务费' when  @zclx=1 then '局长基金' END  as DepName,(sum(BQJE+SQJE))/10000 as BAAMon,(sum(KYJE)-sum(BQJE+SQJE))/10000 as SuppMon  from  [dbo].[RM_keshiyewufei] where Remark1=2015 and Remark=@zclx  fetch  next from unit_cursor into @zclx;  end close unit_cursor; DEALLOCATE unit_cursor ";
            sqlStr = string.Format(sqlStr, year);
            DataTable dt = new DataTable();
            dt=GetDtResult(sqlStr);
            return dt;
        }

        private static DataTable GetDtResult(string sql)
        { 
            DataTable dtall = new DataTable();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(DBUnity.connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                conn.Close();
            }
            if (ds.Tables.Count==0)
            {
                return dtall;
            }
            foreach (DataColumn dc in ds.Tables[0].Columns) //遍历所有的列
            {
                if (!dtall.Columns.Contains(dc.ColumnName))
                {
                    dtall.Columns.Add(dc.ColumnName);
                }
            }
            foreach (DataTable dt in ds.Tables)
            {
                DataRow dr = dtall.NewRow();
                for (int i = 0; i < dtall.Columns.Count; i++)
                {
                    dr[i] = dt.Rows[0][i];
                }
                dtall.Rows.Add(dr);
            }
            return dtall;
        }
    }
}
