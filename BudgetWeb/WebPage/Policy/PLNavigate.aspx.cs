using System;
using BudgetWeb.BLL;

public partial class WebPage_Policy_PLNavigate : BudgetBasePage
{
    public int Sjsbsh = 0;
    public int Sjsbtj, Sjsh;
    public int Sjtj;
    public string ymdate;
    //public string  Zt = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        hidtj.Visible = false;
        hidsh.Visible = false;
        hidsbtj.Visible = false;
        hidsbsh.Visible = false;
        yuemotixing.Visible = false;
        if (UserLimStr == "审核员" || UserLimStr == "出纳员" || UserLimStr == "局领导")
        {
            DateTime dt = DateTime.Now; //当前时间
            DateTime startMonth = dt.AddDays(1 - dt.Day); //本月月初
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1); //本月月末 
            int difdate = Math.Abs(((TimeSpan)(endMonth - dt)).Days);

            Sjtj = BG_MonPayPlanRemarkLogic.GetCountremark("未提交");
            Sjsh = BG_MonPayPlanRemarkLogic.GetCountremark("提交");
            Sjsbtj = BG_MonPayPlanRemarkLogic.GetCountReimbur("未提交");
            Sjsbsh = BG_MonPayPlanRemarkLogic.GetCountReimbur("提交");
            if (Sjtj > 0)
            {
                hidtj.Visible = true;
            }
            if (Sjsh > 0)
            {
                hidsh.Visible = true;
            }
            if (Sjsbtj > 0)
            {
                hidsbtj.Visible = true;
            }
            if (Sjsbsh > 0)
            {
                hidsbsh.Visible = true;
            }
            if (Sjtj > 0 || Sjsh > 0)
            {
                if (difdate < 3 && difdate >= 0)
                {
                    yuemotixing.Visible = true;
                    if (difdate == 0)
                    {
                        ymdate = "最后一天了未审批数据将会作废";
                    }
                    else
                    {
                        ymdate = "离计算还剩<p style='font-size=30px'>" + difdate + "</p>天";
                    }
                }
            }
           
        }
    }
}