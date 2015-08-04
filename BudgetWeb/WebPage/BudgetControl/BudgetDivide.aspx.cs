using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BudgetWeb.BLL;
using BudgetWeb.Model;
using Common;
using System.Reflection;
public partial class WebPage_BudgetControl_BudgetDivide : BudgetBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtshow.Style.Add("color", "red");
        //cmbPPAData();
        DepDataBind();
    }
    private DataTable getnewpi(DataTable dt)
    {
        System.Data.DataView dv = dt.DefaultView;
        DataTable dtnew = dv.ToTable(true);
        DataRow dr = dtnew.NewRow();
        object[] objs = { "全部" };
        dr.ItemArray = objs;
        dtnew.Rows.InsertAt(dr, 0);
        return dtnew;
    }
    //private void cmbPPAData()
    //{
    //    DataTable dt = BG_PayIncomeLogic.GetAllBG_PayIncome();
    //    cmbPPAstore.DataSource = getnewpi(dt);
    //    cmbPPAstore.DataBind();
    //}

    private void DepDataBind()
    {
        DataTable dt = BGDepartmentManager.GetDepByfadepid(AreaDepID);
        cmbDep.Items.Add(new Ext.Net.ListItem("全局", "0"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //  cmbDepnaem.Items.Add(new Ext.Net.ListItem(depTable.Rows[i]["depName"].ToString(), depTable.Rows[i]["depID"].ToString()));
            cmbDep.Items.Add(new Ext.Net.ListItem(dt.Rows[i]["DepName"].ToString(), dt.Rows[i]["DepID"].ToString()));
        }

    }
    protected void submit_DirectClick(object sender, DirectEventArgs e)
    {
        List<object> strlist = GetListStr();
        // DivideStore.DataSource = strlist;
        DivideStore.DataSource = GetNewList(strlist);
        DivideStore.DataBind();
    }

    private List<object> GetListStr()
    {
        txtshow.Text = "";
        int depid = common.IntSafeConvert(cmbDep.SelectedItem.Value);
        List<object> strlist = new List<object>();
        int piidflag = 0;
        string depname = "";
        string name = "";
        decimal mon = 0;
        decimal supp = 0;
        if (depid == 0 && DropDownFieldDv.Text.ToString() == "全部")
        {
            DataTable dt = BG_BudgetAllocationLogic.GetALLAAMon(common.IntSafeConvert(CurrentYear));
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    depname = BG_DepartmentManager.GetBG_DepartmentByDepID(common.IntSafeConvert(dt.Rows[i]["depid"])).DepName;
            //    name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dt.Rows[i]["PIID"])).PIEcoSubName;
            //    strlist.Add(new { depname = depname, name = name, mon = mon });
            //}
            if (dt.Rows.Count <= 0)
            {
                strlist.Clear();
                string message = "没有查询到数据";
                txtshow.Text = message;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    piidflag = common.IntSafeConvert(dt.Rows[i]["PIID"]);
                    depname = BG_DepartmentManager.GetBG_DepartmentByDepID(common.IntSafeConvert(dt.Rows[i]["DepID"])).DepName;
                    name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dt.Rows[i]["PIID"])).PIEcoSubName;
                    mon = ParToDecimal.ParToDel(dt.Rows[i]["BAAMon"].ToString());
                    supp = ParToDecimal.ParToDel(dt.Rows[i]["SuppMon"].ToString());
                    strlist.Add(new { piidflag = piidflag, depname = depname, name = name, mon = mon, supp = supp });
                }
            }
        }
        else if (depid == 0 && DropDownFieldDv.Text.ToString() != "全部")
        {
            name = DropDownFieldDv.Text.ToString();
            DataTable dt = BGDepartmentManager.GetDepByfadepid(AreaDepID);
            string bgpi = BG_BudItemsLogic.GetBG_PayIncomeByname(name);
            if (name == "工资福利支出" || name == "商品和服务支出" || name == "对个人和家庭补助支出" || name == "其他资本性支出")
            {
                DataTable dta = BG_BudgetAllocationLogic.GetALLAAMon(common.IntSafeConvert(CurrentYear));
                //for (int i = 0; i < dta.Rows.Count; i++)
                //{
                //    depname = BG_DepartmentManager.GetBG_DepartmentByDepID(common.IntSafeConvert(dta.Rows[i]["depid"])).DepName;
                //    name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dta.Rows[i]["PIID"])).PIEcoSubName;
                //    strlist.Add(new { depname = depname, name = name, mon = mon });
                //}
                if (dta.Rows.Count <= 0)
                {
                    strlist.Clear();
                    string message = "没有查询到数据";
                    txtshow.Text = message;
                }
                else
                {
                    for (int i = 0; i < dta.Rows.Count; i++)
                    {
                        piidflag = common.IntSafeConvert(dta.Rows[i]["PIID"]);
                        depname = BG_DepartmentManager.GetBG_DepartmentByDepID(common.IntSafeConvert(dta.Rows[i]["DepID"])).DepName;
                        name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dta.Rows[i]["PIID"])).PIEcoSubName;
                        mon = ParToDecimal.ParToDel(dta.Rows[i]["BAAMon"].ToString());
                        supp = ParToDecimal.ParToDel(dta.Rows[i]["SuppMon"].ToString());
                        strlist.Add(new { piidflag = piidflag, depname = depname, name = name, mon = mon, supp = supp });
                    }
                }
            }

            string[] slist = bgpi.Split(',');
            DataTable dt1 = new DataTable();
            for (int i = 0; i < slist.Count(); i++)
            {
                int piid = common.IntSafeConvert(slist[i]);
                dt1 = BG_BudgetAllocationLogic.GetAAMonDTbyPIID(common.IntSafeConvert(CurrentYear), piid);
                if (dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        depname = BG_DepartmentManager.GetBG_DepartmentByDepID(common.IntSafeConvert(dt1.Rows[j]["DepID"])).DepName;
                        mon += ParToDecimal.ParToDel(dt1.Rows[j]["BAAMon"].ToString());
                        supp += ParToDecimal.ParToDel(dt1.Rows[j]["SuppMon"].ToString());
                        strlist.Add(new { piidflag = piid, depname = depname, name = name, mon = mon, supp = supp });
                    }
                }
            }
            if (strlist.Count <= 0)
            {
                strlist.Clear();
                string message = "没有查询到数据";
                txtshow.Text = message;
            }
        }
        else if (depid != 0 && DropDownFieldDv.Text.ToString() == "全部")
        {
            depname = cmbDep.SelectedItem.Text;
            DataTable dt = BG_BudgetAllocationLogic.GetAAMonDTbyDepID(common.IntSafeConvert(CurrentYear), common.IntSafeConvert(cmbDep.SelectedItem.Value));
            if (dt.Rows.Count <= 0)
            {
                strlist.Clear();
                string message = "没有查询到数据";
                txtshow.Text = message;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    piidflag = common.IntSafeConvert(dt.Rows[i]["PIID"]);
                    name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(piidflag).PIEcoSubName;
                    mon = ParToDecimal.ParToDel(dt.Rows[i]["BAAMon"].ToString());
                    supp = ParToDecimal.ParToDel(dt.Rows[i]["SuppMon"].ToString());
                    strlist.Add(new { piidflag = piidflag, depname = depname, name = name, mon = mon, supp = supp });
                }
            }
        }
        else
        {
            name = DropDownFieldDv.Text.ToString();
            if (name == "工资福利支出" || name == "商品和服务支出" || name == "对个人和家庭补助支出" || name == "其他资本性支出")
            {
                depname = cmbDep.SelectedItem.Text;
                DataTable dta = BG_BudgetAllocationLogic.GetAAMonDTbyDepID(common.IntSafeConvert(CurrentYear), common.IntSafeConvert(cmbDep.SelectedItem.Value));
                if (dta.Rows.Count <= 0)
                {
                    strlist.Clear();
                    string message = "没有查询到数据";
                    txtshow.Text = message;
                }
                else
                {
                    for (int i = 0; i < dta.Rows.Count; i++)
                    {
                        piidflag = common.IntSafeConvert(dta.Rows[i]["PIID"]);
                        name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(piidflag).PIEcoSubName;
                        mon = ParToDecimal.ParToDel(dta.Rows[i]["BAAMon"].ToString());
                        supp = ParToDecimal.ParToDel(dta.Rows[i]["SuppMon"].ToString());
                        strlist.Add(new { piidflag = piidflag, depname = depname, name = name, mon = mon, supp = supp });
                    }
                }
            }
            string bgpi = BG_BudItemsLogic.GetBG_PayIncomeByname(name);
            string[] slist = bgpi.Split(',');
            DataTable dt = new DataTable();
            for (int i = 0; i < slist.Count(); i++)
            {
                int piid = common.IntSafeConvert(slist[i]);
                dt = BG_BudgetAllocationLogic.GetAAMon(depid, piid, common.IntSafeConvert(CurrentYear));
                if (dt.Rows.Count > 0)
                {
                    mon += ParToDecimal.ParToDel(dt.Rows[0]["BAAMon"].ToString());
                    supp += ParToDecimal.ParToDel(dt.Rows[0]["SuppMon"].ToString());
                }
                mon += 0;
                supp += 0;
            }

            if (dt.Rows.Count <= 0)
            {
                strlist.Clear();
                string message = "没有查询到数据";
                txtshow.Text = message;
            }
            else
            {
                depname = cmbDep.SelectedItem.Text;
                strlist.Add(new { piidflag = common.IntSafeConvert(slist[0]), depname = depname, name = name, mon = mon, supp = supp });
            }
        }
        string str = "";
        decimal summon = 0;
        decimal sumsupp = 0;
        for (int i = 0; i < strlist.Count; i++)
        {
            str = strlist[i].ToString();
            summon += Getmon(str, "mon");
            sumsupp += Getmon(str, "supp");
        }
        strlist.Add(new { piidflag = 0, depname = "总计", name = "", mon = summon, supp = sumsupp });
        return strlist;
    }
    private List<object> GetNewList(List<object> strlist)
    {
        List<object> Listtmp = new List<object>();
        int psub = 0;
        decimal t1 = 0, t2 = 0, t3 = 0, t4 = 0;
        decimal s1 = 0, s2 = 0, s3 = 0, s4 = 0;
        for (int i = 0; i < strlist.Count - 1; i++)
        {
            DataTable dttmp = BG_PayIncomeLogic.GetBG_PayIncomeByname(strlist[i].ToString().Split(',')[2].Substring(7).Replace(" ", ""));
            BG_PayIncome pi = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dttmp.Rows[0]["PIID"]));
            psub = pi.PIEcoSubParID;
            if (psub == 0)
            {
                if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("工资福利支出"))
                {
                    t1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "工资福利支出", mon = t1, supp = s1 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("商品和服务支出"))
                {
                    t2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "商品和服务支出", mon = t2, supp = s2 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("对个人和家庭补助支出"))
                {
                    t3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "对个人和家庭补助支出", mon = t3, supp = s3 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("其他资本性支出"))
                {
                    t4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "其他资本性支出", mon = t4, supp = s4 });
                }
            }
            else
            {

                if (psub == 1000 || psub == 1015 || psub == 1051 || psub == 1065)
                {

                    if (psub == 1000)
                    {
                        t1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1015)
                    {
                        t2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1051)
                    {
                        t3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1065)
                    {
                        t4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                }

            }
        }
        if (DropDownFieldDv.Text.ToString() != "全部")
        {
            if (DropDownFieldDv.Value.ToString() == "1000")
            {
                Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t1, supp = s1 });
            }
            else if (DropDownFieldDv.Value.ToString() == "1015")
            { Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t2, supp = s2 }); }
            else if (DropDownFieldDv.Value.ToString() == "1051")
            {
                Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t3, supp = s3 });
            }
            else { Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t4, supp = s4 }); }

        }
        else
        {
            Listtmp.Add(new { piidflag = 1000, depname = "", name = "工资福利支出", mon = t1, supp = s1 });
            Listtmp.Add(new { piidflag = 1015, depname = "", name = "商品和服务支出", mon = t2, supp = s2 });
            Listtmp.Add(new { piidflag = 1051, depname = "", name = "对个人和家庭补助支出", mon = t3, supp = s3 });
            Listtmp.Add(new { piidflag = 1065, depname = "", name = "其他资本性支出", mon = t4, supp = s4 });
            Listtmp.Add(strlist[strlist.Count - 1]);
        }

        return Listtmp;
    }
    private decimal Getmon(string str, string mm)
    {
        int monend = 0;
        decimal montmp = 0;
        int monindex = str.IndexOf(mm);

        if (mm == "supp")
        {
            monend = str.IndexOf("}", monindex);
            montmp = ParToDecimal.ParToDel(str.Substring(monindex + 6, monend - monindex - 6));
        }
        else if (mm == "mon")
        {
            monend = str.IndexOf(",", monindex);
            montmp = ParToDecimal.ParToDel(str.Substring(monindex + 5, monend - monindex - 5));
        }
        return montmp;
    }



    [DirectMethod]
    public void GetRowexpand(string pisub)
    {
        //DataTable dttmp = BG_PayIncomeLogic.GetBG_PayIncomeByname(pisubName.Replace(" ", ""));
        int psub = common.IntSafeConvert(pisub);
        if (psub == 1000 || psub == 1015 || psub == 1051 || psub == 1065)
        {
            if (psub == 1000)
            {
                if (common.IntSafeConvert(Session["1000"]) == 1)
                {
                    Session["1000"] = 0;
                }
                else
                { Session["1000"] = 1; }
            }
            if (psub == 1015)
            {
                if (common.IntSafeConvert(Session["1015"]) == 1)
                {
                    Session["1015"] = 0;
                }
                else
                { Session["1015"] = 1; }
            }
            if (psub == 1051)
            {
                if (common.IntSafeConvert(Session["1051"]) == 1)
                {
                    Session["1051"] = 0;
                }
                else
                { Session["1051"] = 1; }
            }
            if (psub == 1065)
            {
                if (common.IntSafeConvert(Session["1065"]) == 1)
                {
                    Session["1065"] = 0;
                }
                else
                { Session["1065"] = 1; }
            }

            if (common.IntSafeConvert(Session[pisub]) == 0)
            {
                List<object> strlist = GetListStr();
                DivideStore.DataSource = GetNewList(strlist);
                DivideStore.DataBind();
            }
            else
            {
                List<object> strlist = GetListStr();

                if (!ISNode("1000"))
                {
                    DivideStore.DataSource = GetNewList(strlist, psub);
                }
                else if (!ISNode("1015"))
                {
                    DivideStore.DataSource = GetNewList(strlist, psub);
                }
                else if (!ISNode("1051"))
                {
                    DivideStore.DataSource = GetNewList(strlist, psub);
                }
                else if (!ISNode("1065"))
                {
                    DivideStore.DataSource = GetNewList(strlist, psub);
                }
                DivideStore.DataSource = GetNewList(strlist, psub);
                DivideStore.DataBind();
            }

        }
        else
        {
            return;
        }
    }
    private bool ISNode(string NodeID)
    {
        bool valid = false;
        string PIEcoSubName = "";
        if (common.IntSafeConvert(NodeID) == 0)
        {
            valid = true;
        }
        else
        {
            if (BG_PayIncomeLogic.GetLeverByPIID(2))
            {
                return true;
            }
            //if (BG_PayIncomeLogic.GetBoolByPiid(common.IntSafeConvert(NodeID)))
            //{
            //    return true;
            //}
            PIEcoSubName = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(NodeID)).PIEcoSubName;
            string va = "";
            string temstr = Listallocationstr;
            string[] liststr = temstr.Split('*');
            for (int i = 0; i < liststr.Length; i++)
            {
                string[] temtt = liststr[i].Split(',');
                for (int j = 0; j < temtt.Length; j++)
                {
                    va = temtt[j];
                    if (PIEcoSubName == va.Trim())
                    {
                        valid = true;
                    }
                }
            }
        }
        return valid;
    }
    private object GetNewList(List<object> strlist, int piid)
    {
        List<object> Listtmp = new List<object>();
        int psub = 0;
        decimal t1 = 0, t2 = 0, t3 = 0, t4 = 0;
        decimal s1 = 0, s2 = 0, s3 = 0, s4 = 0;
        for (int i = 0; i < strlist.Count - 1; i++)
        {
            DataTable dttmp = BG_PayIncomeLogic.GetBG_PayIncomeByname(strlist[i].ToString().Split(',')[2].Substring(7).Replace(" ", ""));
            BG_PayIncome pi = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dttmp.Rows[0]["PIID"]));
            psub = pi.PIEcoSubParID;
            if (psub == 0)
            {
                if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("工资福利支出"))
                {
                    t1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "工资福利支出", mon = t1, supp = s1 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("商品和服务支出"))
                {
                    t2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "商品和服务支出", mon = t2, supp = s2 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("对个人和家庭补助支出"))
                {
                    t3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "对个人和家庭补助支出", mon = t3, supp = s3 });

                }
                else if (strlist[i].ToString().Split(',')[2].Substring(7).Contains("其他资本性支出"))
                {
                    t4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                    s4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    //Listtmp.Add(new { depname = "", name = "其他资本性支出", mon = t4, supp = s4 });
                }
            }
            else
            {

                if (psub == 1000 || psub == 1015 || psub == 1051 || psub == 1065)
                {

                    if (psub == 1000)
                    {
                        t1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s1 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1015)
                    {
                        t2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s2 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1051)
                    {
                        t3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s3 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                    if (psub == 1065)
                    {
                        t4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[3].Substring(6));
                        s4 += ParToDecimal.ParToDel(strlist[i].ToString().Split(',')[4].Substring(7).Replace("}", ""));
                    }
                }

            }
        }
        if (DropDownFieldDv.Text.ToString() != "全部")
        {
            if (DropDownFieldDv.Value.ToString() == "1000")
            {
                Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t1, supp = s1 });
            }
            else if (DropDownFieldDv.Value.ToString() == "1015")
            { Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t2, supp = s2 }); }
            else if (DropDownFieldDv.Value.ToString() == "1051")
            {
                Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t3, supp = s3 });
            }
            else { Listtmp.Add(new { piidflag = DropDownFieldDv.Value.ToString(), depname = "", name = DropDownFieldDv.Text.ToString(), mon = t4, supp = s4 }); }

        }
        else
        {
            Listtmp.Add(new { piidflag = 1000, depname = "", name = "工资福利支出", mon = t1, supp = s1 });
            Listtmp.Add(new { piidflag = 1015, depname = "", name = "商品和服务支出", mon = t2, supp = s2 });
            Listtmp.Add(new { piidflag = 1051, depname = "", name = "对个人和家庭补助支出", mon = t3, supp = s3 });
            Listtmp.Add(new { piidflag = 1065, depname = "", name = "其他资本性支出", mon = t4, supp = s4 });
            //Listtmp.Add(strlist[strlist.Count - 1]);
        }
        List<object> getleaf = GetLeafList(strlist, piid);
        if (piid == 1000)
        {
            for (int i = 0; i < getleaf.Count; i++)
            {
                Listtmp.Insert(1 + i, getleaf[i]);
            }
        }
        if (piid == 1015)
        {
            for (int i = 0; i < getleaf.Count; i++)
            {
                Listtmp.Insert(2 + i, getleaf[i]);
            }

        }
        if (piid == 1051)
        {
            for (int i = 0; i < getleaf.Count; i++)
            {
                Listtmp.Insert(3 + i, getleaf[i]);
            }
        }
        if (piid == 1065)
        {
            for (int i = 0; i < getleaf.Count; i++)
            {
                Listtmp.Insert(4 + i, getleaf[i]);
            }
        }
        if (DropDownFieldDv.Text.ToString() == "全部")
        {
            Listtmp.Add(strlist[strlist.Count - 1]);
        }
       
        return Listtmp;
    }

    private static List<object> GetLeafList(List<object> strlist, int piid)
    {
        List<object> getleaf = new List<object>();
        for (int i = 0; i < strlist.Count - 1; i++)
        {
            int piidtmpp = common.IntSafeConvert(strlist[i].ToString().Split(',')[0].Substring(12).Replace(" ", ""));
            DataTable dttmp = BG_PayIncomeLogic.GetBG_PayIncomeByname(strlist[i].ToString().Split(',')[2].Substring(7).Replace(" ", ""));
            BG_PayIncome pi = new BG_PayIncome();
            if (dttmp.Rows.Count > 4)
            {
                pi = BG_PayIncomeManager.GetBG_PayIncomeByPIID(piidtmpp);
            }
            else
            {
                pi = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(dttmp.Rows[0]["PIID"]));
            }
            int psubtmp = pi.PIEcoSubParID;
            if (psubtmp == piid)
            {
                getleaf.Add(strlist[i]);
            }
        }
        //DataTable dt = new DataTable();
        //dt.Columns.Add("name");
        //dt.Columns.Add("mon"); 
        //dt.Columns.Add("supp");
        List<object> listTmp = new List<object>();
        if (getleaf == null || getleaf.Count < 1)
        {
            return listTmp;
        }
        DataTable dt = ListToDataTable<object>(getleaf);
        var query = from t in dt.AsEnumerable()
                    group t by new { t1 = t.Field<string>("name") } into m
                    select new
                    {
                        name = m.Key.t1,
                        mon = m.Sum(n => n.Field<decimal>("mon")),
                        supp = m.Sum(n => n.Field<decimal>("supp"))
                    };
        if (query.ToList().Count > 0)
        {
            query.ToList().ForEach(q =>
            {
                listTmp.Add(new { piidflag = 0, depname = "", name = q.name, mon = q.mon, supp = q.supp });
            });
        }

        return listTmp;
    }


    /// <summary>
    /// 将泛类型集合List类转换成DataTable
    /// </summary>
    /// <param name="list">泛类型集合</param>
    /// <returns></returns>
    public static DataTable ListToDataTable<T>(List<T> entitys)
    {
        //检查实体集合不能为空
        if (entitys == null || entitys.Count < 1)
        {
            throw new Exception("需转换的集合为空");
        }
        //取出第一个实体的所有Propertie
        Type entityType = entitys[0].GetType();
        PropertyInfo[] entityProperties = entityType.GetProperties();

        //生成DataTable的structure
        //生产代码中，应将生成的DataTable结构Cache起来，此处略
        DataTable dt = new DataTable();
        for (int i = 0; i < entityProperties.Length; i++)
        {
            //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            if (i > 2)
            {
                dt.Columns.Add(entityProperties[i].Name, typeof(decimal));
            }
            else
            {
                dt.Columns.Add(entityProperties[i].Name, typeof(string));
            }
        }
        //将所有entity添加到DataTable中
        foreach (object entity in entitys)
        {
            //检查所有的的实体都为同一类型
            if (entity.GetType() != entityType)
            {
                throw new Exception("要转换的集合元素类型不一致");
            }
            object[] entityValues = new object[entityProperties.Length];
            for (int i = 0; i < entityProperties.Length; i++)
            {
                entityValues[i] = entityProperties[i].GetValue(entity, null);
            }
            dt.Rows.Add(entityValues);
        }
        return dt;
    }

    [DirectMethod]
    public string NodeLoad(string NodeID)
    {
        if (!ISNode(NodeID))
        {
            return "failure";
        }
        NodeCollection nodes = new NodeCollection();
        int tem = 1;
        int nodeID = common.IntSafeConvert(NodeID);
        string Financial_allocation = "财政拨款";
        string Other_funds = "其他资金";
        string BasicIncome = "基本支出";
        string ProjectIncome = "项目支出";
        Node rootNode = new Node();
        Node nodeO = new Node();
        Node nodeF = new Node();
        Node nodeFB = new Node();
        Node nodeFP = new Node();
        Node nodeOB = new Node();
        Node nodeOP = new Node();
        if (NodeID == "root")
        {
            rootNode.Text = "全部";
            rootNode.NodeID = "全部";
            rootNode.Icon = Icon.Folder;
            nodes.Add(rootNode);
            rootNode.Expanded = true;
        }
        //else if (NodeID == "PA")
        //{
        //    if (SingleNode(NodeID) == 2)
        //    {
        //        nodeF.NodeID = "nodeF";
        //        nodeF.Text = Financial_allocation;
        //        nodeF.Icon = Icon.Folder;
        //        nodes.Add(nodeF);
        //        nodeF.Expanded = true;
        //    }
        //    //else if (SingleNode(NodeID) == 1)
        //    //{
        //    //    nodeO.NodeID = "nodeO";
        //    //    nodeO.Text = Other_funds;
        //    //    nodeO.Icon = Icon.Folder;
        //    //    nodes.Add(nodeO);
        //    //    nodeO.Expanded = true;
        //    //}
        //    else if (SingleNode(NodeID) == 3)
        //    {
        //        nodeF.NodeID = "nodeF";
        //        nodeF.Text = Financial_allocation;
        //        nodeF.Icon = Icon.Folder;
        //        nodes.Add(nodeF);
        //        //nodeO.NodeID = "nodeO";
        //        //nodeO.Text = Other_funds;
        //        //nodeO.Icon = Icon.Folder;
        //        //nodes.Add(nodeO);
        //        //nodeO.Expanded = true;
        //        nodeF.Expanded = true;
        //    }
        //}
        //else if (NodeID == "nodeF")
        //{
        //    //if (SingleNode(NodeID) == 21)
        //    //{
        //    //    nodeFB.NodeID = "nodeFB";
        //    //    nodeFB.Text = BasicIncome;
        //    //    nodeFB.Icon = Icon.Folder;
        //    //    nodes.Add(nodeFB);
        //    //    nodeFB.Expanded = true;
        //    //}
        //    //else if (SingleNode(NodeID) == 22)
        //    //{
        //    //    nodeFP.NodeID = "nodeFP";
        //    //    nodeFP.Text = ProjectIncome;
        //    //    nodeFP.Icon = Icon.Folder;
        //    //    nodes.Add(nodeFP);
        //    //    nodeFP.Expanded = true;
        //    //}
        //    //else if (SingleNode(NodeID) == 23)
        //    //{
        //    //    nodeFB.NodeID = "nodeFB";
        //    //    nodeFB.Text = BasicIncome;
        //    //    nodeFB.Icon = Icon.Folder;
        //    //    nodes.Add(nodeFB);

        //    //    nodeFP.NodeID = "nodeFP";
        //    //    nodeFP.Text = ProjectIncome;
        //    //    nodeFP.Icon = Icon.Folder;
        //    //    nodes.Add(nodeFP);
        //    //    nodeFB.Expanded = true;
        //    //    nodeFP.Expanded = true;
        //    //}
        //    nodeFB.NodeID = "nodeFB";
        //    nodeFB.Text = BasicIncome;
        //    nodeFB.Icon = Icon.Folder;
        //    nodes.Add(nodeFB);
        //    nodeFB.Expanded = true;
        //}
        //else if (NodeID == "nodeO")
        //{
        //    if (SingleNode(NodeID) == 11)
        //    {
        //        nodeOB.NodeID = "nodeOB";
        //        nodeOB.Text = BasicIncome;
        //        nodeOB.Icon = Icon.Folder;
        //        nodes.Add(nodeOB);
        //        nodeOB.Expanded = true;
        //    }
        //    else if (SingleNode(NodeID) == 12)
        //    {
        //        nodeOP.NodeID = "nodeOP";
        //        nodeOP.Text = ProjectIncome;
        //        nodeOP.Icon = Icon.Folder;
        //        nodes.Add(nodeOP);
        //        nodeOP.Expanded = true;
        //    }
        //    else if (SingleNode(NodeID) == 13)
        //    {
        //        nodeOB.NodeID = "nodeOB";
        //        nodeOB.Text = BasicIncome;
        //        nodeOB.Icon = Icon.Folder;
        //        nodes.Add(nodeOB);
        //        nodeOP.NodeID = "nodeOP";
        //        nodeOP.Text = ProjectIncome;
        //        nodeOP.Icon = Icon.Folder;
        //        nodes.Add(nodeOP);
        //        nodeOB.Expanded = true;
        //        nodeOP.Expanded = true;
        //    }
        //}
        else if (NodeID == "全部")
        {
            SetNode(tem, Financial_allocation, BasicIncome, nodes);
        }
        //else if (NodeID == "nodeFP")
        //{
        //    SetNode(tem, Financial_allocation, ProjectIncome, nodes);
        //}
        //else if (NodeID == "nodeOB")
        //{
        //    SetNode(tem, Other_funds, BasicIncome, nodes);
        //}
        //else if (NodeID == "nodeOP")
        //{
        //    SetNode(tem, Other_funds, ProjectIncome, nodes);
        //}
        if (nodeID >= 1000)
        {
            SetNode(nodeID, nodes);
        }
        return nodes.ToJson();
    }
    private void SetNode(int tem, string ftype, string incomeinfo, NodeCollection node)
    {
        NodeCollection nodes = new NodeCollection();
        DataTable dt = BG_PayIncomeLogic.GetDtPayIncome(incomeinfo, ftype, tem);
        if (dt.Rows.Count > 0)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                int piid = common.IntSafeConvert(dt.Rows[j]["PIID"].ToString());
                Node nodeN = new Node();
                nodeN.NodeID = piid.ToString();
                nodeN.Text = dt.Rows[j]["PIEcoSubName"].ToString();
                if (!BG_PayIncomeLogic.GetBoolPayIncome(incomeinfo, ftype, piid))
                {
                    nodeN.Icon = Icon.Anchor;
                    node.Add(nodeN);
                    nodeN.Leaf = true;
                    //Session["slist"] = listpiid;
                    //node.CustomAttributes.Add(new ConfigItem("BAAMon", CountBaamon.ToString(), ParameterMode.Value));
                    //node.CustomAttributes.Add(new ConfigItem("SuppMon", CountSuppmon.ToString(), ParameterMode.Value));
                    //nodeN.Checked = false; 
                }
                else
                {
                    if (SingleNode(piid.ToString()) == 0)
                    {
                        break;
                    }
                    else if (BG_PayIncomeLogic.ISSign(piid))
                    {

                        nodeN.Leaf = true;
                        nodeN.Icon = Icon.Anchor;
                        node.Add(nodeN);

                        //Session["slist"] = listpiid;
                    }
                    else
                    {
                        nodeN.Icon = Icon.Folder;
                        node.Add(nodeN);
                    }
                    //11
                    //nodeN.Expanded = true;
                    //nodeN.CustomAttributes.Add(new ConfigItem("BAAMon", CountBaamon.ToString(), ParameterMode.Value));
                    //nodeN.CustomAttributes.Add(new ConfigItem("SuppMon", CountSuppmon.ToString(), ParameterMode.Value));
                    //CountBaamon = 0;
                    //CountSuppmon = 0;
                }
                //SetNode(piid, ftype, incomeinfo, nodeN);
                //node.Children.Add(nodeN); 
                //SetNode(piid, ftype, incomeinfo, nodeN);
            }

        }
    }
    private void SetNode(int tem, NodeCollection node)
    {
        NodeCollection nodes = new NodeCollection();
        DataTable dt = BG_PayIncomeLogic.GetDtPayIncomeByPIID(tem);
        if (dt.Rows.Count > 0)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                int piid = common.IntSafeConvert(dt.Rows[j]["PIID"].ToString());
                Node nodeN = new Node();
                nodeN.NodeID = piid.ToString();
                nodeN.Text = dt.Rows[j]["PIEcoSubName"].ToString();

                if (BG_PayIncomeLogic.GetBoolPayIncomeByPIID(tem))
                {

                    if (!BG_PayIncomeLogic.GetBoolPayIncomeByPIID(common.IntSafeConvert(piid)))
                    {
                        nodeN.Icon = Icon.Anchor;
                        node.Add(nodeN);
                        nodeN.Leaf = true;
                    }
                    else
                    {
                        if (BG_PayIncomeLogic.GetLever(3))
                        {
                            nodeN.Icon = Icon.Anchor;
                            node.Add(nodeN);
                            nodeN.Leaf = true;

                        }
                        else
                        {
                            nodeN.Icon = Icon.Folder;
                            node.Add(nodeN);
                        }
                    }
                }
                else
                {
                    if (SingleNode(piid.ToString()) == 0)
                    {
                        break;
                    }
                    else if (BG_PayIncomeLogic.ISSign(piid))
                    {

                        nodeN.Leaf = true;
                        nodeN.Icon = Icon.Anchor;
                        node.Add(nodeN);

                        //Session["slist"] = listpiid;
                    }
                    else
                    {
                        nodeN.Icon = Icon.Folder;
                        node.Add(nodeN);
                    }


                    //nodeN.Expanded = true;
                    //nodeN.CustomAttributes.Add(new ConfigItem("BAAMon", CountBaamon.ToString(), ParameterMode.Value));
                    //nodeN.CustomAttributes.Add(new ConfigItem("SuppMon", CountSuppmon.ToString(), ParameterMode.Value));
                    //CountBaamon = 0;
                    //CountSuppmon = 0;
                }
                //SetNode(piid, ftype, incomeinfo, nodeN);
                //node.Children.Add(nodeN); 
                //SetNode(piid, ftype, incomeinfo, nodeN);
            }
        }
    }
    private int SingleNode(string NodeID)
    {
        int t = 0;
        string tem;
        if (common.IntSafeConvert(NodeID) == 0)
        {
            if (NodeID == "全部")
            {
                if (Listallocationstr.Contains("财政拨款") && !Listallocationstr.Contains("其他资金"))
                {
                    t = 2;
                }
                else if (Listallocationstr.Contains("其他资金") && !Listallocationstr.Contains("财政拨款"))
                {
                    t = 1;
                }
                else
                {
                    t = 3;
                }
            }
            if (NodeID == "nodeF")
            {
                if (Listallocationstr.Contains("基本支出") && !Listallocationstr.Contains("项目支出"))
                {
                    t = 21;
                }
                else if (Listallocationstr.Contains("项目支出") && !Listallocationstr.Contains("基本支出"))
                {
                    t = 22;
                }
                else
                {
                    t = 23;
                }
            }
            if (NodeID == "nodeO")
            {
                if (Listallocationstr.Contains("基本支出") && !Listallocationstr.Contains("项目支出"))
                {
                    t = 11;
                }
                else if (Listallocationstr.Contains("项目支出") && !Listallocationstr.Contains("基本支出"))
                {
                    t = 12;
                }
                else
                {
                    t = 13;
                }
            }
        }
        else
        {
            string name = BG_PayIncomeManager.GetBG_PayIncomeByPIID(common.IntSafeConvert(NodeID)).PIEcoSubName;
            if (Listallocationstr.Contains(name))
            {
                t = 1000;
            }
        }
        return t;
    }
}