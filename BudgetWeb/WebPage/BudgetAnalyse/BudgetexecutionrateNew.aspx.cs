using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
        int Year = 0, Month = 0;
        DateTime mydate = DateTime.Now;
        Year = mydate.Year;
        Month = mydate.Month;
        FloatUnit.Click += FloatUnit_Click;
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
    protected void btnsend_DirectClick(object sender, DirectEventArgs e)
    {
        
    }



    #region 经济科目树.

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
        //if (NodeID == "root")
        //{
        //    rootNode.Text = "全部";
        //    rootNode.NodeID = "全部";
        //    rootNode.Icon = Icon.Folder;
        //    nodes.Add(rootNode);
        //    rootNode.Expanded = true;
        //}
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
        if (NodeID == "root")
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
    #endregion
}