﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Budget.BLL;
using BudgetWeb.BLL;
using Common;
using Ext.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public partial class WebPage_BudgetAnalyse_BudgetexecutionrateNew : BudgetBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DepDataBind();
        MonthDataBind();
        //string Year = "", Month = "";
        //DateTime mydate = DateTime.Now;  
        FloatUnit.Click += FloatUnit_Click;
        if (!IsPostBack && !X.IsAjaxRequest)
        {
            this.BuildTree(treepl.Root);
            X.ResourceManager.ShowWarningOnAjaxFailure = false;
            X.ResourceManager.Listeners.AjaxRequestException.Handler = "WriteError(arguments[1].errorMessage)";
        }
        //Year = Year==""?mydate.Year.ToString():Year;
        //Month = Month == "" ? mydate.Month.ToString() : Month;
        //Month = Month.Length > 1 ? Month : "0" + Month;
        //cmbmonth.SelectedItem.Value = Month ;
        //cmbyear.SelectedItem.Value = Year ;
        
       
    }
    [DirectMethod]
    public void WriteError(string msg)
    {
        //            ExceptionPolicy.HandleException(new Exception(msg), "GlobalException");
        //            Server.ClearError();
        //#if DEBUG
        //          //  Console.WriteLine(Server.GetLastError().Message);
        //#else
        //                        Response.Redirect("~/Error.aspx");

        //#endif
        //            Response.Redirect("~/Error.aspx");
    }

    private Ext.Net.NodeCollection BuildTree(Ext.Net.NodeCollection nodes)
    {
        if (nodes == null)
        {
            nodes = new Ext.Net.NodeCollection();
        }
        Ext.Net.Node root = new Ext.Net.Node();
        root.Text = "Root";
        nodes.Add(root);

      

        int depid = 0, tem = 0;
        if (common.IntSafeConvert(cmbdept.SelectedItem.Value) > 0)
        {
            depid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
        }
        string year = cmbyear.SelectedItem.Value ?? DateTime.Now.Year.ToString();
         
            string month = cmbmonth.SelectedItem.Value ?? DateTime.Now.Month.ToString();
            month = month.Length == 1 ? "0" + month : month;
            string yearMonth = year + "-" + month;

            string precmb = (common.IntSafeConvert(month) - 1).ToString();
            precmb = precmb.Length == 1 ? "0" + precmb : precmb;
            string preYearMonth = year + "-" + precmb;
            //yearMonth = "2015-02";
            //preYearMonth = "2015-01";
            //DataTable dtpiidTable = ExecuteNewLogic.GetDtPiidList(depid, yearMonth, tem);
            //DataTable predtpiidTable = ExecuteNewLogic.GetDtPiidList(depid, preYearMonth, tem);

            // DataTable dt = BG_PayIncomeLogic.GetDtPayIncomeByPIID(tem);
            DataTable dt = ExecuteNewLogic.GetDtAllPiidList(depid, yearMonth, preYearMonth);
            if (dt.Rows.Count == 0)
            {
                X.Msg.Alert("系统提示", "本月没有执行相关数据").Show();
                root.EmptyChildren = true;
                return nodes;
            }
            DataTable dtroot = dt.DefaultView.ToTable("dtroot", true, new string[] { "PIID", "ParentPIEcoSubName" });
            string piidList = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int piid = common.IntSafeConvert(dt.Rows[i]["ChildID"]);
                    piidList += piid + ",";
                }

            }
            piidList = piidList.TrimEnd(',');
            DataTable dtcashier = ExecuteNewLogic.GetCashierData(depid, yearMonth, piidList);
            DataTable pMoney = ExecuteNewLogic.GetReceiptsData(depid, preYearMonth, piidList, 0);
            DataTable pMoney1 = ExecuteNewLogic.GetReceiptsData(depid, preYearMonth, piidList, 1);
            DataTable pMoney2 = ExecuteNewLogic.GetReceiptsData(depid, preYearMonth, piidList, 2);
            DataTable rpMoney = ExecuteNewLogic.GetReceiptsData(depid, yearMonth, piidList, 0);
            DataTable rpMoney1 = ExecuteNewLogic.GetReceiptsData(depid, yearMonth, piidList, 1);
            DataTable rpMoney2 = ExecuteNewLogic.GetReceiptsData(depid, yearMonth, piidList, 2);
            DataTable totalMon = ExecuteNewLogic.GetBudgetAllocationData(depid, year, piidList);
            DataTable newTable = new DataTable();
            newTable.Columns.Add("PIID");
            newTable.Columns.Add("ChildID");
            newTable.Columns.Add("PIEcoSubName");
            newTable.Columns.Add("totalMon");
            newTable.Columns.Add("BQMon");
            newTable.Columns.Add("CashierBalance");
            newTable.Columns.Add("PMoney");
            newTable.Columns.Add("PMoney1");
            newTable.Columns.Add("PMoney2");
            newTable.Columns.Add("RpMoney");
            newTable.Columns.Add("RpMoney1");
            newTable.Columns.Add("RpMoney2");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drRow = newTable.NewRow();
                int piid = common.IntSafeConvert(dt.Rows[i]["ChildID"]);
                if (dtcashier.Rows.Count > 0)
                {
                    DataRow[] dtca = dtcashier.Select("piid=" + piid);
                    if (dtca.Length > 0)
                    {
                        drRow["BQMon"] = dtca[0]["BQMon"];
                        drRow["CashierBalance"] = dtca[0]["CashierBalance"];
                    }
                }
                if (rpMoney.Rows.Count > 0)
                {
                    DataRow[] dtrp = rpMoney.Select("piid=" + piid);
                    if (dtrp.Length > 0)
                    {
                        drRow["RpMoney"] = dtrp[0]["RpMoney"];
                    }
                }
                if (rpMoney1.Rows.Count > 0)
                {
                    DataRow[] dtrp1 = rpMoney1.Select("piid=" + piid);
                    if (dtrp1.Length > 0)
                    {
                        drRow["RpMoney1"] = dtrp1[0]["RpMoney"];
                    }
                }
                if (rpMoney2.Rows.Count > 0)
                {
                    DataRow[] dtrp2 = rpMoney2.Select("piid=" + piid);
                    if (dtrp2.Length > 0)
                    {
                        drRow["RpMoney2"] = dtrp2[0]["RpMoney"];
                    }
                }
                if (pMoney.Rows.Count > 0)
                {
                    {
                        DataRow[] dtp = pMoney.Select("piid=" + piid);
                        if (dtp.Length > 0)
                            drRow["PMoney"] = dtp[0]["RpMoney"];
                    }
                }
                if (pMoney1.Rows.Count > 0)
                {
                    DataRow[] dtp1 = pMoney1.Select("piid=" + piid);
                    if (dtp1.Length > 0)
                    {
                        drRow["PMoney1"] = dtp1[0]["RpMoney"];
                    }
                }
                if (pMoney2.Rows.Count > 0)
                {
                    DataRow[] dtp2 = pMoney2.Select("piid=" + piid);
                    if (dtp2.Length > 0)
                    {
                        drRow["PMoney2"] = dtp2[0]["RpMoney"];
                    }
                }
                if (totalMon.Rows.Count > 0)
                {
                    DataRow[] dttotalMon = totalMon.Select("piid=" + piid);
                    if (dttotalMon.Length > 0)
                    {
                        drRow["totalMon"] = dttotalMon[0]["total"];
                    }
                }
                drRow["PIEcoSubName"] = dt.Rows[i]["PIEcoSubName"].ToString();
                drRow["ChildID"] = dt.Rows[i]["ChildID"].ToString();
                drRow["PIID"] = dt.Rows[i]["PIID"].ToString();
                newTable.Rows.Add(drRow);
            }

            for (int i = 0; i < dtroot.Rows.Count; i++)
            {
                Node newNode = new Node();
                string piid = dtroot.Rows[i]["piid"].ToString();
                newNode.NodeID = piid;
                newNode.Text = dtroot.Rows[i]["ParentPIEcoSubName"].ToString();
                newNode.Icon = Icon.Folder;
                decimal BQMon = 0, CashierBalance = 0, PMoney = 0, PMoney1 = 0, PMoney2 = 0, RpMoney = 0, RpMoney1 = 0, RpMoney2 = 0, total = 0;
                DataRow[] drrowsleaf = newTable.Select("PIID=" + piid);
                if (drrowsleaf.Length > 0)
                {
                    for (int j = 0; j < drrowsleaf.Length; j++)
                    {
                        Node nodenew = new Node();
                        nodenew.NodeID = drrowsleaf[j]["ChildID"].ToString();
                        nodenew.Text = drrowsleaf[j]["PIEcoSubName"].ToString();
                        //nodenew.CustomAttributes.Add(new ConfigItem("PIID",drRows[j]["PIEcoSubName"].ToString(), ParameterMode.Value));
                        //nodenew.CustomAttributes.Add(new ConfigItem("PIEcoSubName",drRows[j]["PIEcoSubName"].ToString(), ParameterMode.Value)); 
                        nodenew.CustomAttributes.Add(new ConfigItem("BQMon", drrowsleaf[j]["BQMon"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("CashierBalance", drrowsleaf[j]["CashierBalance"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("PMoney", drrowsleaf[j]["PMoney"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("PMoney1 ", drrowsleaf[j]["PMoney1"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("PMoney2 ", drrowsleaf[j]["PMoney2"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("RpMoney ", drrowsleaf[j]["RpMoney"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("RpMoney1 ", drrowsleaf[j]["RpMoney1"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("RpMoney2 ", drrowsleaf[j]["RpMoney2"].ToString(), ParameterMode.Value)); nodenew.CustomAttributes.Add(new ConfigItem("totalMon ", drrowsleaf[j]["totalMon"].ToString(), ParameterMode.Value));
                        nodenew.Leaf = true;
                        nodenew.Icon = Icon.Anchor;
                        newNode.Children.Add(nodenew);
                        BQMon += ParToDecimal.ParToDel(drrowsleaf[j]["BQMon"].ToString());
                        CashierBalance += ParToDecimal.ParToDel(drrowsleaf[j]["CashierBalance"].ToString());
                        PMoney += ParToDecimal.ParToDel(drrowsleaf[j]["PMoney"].ToString());
                        PMoney1 += ParToDecimal.ParToDel(drrowsleaf[j]["PMoney1"].ToString());
                        PMoney2 += ParToDecimal.ParToDel(drrowsleaf[j]["PMoney2"].ToString());
                        RpMoney += ParToDecimal.ParToDel(drrowsleaf[j]["RpMoney"].ToString());
                        RpMoney1 += ParToDecimal.ParToDel(drrowsleaf[j]["RpMoney1"].ToString());
                        RpMoney2 += ParToDecimal.ParToDel(drrowsleaf[j]["RpMoney2"].ToString());
                        total += ParToDecimal.ParToDel(drrowsleaf[j]["totalMon"].ToString());
                    }
                }
                else
                {
                    newNode.EmptyChildren = true;
                }
                newNode.CustomAttributes.Add(new ConfigItem("BQMon", BQMon.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("CashierBalance", CashierBalance.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("PMoney", PMoney.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("PMoney1 ", PMoney1.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("PMoney2 ", PMoney2.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("RpMoney ", RpMoney.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("RpMoney1 ", RpMoney1.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("RpMoney2 ", RpMoney2.ToString(), ParameterMode.Value));
                newNode.CustomAttributes.Add(new ConfigItem("totalMon ", total.ToString(), ParameterMode.Value));
                root.Children.Add(newNode);
            } 
        
        return nodes;
    }

    [DirectMethod]
    public string RefreshMenu()
    {
        Ext.Net.NodeCollection nodes = this.BuildTree(null);

        return nodes.ToJson();
    }

    void FloatUnit_Click(object sender, EventArgs e)
    {
        Response.Redirect("BudgetexecutionrateNew.aspx");
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
 

 
}