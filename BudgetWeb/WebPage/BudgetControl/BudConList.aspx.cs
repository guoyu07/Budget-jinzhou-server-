using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BudgetWeb.BLL;
using BudgetWeb.Model;
using Common;

public partial class BudgetPage_mainPages_BudConList : BudgetBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack )
        {
            Session["BaseMon"] = 0;
            Session["TotalMon"] = 0;
            GetYear();
            BGBudItemsDataBind();
            BGProvBudItemsDataBind();
            DtDataBind();
           
        }

    }
    private void DtDataBind()
    {
        decimal txt = 0;
        decimal txt1 = 0;
        int year = Convert.ToInt32(HidYear.Value);
        decimal pbbase= BG_BudItemsLogic.GetTotal(year);
        Session["BaseMon"] = pbbase;
        int bgmonid = BG_MonLogic.GEtIDisEditMon(common.IntSafeConvert(CurrentYear));
        BG_Mon bgMon = BG_MonManager.GetBG_MonByBGID(bgmonid);

        if (bgMon != null && bgMon.IsEditMon == 1)
        {
            txt = bgMon.BGMon;
        }
        else
        {
            DataTable dt1 = BG_BudItemsLogic.GetPayOne(year);
            if (dt1.Rows.Count > 0)
            {
                txt += ParToDecimal.ParToDel(dt1.Rows[0]["POTitol"].ToString());
            }
            DataTable dt2 = BG_BudItemsLogic.GetPayTwo(year);
            if (dt1.Rows.Count > 0)
            {
                if (dt2.Rows.Count > 0)
                {
                    txt += ParToDecimal.ParToDel(dt2.Rows[0]["PTTitol"].ToString());
                }
            }
            DataTable dt3 = BG_BudItemsLogic.GetPubPay(year);
            if (dt1.Rows.Count > 0)
            {
                if (dt3.Rows.Count > 0)
                {
                    txt += ParToDecimal.ParToDel(dt3.Rows[0]["PBIDTitol"].ToString());
                }
            }
            DataTable dt4 = BG_BudItemsLogic.GetProPay(year);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    txt += Convert.ToDecimal(dt4.Rows[i]["ProPA0M"]);
                }
            }
        }
        txt1 = txt;
        DataTable dt5 = BG_BudItemsLogic.GetBudgetAllocation(year);
        if (dt5.Rows.Count > 0)
        {
            for (int i = 0; i < dt5.Rows.Count; i++)
            {
                txt -= ParToDecimal.ParToDel(dt5.Rows[i]["BAAMon"].ToString());
            }
        }
        DataTable dtpre = BG_PreLogic.GetBG_PreByyear(common.IntSafeConvert(CurrentYear));
        decimal premon = 0;
        if (dtpre == null || dtpre.Rows.Count == 0)
        {
            premon = 0;
        }
        else { premon = ParToDecimal.ParToDel(dtpre.Rows[0]["PreMon"].ToString()); }
        Session["TotalMon"] = (txt + premon).ToString("f8").TrimEnd('0').TrimEnd('.');
    }
    private void GetYear()
    {
        HidYear.Value = CurrentYear;
    }


    private void BGBudItemsDataBind()
    {
        int year = Convert.ToInt32(HidYear.Value);
        DataTable dt = BG_BudItemsLogic.GetBudItem(year);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }
    private void BGProvBudItemsDataBind()
    {
        int year = Convert.ToInt32(HidYear.Value);
        DataTable dt = BG_BudItemsLogic.GetAllProPay(year);
        repBudConList.DataSource = dt;
        repBudConList.DataBind();
    }
    protected void Detail(object source, RepeaterCommandEventArgs e)
    {
        Response.Redirect("BudConPageDet.aspx", true);
    }
}