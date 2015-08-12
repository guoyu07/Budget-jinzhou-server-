using System;
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

public partial class WebPage_BudgetAnalyse_BudgetexecutionrateNewyear : BudgetBasePage
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

        
            DataTable dt = ExecuteNewLogic.GetDtAllPiidListyear(depid, year);
            if (dt.Rows.Count==0)
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
            DataTable dtcashier = ExecuteNewLogic.GetCashierDatayear(depid, year, piidList);
            DataTable RpMoney = ExecuteNewLogic.GetReceiptsDatayear(depid, year, piidList, 2); 
            DataTable totalMon = ExecuteNewLogic.GetBudgetAllocationData(depid, year, piidList);
            DataTable newTable = new DataTable();
            newTable.Columns.Add("PIID");
            newTable.Columns.Add("ChildID");
            newTable.Columns.Add("PIEcoSubName");
            newTable.Columns.Add("totalMon");
            newTable.Columns.Add("BQMon"); 
            newTable.Columns.Add("RpMoney");
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
                        //drRow["CashierBalance"] = dtca[0]["CashierBalance"];
                    }
                } 
                if (RpMoney.Rows.Count > 0)
                {
                    DataRow[] dtp2 = RpMoney.Select("piid=" + piid);
                    if (dtp2.Length > 0)
                    {
                        drRow["RpMoney"] = dtp2[0]["RpMoney"];
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
               // newNode.Leaf = true;
                decimal BQMond = 0, RpMoneyd = 0, totalMond = 0;
                DataRow[] drrowsleaf = newTable.Select("PIID=" + piid);
                if (drrowsleaf.Length > 0)
                {
                    NodeCollection nc=new NodeCollection();
                    for (int j = 0; j < drrowsleaf.Length; j++)
                    {
                        Node nodenew = new Node();
                        nodenew.NodeID = drrowsleaf[j]["ChildID"].ToString();
                        nodenew.Text = drrowsleaf[j]["PIEcoSubName"].ToString();
                        //nodenew.CustomAttributes.Add(new ConfigItem("PIID",drRows[j]["PIEcoSubName"].ToString(), ParameterMode.Value));
                        //nodenew.CustomAttributes.Add(new ConfigItem("PIEcoSubName",drRows[j]["PIEcoSubName"].ToString(), ParameterMode.Value));
                        nodenew.CustomAttributes.Add(new ConfigItem("BQMon", drrowsleaf[j]["BQMon"].ToString(), ParameterMode.Value)); 
                        nodenew.CustomAttributes.Add(new ConfigItem("RpMoney ", drrowsleaf[j]["RpMoney"].ToString(), ParameterMode.Value)); 
                        nodenew.CustomAttributes.Add(new ConfigItem("totalMon ", drrowsleaf[j]["totalMon"].ToString(), ParameterMode.Value));
                        nodenew.Leaf = true;
                        nodenew.Icon = Icon.Anchor;
                        nc.Add(nodenew);
                        
                        BQMond += ParToDecimal.ParToDel(drrowsleaf[j]["BQMon"].ToString());
                        RpMoneyd += ParToDecimal.ParToDel(drrowsleaf[j]["RpMoney"].ToString());
                        totalMond += ParToDecimal.ParToDel(drrowsleaf[j]["totalMon"].ToString());
                    }
                    newNode.CustomAttributes.Add(new ConfigItem("BQMon", BQMond.ToString(), ParameterMode.Value));
                    newNode.CustomAttributes.Add(new ConfigItem("RpMoney ", RpMoneyd.ToString(), ParameterMode.Value));
                    newNode.CustomAttributes.Add(new ConfigItem("totalMon ", totalMond.ToString(), ParameterMode.Value));
                   
                    root.Children.Add(newNode);
                    newNode.Children.AddRange(nc);
                }
                else
                {
                    newNode.EmptyChildren = true;
                }
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