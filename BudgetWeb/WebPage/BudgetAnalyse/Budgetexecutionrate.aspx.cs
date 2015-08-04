using BudgetWeb.BLL;
using BudgetWeb.Model;
using Common;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

public partial class WebPage_BudgetAnalyse_Budgetexecutionrate : BudgetBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DepDataBind();
        MonthDataBind();
        int Year = 0, Month = 0;
        DateTime mydate = DateTime.Now;
        Year = mydate.Year;
        Month = mydate.Month;
        FloatUnit.Click += FloatUnit_Click;
    }

    void FloatUnit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Budgetexecutionrate.aspx");
    }
    protected void exbtn_Click(object sender, EventArgs e)
    {
        if (GridData.Value.ToString() == "")
        {
            X.Msg.Alert("系统提示", "请先查询数据").Show();
        }

        ExportHtmlTableToExcel(GridData.Value.ToString(), "汇总查询");
    }

    /// <summary>
    /// 将DataTable导出到Excel
    /// </summary>
    /// <param name="htmlTable">html表格内容</param> 
    /// <param name="fileName">仅文件名（非路径）</param> 
    /// <returns>返回Excel文件绝对路径</returns>
    public void ExportHtmlTableToExcel(string htmlTable, string fileName)
    {
        #region 第一步：将HtmlTable转换为DataTable
        htmlTable = htmlTable.Replace("\"", "'");
        var trReg = new Regex(pattern: @"(?<=(<[t|T][r|R]))[\s\S]*?(?=(</[t|T][r|R]>))");
        var trMatchCollection = trReg.Matches(htmlTable);
        DataTable dt = new DataTable("data");
        for (int i = 0; i < trMatchCollection.Count; i++)
        {
            var row = "<tr " + trMatchCollection[i].ToString().Trim() + "</tr>";
            var tdReg = new Regex(pattern: @"(?<=(<[t|T][d|D|h|H]))[\s\S]*?(?=(</[t|T][d|D|h|H]>))");
            var tdMatchCollection = tdReg.Matches(row);
            if (i == 0)
            {
                foreach (var rd in tdMatchCollection)
                {
                    var tdValue = RemoveHtml("<td " + rd.ToString().Trim() + "</td>");
                    DataColumn dc = new DataColumn(tdValue);
                    dt.Columns.Add(dc);
                }
            }
            if (i > 0)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tdMatchCollection.Count; j++)
                {
                    var tdValue = RemoveHtml("<td " + tdMatchCollection[j].ToString().Trim() + "</td>");
                    dr[j] = tdValue;
                }
                dt.Rows.Add(dr);
            }
        }
        #endregion

        IWorkbook workbook = new HSSFWorkbook();
        ISheet sheet = workbook.CreateSheet();
        ICellStyle style = workbook.CreateCellStyle();
        style.Alignment = HorizontalAlignment.CENTER;
        style.VerticalAlignment = VerticalAlignment.CENTER;//垂直居中
        style.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
        style.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
        style.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
        style.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
        //这里是表头绘制
        IRow headrow = sheet.CreateRow(0);
        ICell cell = headrow.CreateCell(0);
        cell.SetCellValue("经济科目");
        cell.CellStyle = style;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
        ICell cell1 = headrow.CreateCell(1);
        cell1.SetCellValue("年初经费(万元)");
        cell1.CellStyle = style;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1));
        ICell cell2 = headrow.CreateCell(2);
        cell2.SetCellValue("预算执行");
        cell2.CellStyle = style;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 2, 4));
        ICell cell3 = headrow.CreateCell(5);
        cell3.SetCellValue("预算结余");
        cell3.CellStyle = style;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 5, 7));
        ICell cell4 = headrow.CreateCell(8);
        cell4.SetCellValue("预算执行率");
        cell4.CellStyle = style;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 8, 10));


        IRow headrow1 = sheet.CreateRow(1);
        ICell cell5 = headrow1.CreateCell(2);
        cell5.SetCellValue("申请数(万元)");
        cell5.CellStyle = style;
        ICell cell6 = headrow1.CreateCell(3);
        cell6.SetCellValue("已审核(万元)");
        cell6.CellStyle = style;
        ICell cell7 = headrow1.CreateCell(4);
        cell7.SetCellValue("已支付(万元)");
        cell7.CellStyle = style;
        ICell cell8 = headrow1.CreateCell(5);
        cell8.SetCellValue("申请数(万元)");
        cell8.CellStyle = style;
        ICell cell9 = headrow1.CreateCell(6);
        cell9.SetCellValue("已审核(万元)");
        cell9.CellStyle = style;
        ICell cell10 = headrow1.CreateCell(7);
        cell10.SetCellValue("已支付(万元)");
        cell10.CellStyle = style;
        ICell cell11 = headrow1.CreateCell(8);
        cell11.SetCellValue("申请数");
        cell11.CellStyle = style;
        ICell cell12 = headrow1.CreateCell(9);
        cell12.SetCellValue("已审核");
        cell12.CellStyle = style;
        ICell cell13 = headrow1.CreateCell(10);
        cell13.SetCellValue("已支付");
        cell13.CellStyle = style;
        #region 第二步：将DataTable导出到Excel
        if (dt != null && dt.Rows.Count > 0)
        {
            MemoryStream ms = ExcelRender.RenderToExcel(dt, workbook);
            Response.ContentType = "application/xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("预算执行率.xls", System.Text.Encoding.UTF8));
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
        else
        {
            this.Response.Write("<script>alert('无数据不允许导出')</script>");
        }
        #endregion
    }


    /// <summary>
    ///     去除HTML标记
    /// </summary>
    /// <param name="htmlstring"></param>
    /// <returns>已经去除后的文字</returns>
    public static string RemoveHtml(string htmlstring)
    {
        //删除脚本    
        htmlstring =
            Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>",
                          "", RegexOptions.IgnoreCase);
        //删除HTML    
        htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);


        htmlstring = htmlstring.Replace("<", "");
        htmlstring = htmlstring.Replace(">", "");
        htmlstring = htmlstring.Replace("\r\n", "");
        return htmlstring;
    }

    private void MonthDataBind()
    {
        int year = common.IntSafeConvert(CurrentYear);
        string str = "";
        for (int i = year; i > year - 5; i--)
        {
            str = i.ToString();
            cmbyear.Items.Add(new Ext.Net.ListItem(str, str));
        }
    }
    private void DepDataBind()
    {
        DepID = ((UserLimStr == "审核员" || UserLimStr == "出纳员") ? AreaDepID : DepID);
        if (DepID == AreaDepID)
        {
            DataTable dt = IncomeContrastpayLogic.GetDepByfadepid(AreaDepID);
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //  cmbDepnaem.Items.Add(new Ext.Net.ListItem(depTable.Rows[i]["depName"].ToString(), depTable.Rows[i]["depID"].ToString()));

                {
                    cmbdept.Items.Add(new Ext.Net.ListItem(dt.Rows[i]["DepName"].ToString(), dt.Rows[i]["DepID"].ToString()));
                }
            }
        }
        else
        {
            cmbdept.Items.Add(new Ext.Net.ListItem(DepName, DepID.ToString()));
        }
        cmbdept.Items.Add(new Ext.Net.ListItem("科室业务费", "科室业务费"));
        cmbdept.Items.Add(new Ext.Net.ListItem("局长基金", "局长基金"));
        cmbdept.Items.Insert(0, new Ext.Net.ListItem("全部", "0"));

    }
    protected void btnsend_DirectClick(object sender, DirectEventArgs e)
    {
        GetGridData();
    }

    private void GetGridData()
    {
        int depid = 0;
        string YearMonth = cmbyear.SelectedItem.Value + "-" + cmbmonth.SelectedItem.Value;
        DataTable dt = new DataTable();
        try
        {
            depid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
        }
        catch
        {
            return;
        }
        if (cmbdept.SelectedItem.Text == "科室业务费" || cmbdept.SelectedItem.Text == "局长基金")
        {
            dt = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text);
            if (dt == null || dt.Rows.Count == 0)
            {
                X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                return;
            }
            Store1.DataSource = dt;
            Store1.DataBind();
        }
        else
        {
            dt = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
            if (depid == 0)
            {
                DataTable dtbase1 = IncomeContrastpayLogic.GetAllDT(YearMonth, "科室业务费");
                DataTable dtbase2 = IncomeContrastpayLogic.GetAllDT(YearMonth, "局长基金");
                int t = dt.Rows.Count;
                for (int i = 0; i < dtbase1.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    //设定行中的值
                    row["piid"] = dtbase1.Rows[i]["piid"];
                    row["PIEcoSubName"] = dtbase1.Rows[i]["PIEcoSubName"];
                    row["totalMon"] = dtbase1.Rows[i]["totalMon"];
                    row["RPMoney"] = dtbase1.Rows[i]["RPMoney"];
                    row["RPMoney1"] = dtbase1.Rows[i]["RPMoney1"];
                    row["RPMoney2"] = dtbase1.Rows[i]["RPMoney2"];
                    dt.Rows.InsertAt(row, t++);
                }
                t = dt.Rows.Count;
                for (int i = 0; i < dtbase2.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    //设定行中的值
                    row["piid"] = dtbase2.Rows[i]["piid"];
                    row["PIEcoSubName"] = dtbase2.Rows[i]["PIEcoSubName"];
                    row["totalMon"] = dtbase2.Rows[i]["totalMon"];
                    row["RPMoney"] = dtbase2.Rows[i]["RPMoney"];
                    row["RPMoney1"] = dtbase2.Rows[i]["RPMoney1"];
                    row["RPMoney2"] = dtbase2.Rows[i]["RPMoney2"];
                    dt.Rows.InsertAt(row, t++);
                }
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                return;
            }
            Store1.DataSource = GetTotal(dt);
            Store1.DataBind();

        }


    }

    [DirectMethod]
    public void DB(string pisubid)
    {
        int psub = common.IntSafeConvert(pisubid);
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
            if (common.IntSafeConvert(Session[pisubid]) == 0)
            {
                GetGridData();
            }
            else
            {
                GetGridData(pisubid);
            }
        }
        else if (pisubid == "科室业务费" || pisubid == "局长基金")
        {
            if (common.IntSafeConvert(Session[pisubid]) == 1)
            {
                Session[pisubid] = 0; GetGridData();
            }
            else
            { Session[pisubid] = 1; GetGridData(pisubid); }

        }
    }
    private void GetGridData(string pisubid)
    {
        //int t = 0;
        int depid = 0;
        DataTable dtbase = new DataTable();
        DataTable dt = new DataTable();
        try
        {
            depid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
        }
        catch
        {
            return;
        }
        string YearMonth = cmbyear.SelectedItem.Value + "-" + cmbmonth.SelectedItem.Value;
        if (cmbdept.SelectedItem.Text == "科室业务费" || cmbdept.SelectedItem.Text == "局长基金")
        {
            //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text);
            //t = dtbase.Rows.Count;
            dt = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text, common.IntSafeConvert(pisubid));
            int t = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dtbase.NewRow();
                //设定行中的值
                row["piid"] = dt.Rows[i]["ChildID"];
                row["PIEcoSubName"] = "　　" + dt.Rows[i]["ChildPIEcoSubName"];
                row["totalMon"] = dt.Rows[i]["totalMon"];
                row["RPMoney"] = dt.Rows[i]["RPMoney"];
                row["RPMoney1"] = dt.Rows[i]["RPMoney1"];
                row["RPMoney2"] = dt.Rows[i]["RPMoney2"];
                dtbase.Rows.InsertAt(row, t++);
            }
            Store1.DataSource = dtbase;
            Store1.DataBind();
        }
        else
        {
            dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
           // t = dtbase.Rows.Count;
            int t = 0, z = 0;

            if (depid == 0)
            {
                //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, pisubid);

                DataTable dtbase1 = IncomeContrastpayLogic.GetAllDT(YearMonth, "科室业务费");
                DataTable dtbase2 = IncomeContrastpayLogic.GetAllDT(YearMonth, "局长基金");
                if (pisubid=="科室业务费")
                {
                    t = 4;
                }
                else if (pisubid == "局长基金")
                {
                    t = 5;
                }
                z = dtbase.Rows.Count;
                for (int i = 0; i < dtbase1.Rows.Count; i++)
                {
                    DataRow row = dtbase.NewRow();
                    //设定行中的值
                    row["piid"] = dtbase1.Rows[i]["piid"];
                    row["PIEcoSubName"] = dtbase1.Rows[i]["PIEcoSubName"];
                    row["totalMon"] = dtbase1.Rows[i]["totalMon"];
                    row["RPMoney"] = dtbase1.Rows[i]["RPMoney"];
                    row["RPMoney1"] = dtbase1.Rows[i]["RPMoney1"];
                    row["RPMoney2"] = dtbase1.Rows[i]["RPMoney2"];
                    dtbase.Rows.InsertAt(row, z++);
                }
                
                for (int i = 0; i < dtbase2.Rows.Count; i++)
                {
                    DataRow row = dtbase.NewRow();
                    //设定行中的值
                    row["piid"] = dtbase2.Rows[i]["piid"];
                    row["PIEcoSubName"] = dtbase2.Rows[i]["PIEcoSubName"];
                    row["totalMon"] = dtbase2.Rows[i]["totalMon"];
                    row["RPMoney"] = dtbase2.Rows[i]["RPMoney"];
                    row["RPMoney1"] = dtbase2.Rows[i]["RPMoney1"];
                    row["RPMoney2"] = dtbase2.Rows[i]["RPMoney2"];
                    dtbase.Rows.InsertAt(row, z++);
                }
                if (dtbase == null || dtbase.Rows.Count == 0)
                {
                    X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                    return;
                }
                if (common.IntSafeConvert(pisubid)>0)
                {
                    dt = IncomeContrastpayLogic.GetAllDT(YearMonth, depid, common.IntSafeConvert(pisubid));
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < dtbase.Rows.Count; j++)
                        {
                            if (dtbase.Rows[j]["piid"].ToString() == pisubid)
                            {
                                t = j + 1;
                            }
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dtbase.NewRow();
                            //设定行中的值
                            row["piid"] = dt.Rows[i]["ChildID"];
                            row["PIEcoSubName"] = "　　" + dt.Rows[i]["ChildPIEcoSubName"];
                            row["totalMon"] = dt.Rows[i]["totalMon"];
                            row["RPMoney"] = dt.Rows[i]["RPMoney"];
                            row["RPMoney1"] = dt.Rows[i]["RPMoney1"];
                            row["RPMoney2"] = dt.Rows[i]["RPMoney2"];
                            dtbase.Rows.InsertAt(row, t++);
                        }
                    }
                }
                else
                {
                    dt = IncomeContrastpayLogic.GetAllDT(YearMonth, pisubid, common.IntSafeConvert(pisubid));
                    //int t = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dtbase.NewRow();
                        //设定行中的值
                        row["piid"] = dt.Rows[i]["ChildID"];
                        row["PIEcoSubName"] = "　　" + dt.Rows[i]["ChildPIEcoSubName"];
                        row["totalMon"] = dt.Rows[i]["totalMon"];
                        row["RPMoney"] = dt.Rows[i]["RPMoney"];
                        row["RPMoney1"] = dt.Rows[i]["RPMoney1"];
                        row["RPMoney2"] = dt.Rows[i]["RPMoney2"];
                        dtbase.Rows.InsertAt(row, t++);
                    }
                }
                
              
            }
            else
            {
                //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
                dt = IncomeContrastpayLogic.GetAllDT(YearMonth, depid, common.IntSafeConvert(pisubid));
                if (dt == null || dt.Rows.Count == 0)
                {
                    X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                    return;
                }
                else
                { 
                    for (int j = 0; j < dtbase.Rows.Count; j++)
                    {
                        if (dtbase.Rows[j]["piid"].ToString() == pisubid)
                        {
                            t = j + 1;
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dtbase.NewRow();
                        //设定行中的值
                        row["piid"] = dt.Rows[i]["ChildID"];
                        row["PIEcoSubName"] = "　　" + dt.Rows[i]["ChildPIEcoSubName"];
                        row["totalMon"] = dt.Rows[i]["totalMon"];
                        row["RPMoney"] = dt.Rows[i]["RPMoney"];
                        row["RPMoney1"] = dt.Rows[i]["RPMoney1"];
                        row["RPMoney2"] = dt.Rows[i]["RPMoney2"];
                        dtbase.Rows.InsertAt(row, t++);
                    }
                }
            } 

        }
        Store1.DataSource = GetTotal(dtbase);
        Store1.DataBind();


    }
    public DataTable GetTotal(DataTable dt)
    {
        int depid = 0;
        DataTable dtbase = new DataTable();
        try
        {
            depid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
        }
        catch
        {
            return dt;
        }
        string YearMonth = cmbyear.SelectedItem.Value + "-" + cmbmonth.SelectedItem.Value;
        decimal totalMon = 0, MPFunding = 0, RPMoney = 0, RPMoney1 = 0, RPMoney2 = 0;
        if (cmbdept.SelectedItem.Text == "科室业务费" || cmbdept.SelectedItem.Text == "局长基金")
        {
            dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text);
        }
        else
        {
            dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
        }
        for (int i = 0; i < dtbase.Rows.Count; i++)
        {
            totalMon += ParToDecimal.ParToDel(dtbase.Rows[i]["totalMon"].ToString());
            RPMoney += ParToDecimal.ParToDel(dtbase.Rows[i]["RPMoney"].ToString());
            RPMoney1 += ParToDecimal.ParToDel(dtbase.Rows[i]["RPMoney1"].ToString());
            RPMoney2 += ParToDecimal.ParToDel(dtbase.Rows[i]["RPMoney2"].ToString());
        }
        DataRow row = dt.NewRow();
        //设定行中的值 
        row["PIEcoSubName"] = "总计";
        row["totalMon"] = totalMon;
        row["RPMoney"] = RPMoney;
        row["RPMoney1"] = RPMoney1;
        row["RPMoney2"] = RPMoney2;
        dt.Rows.InsertAt(row, dt.Rows.Count);
        return dt;
    }



}