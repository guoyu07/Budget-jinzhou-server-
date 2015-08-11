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


        if (tabcontrol.ActiveTab.ID == "Tab1")
        {
            IRow headrow = sheet.CreateRow(0);
            ICell cell = headrow.CreateCell(0);
            cell.SetCellValue("经济科目");
            cell.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, 0));
            ICell cell1 = headrow.CreateCell(1);
            cell1.SetCellValue("年初经费(元)");
            cell1.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 1, 1));

            ICell celln2 = headrow.CreateCell(2);
            celln2.SetCellValue("当月可用计划");
            celln2.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 2, 6));

            ICell cell2 = headrow.CreateCell(7);
            cell2.SetCellValue("预算执行");
            cell2.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 7, 9));
            ICell cell3 = headrow.CreateCell(10);
            cell3.SetCellValue("预算结余");
            cell3.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 10, 12));
            ICell cell4 = headrow.CreateCell(13);
            cell4.SetCellValue("预算执行率");
            cell4.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 13, 15));




            IRow headrow1 = sheet.CreateRow(1);
            ICell cell5 = headrow1.CreateCell(7);
            cell5.SetCellValue("申请数(元)");
            cell5.CellStyle = style;
            ICell cell6 = headrow1.CreateCell(8);
            cell6.SetCellValue("已审核(元)");
            cell6.CellStyle = style;
            ICell cell7 = headrow1.CreateCell(9);
            cell7.SetCellValue("已支付(元)");
            cell7.CellStyle = style;
            ICell cell8 = headrow1.CreateCell(10);
            cell8.SetCellValue("申请数(元)");
            cell8.CellStyle = style;
            ICell cell9 = headrow1.CreateCell(11);
            cell9.SetCellValue("已审核(元)");
            cell9.CellStyle = style;
            ICell cell10 = headrow1.CreateCell(12);
            cell10.SetCellValue("已支付(元)");
            cell10.CellStyle = style;
            ICell cell11 = headrow1.CreateCell(13);
            cell11.SetCellValue("申请数");
            cell11.CellStyle = style;
            ICell cell12 = headrow1.CreateCell(14);
            cell12.SetCellValue("已审核");
            cell12.CellStyle = style;
            ICell cell13 = headrow1.CreateCell(15);
            cell13.SetCellValue("已支付");
            cell13.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 7, 7));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 8, 8));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 9, 9));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 10, 10));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 11, 11));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 12, 12));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 13, 13));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 14, 14));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 15, 15));


            ICell celln3 = headrow1.CreateCell(2);
            celln3.SetCellValue("小计(元)");
            celln3.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 2, 2));

            ICell celln4 = headrow1.CreateCell(3);
            celln4.SetCellValue("上月余额");
            celln4.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 3, 5));

            ICell celln5 = headrow1.CreateCell(6);
            celln5.SetCellValue("本月申请(元)");
            celln5.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 6, 6));

            IRow headrow2 = sheet.CreateRow(2);

            ICell cellnn3 = headrow2.CreateCell(3);
            cellnn3.SetCellValue("申请数(元)");
            cellnn3.CellStyle = style;

            ICell cellnn4 = headrow2.CreateCell(4);
            cellnn4.SetCellValue("已审核数(元)");
            cellnn4.CellStyle = style;


            ICell cellnn5 = headrow2.CreateCell(5);
            cellnn5.SetCellValue("已支付数(元)");
            cellnn5.CellStyle = style;
        }
        else
        {
            IRow headrow = sheet.CreateRow(0);
            ICell cell = headrow.CreateCell(0);
            cell.SetCellValue("经济科目");
            cell.CellStyle = style;
            // sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
            ICell cell1 = headrow.CreateCell(1);
            cell1.SetCellValue("年初经费(元)");
            cell1.CellStyle = style;
            // sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1));
            ICell cell2 = headrow.CreateCell(2);
            cell2.SetCellValue("申请计划数(元)");
            cell2.CellStyle = style;
            //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 2, 4));
            ICell cell3 = headrow.CreateCell(3);
            cell3.SetCellValue("报销执行金额(元)");
            cell3.CellStyle = style;
            // sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 5, 7));
            ICell cell4 = headrow.CreateCell(4);
            cell4.SetCellValue("余额");
            cell4.CellStyle = style;
            ICell cell5 = headrow.CreateCell(6);
            cell5.SetCellValue("执行率");
            cell5.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 3, 3));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 4, 5));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 6, 7));


            IRow headrow1 = sheet.CreateRow(1);
            ICell cell6 = headrow1.CreateCell(4);
            cell6.SetCellValue("计划(元)");
            cell6.CellStyle = style;

            ICell cell7 = headrow1.CreateCell(5);
            cell7.SetCellValue("执行(元)");
            cell7.CellStyle = style;

            ICell cell8 = headrow1.CreateCell(6);
            cell8.SetCellValue("计划");
            cell8.CellStyle = style;

            ICell cell9 = headrow1.CreateCell(7);
            cell9.SetCellValue("执行");
            cell9.CellStyle = style;
        }
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
        if (tabcontrol.ActiveTab.ID == "Tab1")
        {
            GetGridData();
        }
        else
        {
            GetYearData();
        }

    }

    private void GetYearData()
    {
        int depid = 0;
        int year = common.IntSafeConvert(cmbyear.SelectedItem.Value);
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
            dt = IncomeContrastpayLogic.GetAllDT(year, cmbdept.SelectedItem.Text);
            if (dt == null || dt.Rows.Count == 0)
            {
                X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                return;
            }
            Store2.DataSource = dt;
            Store2.DataBind();
        }
        else
        {
            dt = IncomeContrastpayLogic.GetAllDT(year, depid);

            if (depid == 0)
            {

                DataTable dtbase1 = IncomeContrastpayLogic.GetAllDT(year, "科室业务费");
                DataTable dtbase2 = IncomeContrastpayLogic.GetAllDT(year, "局长基金");
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("piid");
                    dt.Columns.Add("PIEcoSubName");
                    dt.Columns.Add("totalMon");
                    dt.Columns.Add("RPMoney");
                    dt.Columns.Add("RPMoney1");
                    dt.Columns.Add("RPMoney2");
                }
                int t = common.IntSafeConvert(dt.Rows.Count);
                if (dtbase1 != null && dtbase1.Rows.Count > 0)
                {
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
                }

                t = common.IntSafeConvert(dt.Rows.Count);
                if (dtbase2 != null && dtbase2.Rows.Count > 0)
                {
                    for (int i = 0; i < dtbase2.Rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        //设定行中的值
                        row["piid"] = dtbase2.Rows[i]["piid"];
                        row["PIEcoSubName"] = dtbase2.Rows[i]["PIEcoSubName"];
                        row["totalMon"] = dtbase2.Rows[i]["totalMon"];
                        //row["MPFunding"] = dtbase2.Rows[i]["MPFunding"];
                        row["RPMoney"] = dtbase2.Rows[i]["RPMoney"];
                        row["RPMoney1"] = dtbase2.Rows[i]["RPMoney1"];
                        row["RPMoney2"] = dtbase2.Rows[i]["RPMoney2"];
                        dt.Rows.InsertAt(row, t++);
                    }
                }

            }
            if (dt == null || dt.Rows.Count == 0)
            {
                X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                Store2.DataSource = dt;
                Store2.DataBind();
                //return;
            }
            else
                Store2.DataSource = GetTotal(dt);
            Store2.DataBind();

        }
    }
    private void GetYearData(string pisubid)
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
        int year = common.IntSafeConvert(cmbyear.SelectedItem.Value);
        if (cmbdept.SelectedItem.Text == "科室业务费" || cmbdept.SelectedItem.Text == "局长基金")
        {
            //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text);
            //t = dtbase.Rows.Count;
            dt = IncomeContrastpayLogic.GetAllDT(year, cmbdept.SelectedItem.Text, common.IntSafeConvert(pisubid));
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
            Store2.DataSource = dtbase;
            Store2.DataBind();
        }
        else
        {
            dtbase = IncomeContrastpayLogic.GetAllDT(year, depid);
            // t = dtbase.Rows.Count;
            int t = 0, z = 0;

            if (depid == 0)
            {
                //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, pisubid);

                DataTable dtbase1 = IncomeContrastpayLogic.GetAllDT(year, "科室业务费");
                DataTable dtbase2 = IncomeContrastpayLogic.GetAllDT(year, "局长基金");
                if (pisubid == "科室业务费")
                {
                    t = 4;
                }
                else if (pisubid == "局长基金")
                {
                    t = 5;
                }
                z = common.IntSafeConvert(dtbase.Rows.Count);
                if (dtbase1 != null && dtbase1.Rows.Count > 0)
                {
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
                }

                if (dtbase2 != null && dtbase2.Rows.Count > 0)
                {
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
                }

                if (dtbase == null || dtbase.Rows.Count == 0)
                {
                    X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                    return;
                }
                if (common.IntSafeConvert(pisubid) > 0)
                {
                    dt = IncomeContrastpayLogic.GetAllDT(year, depid, common.IntSafeConvert(pisubid));
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
                    dt = IncomeContrastpayLogic.GetAllDT(year, pisubid, common.IntSafeConvert(pisubid));
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
                dt = IncomeContrastpayLogic.GetAllDT(year, depid, common.IntSafeConvert(pisubid));
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
        Store2.DataSource = GetTotal(dtbase);
        Store2.DataBind();


    }

    private void GetGridData()
    {
        int depid = 0;
        string YearMonth = cmbyear.SelectedItem.Value + "-" + cmbmonth.SelectedItem.Value;
        int precmb = common.IntSafeConvert(cmbmonth.SelectedItem.Value) - 1;
        string preYearMonth = cmbyear.SelectedItem.Value + "-" + (precmb < 10 ? "0" + precmb : precmb.ToString());
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
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("piid");
                    dt.Columns.Add("PIEcoSubName");
                    dt.Columns.Add("totalMon");
                    dt.Columns.Add("RPMoney");
                    dt.Columns.Add("RPMoney1");
                    dt.Columns.Add("RPMoney2");
                    dt.Columns.Add("PMoney");
                    dt.Columns.Add("PMoney1");
                    dt.Columns.Add("PMoney2");
                    dt.Columns.Add("BQMon");
                    dt.Columns.Add("CashierBalance");
                }
                else
                {
                    dt.Columns.Add("PMoney");
                    dt.Columns.Add("PMoney1");
                    dt.Columns.Add("PMoney2");
                    dt.Columns.Add("BQMon");
                    dt.Columns.Add("CashierBalance");
                }
                int t = common.IntSafeConvert(dt.Rows.Count);
                if (dtbase1 != null && dtbase1.Rows.Count > 0)
                {
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
                }

                t = common.IntSafeConvert(dt.Rows.Count);
                if (dtbase2 != null && dtbase2.Rows.Count > 0)
                {
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

            }
            if (dt == null || dt.Rows.Count == 0)
            {

                if (precmb > 0)
                {
                    DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid);
                    if (dtpre != null && dtpre.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtpre.Rows.Count; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["piid"] = dtpre.Rows[i]["piid"];
                            row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                            // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                            row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                            row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                            row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                            row["BQMon"] = dtpre.Rows[i]["BQMon"];
                            row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                            dt.Rows.Add(row);
                        }
                    }

                }
                else
                {
                    X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                }

                Store1.DataSource = dt;
                Store1.DataBind();
                //return;
            }
            else
            {
                DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid);
                //DataTable dtpresum = IncomeContrastpayLogic.GetAllDT(YearMonth, depid,piid);
                if (dtpre != null && dtpre.Rows.Count > 0)
                {
                    List<string> liststr = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow[] dtlist = dtpre.Select("piid=" + dt.Rows[i]["piid"].ToString());
                        if (dtlist.Length > 0)
                        {
                            dt.Rows[i]["PMoney"] = dtlist[0]["RPMoney"];
                            dt.Rows[i]["PMoney1"] = dtlist[0]["RPMoney1"];
                            dt.Rows[i]["PMoney2"] = dtlist[0]["RPMoney2"];
                            dt.Rows[i]["BQMon"] = dtlist[0]["BQMon"];
                            dt.Rows[i]["CashierBalance"] = dtlist[0]["CashierBalance"];
                        }
                        liststr.Add(dt.Rows[i]["piid"].ToString());
                    }
                    for (int i = 0; i < dtpre.Rows.Count; i++)
                    {
                        if (!liststr.Contains(dtpre.Rows[i]["piid"]))
                        {
                            DataRow row = dt.NewRow();
                            row["piid"] = dtpre.Rows[i]["piid"];
                            row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                            // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                            row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                            row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                            row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                            row["BQMon"] = dtpre.Rows[i]["BQMon"];
                            row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                            dt.Rows.Add(row);
                        }
                    }

                }

                Store1.DataSource = GetTotal(dt);
                Store1.DataBind();

            }

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
                if (tabcontrol.ActiveTab.ID == "Tab1")
                {
                    GetGridData();
                }
                else
                {
                    GetYearData();
                }
            }
            else
            {
                if (tabcontrol.ActiveTab.ID == "Tab1")
                {
                    GetGridData(pisubid);
                }
                else
                {
                    GetYearData(pisubid);
                }
            }
        }
        else if (pisubid == "科室业务费" || pisubid == "局长基金")
        {
            if (common.IntSafeConvert(Session[pisubid]) == 1)
            {
                Session[pisubid] = 0;
                if (tabcontrol.ActiveTab.ID == "Tab1")
                {
                    GetGridData();
                }
                else
                {
                    GetYearData();
                }
            }
            else
            {
                Session[pisubid] = 1; if (tabcontrol.ActiveTab.ID == "Tab1")
                {
                    GetGridData(pisubid);
                }
                else
                {
                    GetYearData(pisubid);
                }
            }

        }
    }
    private void GetGridData(string pisubid)
    {
        //int t = 0;
        int depid = 0;
        DataTable dtbase = new DataTable();
        DataTable dt = new DataTable();
        //dt = new DataTable();
        //dt.Columns.Add("piid");
        //dt.Columns.Add("PIEcoSubName");
        //dt.Columns.Add("totalMon");
        //dt.Columns.Add("RPMoney");
        //dt.Columns.Add("RPMoney1");
        //dt.Columns.Add("RPMoney2");
        //dt.Columns.Add("PMoney");
        //dt.Columns.Add("PMoney1");
        //dt.Columns.Add("PMoney2");
        //dt.Columns.Add("BQMon");
        //dt.Columns.Add("CashierBalance");
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
            int precmb = common.IntSafeConvert(cmbmonth.SelectedItem.Value) - 1;
            string preYearMonth = cmbyear.SelectedItem.Value + "-" + (precmb < 10 ? "0" + precmb : precmb.ToString());
            dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
            // t = dtbase.Rows.Count;
            int t = 0, z = 0;

            if (depid == 0)
            {
                //dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, pisubid);

                DataTable dtbase1 = IncomeContrastpayLogic.GetAllDT(YearMonth, "科室业务费");
                DataTable dtbase2 = IncomeContrastpayLogic.GetAllDT(YearMonth, "局长基金");
                if (pisubid == "科室业务费")
                {
                    t = 4;
                }
                else if (pisubid == "局长基金")
                {
                    t = 5;
                }
                z = common.IntSafeConvert(dtbase.Rows.Count);
                if (dtbase1 != null && dtbase1.Rows.Count > 0)
                {
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
                }

                if (dtbase2 != null && dtbase2.Rows.Count > 0)
                {
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
                }

                if (dtbase == null || dtbase.Rows.Count == 0)
                {
                    dt.Columns.Add("PMoney");
                    dt.Columns.Add("PMoney1");
                    dt.Columns.Add("PMoney2");
                    dt.Columns.Add("BQMon");
                    dt.Columns.Add("CashierBalance");
                    if (precmb > 0)
                    {
                        DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid);
                        if (dtpre != null && dtpre.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtpre.Rows.Count; i++)
                            {
                                DataRow row = dtbase.NewRow();
                                row["piid"] = dtpre.Rows[i]["piid"];
                                row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                                // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                                row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                                row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                                row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                                row["BQMon"] = dtpre.Rows[i]["BQMon"];
                                row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                                dtbase.Rows.Add(row);
                            }
                        }

                    }
                    else
                    {
                        X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                    }
                }
                if (common.IntSafeConvert(pisubid) > 0)
                {
                    dt = IncomeContrastpayLogic.GetAllDT(YearMonth, depid, common.IntSafeConvert(pisubid));
                    dt.Columns.Add("PMoney");
                    dt.Columns.Add("PMoney1");
                    dt.Columns.Add("PMoney2");
                    dt.Columns.Add("BQMon");
                    dt.Columns.Add("CashierBalance");
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        if (precmb > 0)
                        {
                            DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid, common.IntSafeConvert(pisubid));
                            if (dtpre != null && dtpre.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtpre.Rows.Count; i++)
                                {
                                    DataRow row = dt.NewRow();
                                    row["piid"] = dtpre.Rows[i]["piid"];
                                    row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                                    // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                                    row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                                    row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                                    row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                                    row["BQMon"] = dtpre.Rows[i]["BQMon"];
                                    row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                                    dt.Rows.Add(row);
                                }
                            }

                        }
                        else
                        {
                            X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                        }
                    }
                    else
                    {
                        DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid, common.IntSafeConvert(pisubid));
                        //DataTable dtpresum = IncomeContrastpayLogic.GetAllDT(YearMonth, depid,piid);
                        if (dtpre != null && dtpre.Rows.Count > 0)
                        {
                            List<string> liststr = new List<string>();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow[] dtlist = dtpre.Select("piid=" + dt.Rows[i]["piid"].ToString());
                                if (dtlist.Length > 0)
                                {
                                    dt.Rows[i]["PMoney"] = dtlist[0]["RPMoney"];
                                    dt.Rows[i]["PMoney1"] = dtlist[0]["RPMoney1"];
                                    dt.Rows[i]["PMoney2"] = dtlist[0]["RPMoney2"];
                                    dt.Rows[i]["BQMon"] = dtlist[0]["BQMon"];
                                    dt.Rows[i]["CashierBalance"] = dtlist[0]["CashierBalance"];
                                }
                                liststr.Add(dt.Rows[i]["piid"].ToString());
                            }
                            for (int i = 0; i < dtpre.Rows.Count; i++)
                            {
                                if (!liststr.Contains(dtpre.Rows[i]["piid"]))
                                {
                                    DataRow row = dt.NewRow();
                                    row["piid"] = dtpre.Rows[i]["piid"];
                                    row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                                    // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                                    row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                                    row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                                    row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                                    row["BQMon"] = dtpre.Rows[i]["BQMon"];
                                    row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                                    dt.Rows.Add(row);
                                }
                            }

                        }

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
                dt.Columns.Add("PMoney");
                dt.Columns.Add("PMoney1");
                dt.Columns.Add("PMoney2");
                dt.Columns.Add("BQMon");
                dt.Columns.Add("CashierBalance");
                if (dt == null || dt.Rows.Count == 0)
                {
                    if (precmb > 0)
                    {
                        DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid, common.IntSafeConvert(pisubid));
                        if (dtpre != null && dtpre.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtpre.Rows.Count; i++)
                            {
                                DataRow row = dt.NewRow();
                                row["piid"] = dtpre.Rows[i]["piid"];
                                row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                                // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                                row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                                row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                                row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                                row["BQMon"] = dtpre.Rows[i]["BQMon"];
                                row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                                dt.Rows.Add(row);
                            }
                        }

                    }
                    else
                    {
                        X.Msg.Alert("系统提示", "您所查询的数据为空。").Show();
                    }
                }
                else
                {
                    DataTable dtpre = IncomeContrastpayLogic.GetAllDTpre(preYearMonth, depid, common.IntSafeConvert(pisubid));
                    //DataTable dtpresum = IncomeContrastpayLogic.GetAllDT(YearMonth, depid,piid);
                    if (dtpre != null && dtpre.Rows.Count > 0)
                    {
                        List<string> liststr = new List<string>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow[] dtlist = dtpre.Select("piid=" + dt.Rows[i]["piid"].ToString());
                            if (dtlist.Length > 0)
                            {
                                dt.Rows[i]["PMoney"] = dtlist[0]["RPMoney"];
                                dt.Rows[i]["PMoney1"] = dtlist[0]["RPMoney1"];
                                dt.Rows[i]["PMoney2"] = dtlist[0]["RPMoney2"];
                                dt.Rows[i]["BQMon"] = dtlist[0]["BQMon"];
                                dt.Rows[i]["CashierBalance"] = dtlist[0]["CashierBalance"];
                            }
                            liststr.Add(dt.Rows[i]["piid"].ToString());
                        }
                        for (int i = 0; i < dtpre.Rows.Count; i++)
                        {
                            if (!liststr.Contains(dtpre.Rows[i]["piid"]))
                            {
                                DataRow row = dt.NewRow();
                                row["piid"] = dtpre.Rows[i]["piid"];
                                row["PIEcoSubName"] = dtpre.Rows[i]["PIEcoSubName"];
                                // row["totalMon"] = dtpre.Rows[i]["totalMon"];
                                row["PMoney"] = dtpre.Rows[i]["RPMoney"];
                                row["PMoney1"] = dtpre.Rows[i]["RPMoney1"];
                                row["PMoney2"] = dtpre.Rows[i]["RPMoney2"];
                                row["BQMon"] = dtpre.Rows[i]["BQMon"];
                                row["CashierBalance"] = dtpre.Rows[i]["CashierBalance"];
                                dt.Rows.Add(row);
                            }
                        }

                    }

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
        //int depid = 0;
        //DataTable dtbase = new DataTable();
        //try
        //{
        //    depid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
        //}
        //catch
        //{
        //    return dt;
        //}
        //string YearMonth = cmbyear.SelectedItem.Value + "-" + cmbmonth.SelectedItem.Value;
        decimal totalMon = 0, MPFunding = 0, RPMoney = 0, RPMoney1 = 0, RPMoney2 = 0;
        //if (cmbdept.SelectedItem.Text == "科室业务费" || cmbdept.SelectedItem.Text == "局长基金")
        //{
        //    dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, cmbdept.SelectedItem.Text);
        //}
        //else
        //{
        //    dtbase = IncomeContrastpayLogic.GetAllDT(YearMonth, depid);
        //}
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!dt.Rows[i]["PIEcoSubName"].ToString().Contains("　　"))
                {
                    MPFunding += ParToDecimal.ParToDel(dt.Rows[i]["MPFunding"].ToString());
                    totalMon += ParToDecimal.ParToDel(dt.Rows[i]["totalMon"].ToString());
                    RPMoney += ParToDecimal.ParToDel(dt.Rows[i]["RPMoney"].ToString());
                    RPMoney1 += ParToDecimal.ParToDel(dt.Rows[i]["RPMoney1"].ToString());
                    RPMoney2 += ParToDecimal.ParToDel(dt.Rows[i]["RPMoney2"].ToString());
                }

            }

        }


        DataRow row = dt.NewRow();
        //设定行中的值 
        row["PIEcoSubName"] = "总计";
        row["totalMon"] = totalMon;
        row["RPMoney"] = RPMoney;
        row["MPFunding"] = MPFunding;
        row["RPMoney1"] = RPMoney1;
        row["RPMoney2"] = RPMoney2;
        dt.Rows.InsertAt(row, dt.Rows.Count);
        return dt;
    }


}