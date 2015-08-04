using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BudgetWeb.Model;
using BudgetWeb.DAL;
using System.Data;
using System.Data.SqlClient;

namespace BudgetWeb.BLL
{
    public class BG_ChaSecAuditLogic
    {
        public static DataTable GetChaSecAudit(int dept, int year, int month,int pici)
        {
            string sqlStr = "select PRID, PIEcoSubParID,PIEcoSubName,MPFundingAdd*10000 as MPFunding,MASta,MACause ,CPID,BG_PayIncome.PIID,DeptID,MPTime,MPFundingAdd,MPFunding as Mon from vMonPlayRemark left join BG_PayIncome on vMonPlayRemark.PIID=BG_PayIncome.PIID left join  dbo.BG_Department on vMonPlayRemark.deptid=BG_Department.DepID where (({0}=0 ) or ({0}<>0 and  deptid={0})) and  year(MPTime)={1} and month(MPTime)={2} and  (({3}=0 ) or ({3}<>0 and  MATimes={3})) and MASta='提交'";
            sqlStr = string.Format(sqlStr, dept, year, month,pici);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        } 

        public static DataTable GetChaSecAuditsh(int depid, int year, int month)
        {
            string sqlStr = "select * from vMonPlayRemark left join BG_PayIncome on vMonPlayRemark.PIID=BG_PayIncome.PIID left join  dbo.BG_Department on vMonPlayRemark.deptid=BG_Department.DepID where (({0}=0 ) or ({0}<>0 and  deptid={0})) and  year(MPTime)={1} and month(MPTime)={2} and MASta='审核通过'";
            sqlStr = string.Format(sqlStr, depid, year, month);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }

        public static DataTable GetChaSecAudit1(int dept, int year, int month,int pici)
        {
            string sqlStr = "SELECT   PIEcoSubParID,  PIEcoSubName, SUM(MPFunding) AS MPFunding, MASta, MACause FROM      (select a.PIEcoSubParID,isnull(b.PIEcoSubName,a.PIEcoSubName) as PIEcoSubName,sum(MPFundingAdd)*10000 as MPFunding,MASta,null as MACause,sum(MPFunding) as Mon from  (select  BG_PayIncome.PIID,PIEcoSubName,MPFunding,MASta,MACause,MPFundingAdd,BG_PayIncome.PIEcoSubParID from vMonPlayRemark left join BG_PayIncome on vMonPlayRemark.PIID=BG_PayIncome.PIID left join  dbo.BG_Department on vMonPlayRemark.deptid=BG_Department.DepID where (({0}=0 ) or ({0}<>0 and  deptid={0})) and  year(MPTime)={1} and month(MPTime)={2} and  (({3}=0 ) or ({3}<>0 and  MATimes={3})) and MASta='提交' ) as a left join[dbo].[BG_PayIncome] as b on a.PIEcoSubParID=b.PIID group by a.PIEcoSubParID,b.PIEcoSubName,MASta,a.PIEcoSubName) AS z GROUP BY PIEcoSubParID,  PIEcoSubName, MASta, MACause";
            sqlStr = string.Format(sqlStr, dept, year, month,pici);
            DataTable dt = DBUnity.AdapterToTab(sqlStr);
            return dt;
        }
    }
}
