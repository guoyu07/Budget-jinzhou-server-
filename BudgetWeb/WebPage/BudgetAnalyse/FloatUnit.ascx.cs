using BudgetWeb.BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class WebPage_BudgetAnalyse_FloatUnit : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            hidRecordCount.Value = IncomeContrastpayLogic.GetRecordCount().ToString();
        }
        ele9.InnerText = IncomeContrastpayLogic.GetUnitByUnitCode();

    }
    public event EventHandler Click;

    protected void Button_Click(object sender, EventArgs e)
    {
        Click(this, EventArgs.Empty);
    } 
}