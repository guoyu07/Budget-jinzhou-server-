using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BudgetWeb.BLL;
using BudgetWeb.Model;
using Common;

public partial class mainPages_BudConExeList : BudgetBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlTimeDataBind();
        }
        FloatUnit.Click += FloatUnit_Click;
    }

    void FloatUnit_Click(object sender, EventArgs e)
    {
        //DepDataBind();
    }
    private void ddlTimeDataBind()
    {
        int year = common.IntSafeConvert(CurrentYear);
        string str = "";
        for (int i = year; i > year - 5; i--)
        {
            str = i.ToString();
            ddlyear.Items.Add(new ListItem(str, str));
        }
        //ddlyear.DataTextField = "Year";
        //ddlyear.DataValueField = "Year";
        //ddlyear.DataSource = dt1;
        //ddlyear.DataBind();
    }

    //private void repBudConDataBind(string depname)
    //{
    //    DataTable dt = VApplyPayDepartManager.GetVApplyPayDepartByDepName(depname);
    //    repBudCon.DataSource = dt;
    //    repBudCon.DataBind();
    //}
    protected void btnInq_Click(object sender, EventArgs e)
    {
        lanotice.Text = "";
        string ARTime = ddlyear.Text.Trim() + "-" + DropDownList1.Text.Trim();
        string BAAYear = ddlyear.Text.Trim();
        DataTable dt2 = IncomeContrastpayLogic.GetARMonByBAAYear(BAAYear);
        if (dt2.Rows.Count < 1)
        {
            lanotice.Text = "查询结果为空！";
            repBudCon.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DepName");
            dt.Columns.Add("ARMon");
            dt.Columns.Add("ChangeMon");
            dt.Columns.Add("UserMon");
            dt.Columns.Add("Pecent");
            //dt.Columns.Add("");
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt.Rows.Add("");
                dt.Rows[i]["DepName"] = IncomeContrastpayLogic.GetDepBydepid(dt2.Rows[i]["DepID"].ToString());
                dt.Rows[i]["ARMon"] = Convert.ToDecimal(dt2.Rows[i]["BAAMon"]).ToString("f2").TrimEnd('0').TrimEnd('.');
                int depid = common.IntSafeConvert(dt2.Rows[i]["DepID"]);
                DataTable dtsupp = IncomeContrastpayLogic.GetBG_SupplementaryDivide(depid, common.IntSafeConvert(ddlyear.Text.Trim()));
                dt.Rows[i]["ChangeMon"] = Convert.ToDecimal(dtsupp.Rows[0]["SuppMon"]).ToString("f2").TrimEnd('0').TrimEnd('.');
                //DataTable dt1 = BG_ApplyReimburLogic.GetARMonByARTime(ARTime, int.Parse(dt2.Rows[i]["DepID"].ToString()));
                DataTable dt1 = IncomeContrastpayLogic.GetZXMonByZXTime(ARTime, int.Parse(dt2.Rows[i]["DepID"].ToString()));
                if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() != "")
                {
                    //dt.Rows[i]["Balance"] = (Convert.ToInt32(dt2.Rows[0]["BAAMon"]) - Convert.ToInt32(dt1.Rows[0]["ARMon"])).ToString();
                    dt.Rows[i]["UserMon"] = ParToDecimal.ParToDel((dt1.Rows[0][0]).ToString()).ToString("f2").TrimEnd('0').TrimEnd('.');
                }
                else
                {
                    //dt.Rows[i]["Balance"] = dt2.Rows[i]["BAAMon"].ToString();
                    dt.Rows[i]["UserMon"] = "0";
                }
                if (ParseUtil.ToDecimal(dt.Rows[i]["ARMon"].ToString(), 0) != 0)
                {
                    decimal usermon = ParseUtil.ToDecimal(dt.Rows[i]["UserMon"].ToString(), 0);
                    decimal armon = ParseUtil.ToDecimal(dt.Rows[i]["ARMon"].ToString(), 0) + ParseUtil.ToDecimal(dt.Rows[i]["ChangeMon"].ToString(), 0);
                    dt.Rows[i]["Pecent"] = ((usermon / armon) * 100).ToString("f2").TrimEnd('0') + "%";
                }
                else
                {
                    dt.Rows[i]["Pecent"] = "0%";
                }
            }
            decimal k1 = IncomeContrastpayLogic.GetARmon(ARTime, "科室业务费");
            decimal k2 = IncomeContrastpayLogic.GetUserMon(ARTime, "科室业务费");
            decimal zj1 = IncomeContrastpayLogic.GetARmonZJ(ARTime, "科室业务费");
            decimal j1 = IncomeContrastpayLogic.GetARmon(ARTime, "局长基金");
            decimal j2 = IncomeContrastpayLogic.GetUserMon(ARTime, "局长基金");
           
            decimal zj2 = IncomeContrastpayLogic.GetARmonZJ(ARTime, "局长基金");
            DataRow dr = dt.NewRow();
            dr["DepName"] = "科室业务费";
            dr["ARMon"] = k1.ToString("f2");
            dr["ChangeMon"] = zj1.ToString("f2");
            dr["UserMon"] = k2.ToString("f2");
            if (k2 == 0)
            {
                dr["Pecent"] = "0.00%"; 
            }
            else { dr["Pecent"] = k1+zj1 / k2*100 +"%"; }

            DataRow dr1 = dt.NewRow();
            dr1["DepName"] = "局长基金";
            dr1["ARMon"] = j1.ToString("f2");
            dr1["ChangeMon"] = zj2.ToString("f2");
            dr1["UserMon"] = j2.ToString("f2");
            if (j2 == 0)
            {
                dr1["Pecent"] = "0.00%";
            }
            else { dr1["Pecent"] = j1+zj2 / j2 * 100 + "%"; }
            dt.Rows.Add(dr);
            dt.Rows.Add(dr1);
            repBudCon.DataSource = dt;
            repBudCon.DataBind();
        }

    }
}
