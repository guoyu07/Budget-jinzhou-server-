using BudgetWeb.BLL;
using BudgetWeb.Model;
using Common;
using Ext.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

public partial class WebPage_BudgetEdit_BudgetMonEdit : BudgetBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserLimStr == "审核员")
        {
            GridPanel1.Hide();
        }
        DepDataBind();
        MonthDataBind();
        int Year = 0, Month = 0;
        DateTime mydate = DateTime.Now;
        Year = mydate.Year;
        Month = mydate.Month;
        //if (Request.QueryString["Year"] != null && Request.QueryString["cmbdeptid"] != null)
        //{
        //    cmbyear.SelectedItem.Text = Request.QueryString["Year"].ToString();
        //    cmbdept.SelectedItem.Text = Request.QueryString["cmbdeptid"].ToString();
        //    GetGridData();
        //}
    }
    private GridPanel BuildGridPanel(DataTable dt)
    {
        GridPanel gridpl = new GridPanel()
        {
            ID = "gridpl",
            Region = Region.Center,
            Title = "预算金额编辑",
            ColumnLines = true,
            AutoScroll = true,
            Border = false,
            Store =  
            {
                this.BuildStore(dt)
            },
            SelectionModel = 
            { 
                new RowSelectionModel() { Mode = SelectionMode.Single }
            },

            View =
            {
               new Ext.Net.GridView()
               {
                    StripeRows = true,
                    TrackOver = true 
               }
            }
        };

        List<Ext.Net.ListItem> listvalue = cmbdept.SelectedItems.ToList();
        int listcout = 0;
        if (listvalue.Count == 1)
        {
            listcout = 1;
        }
        else { listcout = listvalue.Count + 1; }
        for (int i = 0; i < listcout; i++)
        {
            //Column call = new Column(); 
            if (i == 0)
            {
                Column c1 = new Column() { Text = cjname.Text, DataIndex = cjname.DataIndex, Width = 280, Sortable = false, Locked = true, MinWidth = 280, MaxWidth = 280 };
                //call.Columns.Add(c1);
                gridpl.ColumnModel.Add(c1);
            }
            Column co = new Column();
            if (listcout > 1 && i == 0)
            {
                co.Text = "预算支出";
            }
            else if (listcout == 1 && i == 0)
            {
                co.Text = listvalue[i].Text;
            }
            else { co.Text = listvalue[i - 1].Text; }
            for (int j = 0; j < columndepname.Columns.Count; j++)
            {
                string colm = columndepname.Columns[j].DataIndex;
                if (i > 0)
                {
                    colm = columndepname.Columns[j].DataIndex + i;
                }
                Column coleaf = new Column() { Renderer = columndepname.Columns[j].Text.Contains("（元）") ? new Renderer("return value/10000") : null, Text = columndepname.Columns[j].Text.Contains("（元）") ? columndepname.Columns[j].Text.Replace("（元）", "（万元）") : columndepname.Columns[j].Text, DataIndex = colm, Width = 100 };
                string tmp = i == 0 ? "" : i.ToString();
                if (i > 0)
                {
                    tmp = i.ToString();
                }
                string handler = string.Format("return (record.data.BGAMMon{0}-record.data.BGAMLastMon{0})", tmp);
                string handler1 = "if((record.data.BGAMMon{0}-record.data.BGAMLastMon{0})===0||(record.data.BGAMMon{0}-record.data.BGAMLastMon{0})===null){{return '0%'}} else if((record.data.BGAMLastMon{0})===0||(record.data.BGAMLastMon{0})===null){{return '0%'}} else {{return  ((record.data.BGAMMon{0}-record.data.BGAMLastMon{0})*100/record.data.BGAMLastMon{0}).toFixed(2)+'%'}}";
                handler1 = string.Format(handler1, tmp);
                //string handler1 = string.Format(@"if((record.data.BGAMMon{0}-record.data.BGAMLastMon{0})===0||(record.data.BGAMMon{0}-record.data.BGAMLastMon{0})===null){return '0%'} else if((record.data.BGAMLastMon{0})===0||(record.data.BGAMLastMon{0})===null){return '0%'} else {return  ((record.data.BGAMMon{0}-record.data.BGAMLastMon{0})*100/record.data.BGAMLastMon{0}).toFixed(2)+'%'}", i);
                Renderer rd = new Renderer(handler);
                Renderer rd1 = new Renderer(handler1);
                if (coleaf.Text == "同比+")
                {
                    coleaf.Renderer = rd;
                }
                else if (coleaf.Text == "同比+%")
                {
                    coleaf.Renderer = rd1;
                }
                co.Columns.Add(coleaf);
            }
            if (co.Text == "预算支出")
            {
                Column cochild = new Column() { Text = "总计（万元）", Width = 100 };
                string handler2 = string.Format("return (record.data.BGAMIncome-record.data.BGAMMon)");
                Renderer rd2 = new Renderer(handler2);
                cochild.Renderer = rd2;
                co.Columns.Add(cochild);
            }

            //call.Columns.Add(co);
            gridpl.ColumnModel.Add(co);
        }

        GridHead.Text = GridHead.Text.TrimEnd('*');
        //for (int i = 0; i < listvalue.Count; i++)
        //{
        //    for (int j = 0; j < GridPanel1.ColumnModel.Columns.Count; j++)
        //    {
        //        if (i != 1)
        //        {
        //            j += 1;
        //        } 
        //        Column co = new Column() { Text = GridPanel1.ColumnModel.Columns[j].Text, DataIndex = GridPanel1.ColumnModel.Columns[j].DataIndex + i };
        //        gridpl.ColumnModel.Columns.Add(co);
        //    }
        //}
        return gridpl;
    }
    [DirectMethod]
    public void DoBind()
    {
        DepDataBind();
    }
    private Store BuildStore(DataTable dt)
    {
        Store store = new Store();
        Model md = new Model();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ModelField mdf = new ModelField(dt.Columns[i].ToString());
            md.Fields.Add(mdf);
        }
        store.Model.Add(md);
        store.DataSource = dt;

        return store;
    }

    private void GetGridData()
    {
        int deptid = DepID;
        int year = common.IntSafeConvert(cmbyear.SelectedItem.Value);
        if (UserLimStr == "审核员")
        {
            List<Ext.Net.ListItem> listvalue = cmbdept.SelectedItems.ToList();
            List<int> listt = new List<int>();
            for (int i = 0; i < listvalue.Count; i++)
            {
                listt.Add(common.IntSafeConvert(listvalue[i].Value));
            }
            DataTable dt = BG_CaliberLogic.GetAllBG_CaliberMon(listt, year);
            DataTable dtnew = GetNewDt(dt);
            this.BuildGridPanel(dtnew).AddTo(this.viewport1);
            this.viewport1.Remove("gridpl");
            editnum.Disable(true);
        }
        else
        {
            columndepname.Text = cmbdept.SelectedItem.Text;
            DataTable dt = BG_CaliberLogic.GetAllBG_CaliberMon(deptid, year);
            if (dt != null)
            {
                DataTable dtnew = GetNewDt(dt);
                Store1.DataSource = dtnew;
                Store1.DataBind();
            }


        }

    }

    private DataTable GetNewDt(DataTable dt)
    {
        DataTable dtnew = dt.Clone();
        int a1 = 0, a2 = 0, a3 = 0, a4 = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dtnew.NewRow();
            if (dt.Rows[i]["CbLever"].ToString() == "1")
            {
                a1 += 1;
                a2 = 0; a3 = 0; a4 = 0;
                drBing(dr, dt, i);
                dr["CaliberName"] = Common.NumberToCurrency.ConvertLessThanHundred(a1) + "、" + dt.Rows[i]["CaliberName"];
            }
            else if (dt.Rows[i]["CbLever"].ToString() == "2")
            {
                a2 += 1;
                a3 = 0; a4 = 0;
                drBing(dr, dt, i);
                dr["CaliberName"] = "　　" + "(" + Common.NumberToCurrency.ConvertLessThanHundred(a2) + ")" + "、" + dt.Rows[i]["CaliberName"];
            }
            else if (dt.Rows[i]["CbLever"].ToString() == "3")
            {
                a3 += 1;
                a4 = 0;
                drBing(dr, dt, i);
                dr["CaliberName"] = "　　" + "　　" + a3 + "、" + dt.Rows[i]["CaliberName"];
            }
            else if (dt.Rows[i]["CbLever"].ToString() == "4")
            {
                a4 += 1;
                drBing(dr, dt, i);
                dr["CaliberName"] = "　　" + "　　" + "　　" + Common.NumberToCurrency.ConvertToABC(a4) + "、" + dt.Rows[i]["CaliberName"];
            }
            dtnew.Rows.Add(dr);
        }
        return dtnew;
    }
    [DirectMethod]
    public void Dodisable(string caliberID)
    {
        editnum.Enable(true);
        bool flag = false;
        int t = common.IntSafeConvert(caliberID);
        flag = BG_CaliberLogic.IsLeafEnd(t);

        string message = "<b>无法在此处填写</b>";
        if (!flag)
        {
            if (!editnum.Enabled)
            {

            }
            else
            {
                editnum.Disable(true); X.Msg.Notify(new NotificationConfig()
                {
                    Title = "消息提示",
                    Html = message,
                    Width = 250
                }).Show();
            }

        }
    }
    private void drBing(DataRow dr, DataTable dt, int j)
    {

        for (int i = 0; i < dt.Columns.Count; i++)
        {
            dr[i] = dt.Rows[j][i];
        }
    }

    [DirectMethod(Namespace = "CompanyX")]
    public void Edit2(int id, string name, object BGAMID, string oldValue, string newValue, object customer)
    {
        int deptid = DepID;
        string message = " {0}<br /><b>原收入经费:</b> {1}<br /><b>更改收入经费:</b> {2}";
        bool flag = false;
        BG_Amount ma = new BG_Amount();
        ma = BG_AmountLogic.GetBG_AmountByYear(id, common.IntSafeConvert(cmbyear.SelectedItem.Value), deptid);
        if (ma == null)
        {
            ma = new BG_Amount();
            ma.BGAMIncome = ParToDecimal.ParToDel(newValue);
            ma.DepID = deptid;
            ma.BGAMYear = common.IntSafeConvert(cmbyear.SelectedItem.Value);
            ma.CBID = id;
            flag = BG_AmountManager.AddBG_Amount(ma).BGAMID > 0;
        }
        else
        {
            ma.BGAMIncome = ParToDecimal.ParToDel(newValue);
            flag = BG_AmountManager.ModifyBG_Amount(ma);
        }
        if (flag)
        {
            X.Msg.Notify(new NotificationConfig()
            {
                Title = "消息提示",
                Html = string.Format(message, common.IntSafeConvert(cmbyear.SelectedItem.Value) - 1 + "年" + "口径：" + name.Split('、')[1], oldValue, newValue),
                Width = 250
            }).Show();
        }
    }

    [DirectMethod(Namespace = "CompanyX")]
    public void Edit1(int id, string name, object BGAMID, string oldValue, string newValue, object customer)
    {
        int deptid = DepID;
        if (UserLimStr == "审核员")
        {
            deptid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
            editnum.Disable(true);
        }
        string message = " {0}<br /><b>原支出经费:</b> {1}<br /><b>更改支出经费:</b> {2}";
        bool flag = false;
        if (common.IntSafeConvert(BGAMID) == 0)
        {
            BG_Amount ma = new BG_Amount();
            ma.BGAMMon = ParToDecimal.ParToDel(newValue);
            ma.DepID = deptid;
            ma.BGAMYear = common.IntSafeConvert(cmbyear.SelectedItem.Value) - 1;
            ma.CBID = id;
            flag = BG_AmountManager.AddBG_Amount(ma).BGAMID > 0;
        }
        else
        {
            BG_Amount ma = new BG_Amount();
            ma = BG_AmountLogic.GetBG_AmountByYear(id, common.IntSafeConvert(cmbyear.SelectedItem.Value) - 1, deptid);
            if (ma == null)
            {
                ma = new BG_Amount();
                ma.BGAMMon = ParToDecimal.ParToDel(newValue);
                ma.DepID = deptid;
                ma.BGAMYear = common.IntSafeConvert(cmbyear.SelectedItem.Value) - 1;
                ma.CBID = id;
                flag = BG_AmountManager.AddBG_Amount(ma).BGAMID > 0;
            }
            else
            {
                ma.BGAMMon = ParToDecimal.ParToDel(newValue);
                flag = BG_AmountManager.ModifyBG_Amount(ma);
            }
        }
        if (flag)
        {
            X.Msg.Notify(new NotificationConfig()
            {
                Title = "消息提示",
                Html = string.Format(message, common.IntSafeConvert(cmbyear.SelectedItem.Value) - 1 + "年" + "口径：" + name.Split('、')[1], oldValue, newValue),
                Width = 250
            }).Show();
        }
    }
    [DirectMethod(Namespace = "CompanyX")]
    public void Edit(int id, string name, object BGAMID, string oldValue, string newValue, object customer)
    {
        int deptid = DepID;
        if (UserLimStr == "审核员")
        {
            deptid = common.IntSafeConvert(cmbdept.SelectedItem.Value);
            editnum.Disable(true);
        }
        string message = " {0}<br /><b>原支出经费:</b> {1}<br /><b>更改支出经费:</b> {2}";
        bool flag = false;
        BG_Amount ma = new BG_Amount();
        ma = BG_AmountLogic.GetBG_AmountByYear(id, common.IntSafeConvert(cmbyear.SelectedItem.Value), deptid);
        if (ma == null)
        {
            ma = new BG_Amount();
            ma.BGAMMon = ParToDecimal.ParToDel(newValue);
            ma.DepID = deptid;
            ma.BGAMYear = common.IntSafeConvert(cmbyear.SelectedItem.Value);
            ma.CBID = id;
            flag = BG_AmountManager.AddBG_Amount(ma).BGAMID > 0;
        }
        else
        {
            ma.BGAMMon = ParToDecimal.ParToDel(newValue);
            flag = BG_AmountManager.ModifyBG_Amount(ma);
        }
        if (flag)
        {
            X.Msg.Notify(new NotificationConfig()
            {
                Title = "消息提示",
                Html = string.Format(message, common.IntSafeConvert(cmbyear.SelectedItem.Value) + "年" + "口径：" + name.Split('、')[1], oldValue, newValue),
                Width = 250
            }).Show();
        }
        // Send Message...


        //this.GridPanel1.GetStore().GetById(id).Commit();

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
        if (UserLimStr == "审核员")
        {
            int year = common.IntSafeConvert(CurrentYear);
            try
            {
                year = common.IntSafeConvert(cmbyear.SelectedItem.Value);
            }
            catch
            {
                year = common.IntSafeConvert(CurrentYear);
            }
            if (year == 0)
            {
                year = common.IntSafeConvert(CurrentYear);
            }
            DataTable dt = BG_DepartmentLogic.GetDepByfadepid(AreaDepID, year);
            cmbdeptStore.DataSource = dt;
            cmbdeptStore.DataBind();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //  cmbDepnaem.Items.Add(new Ext.Net.ListItem(depTable.Rows[i]["depName"].ToString(), depTable.Rows[i]["depID"].ToString()));
            //    cmbdept.Items.Add(new Ext.Net.ListItem(dt.Rows[i]["DepName"].ToString(), dt.Rows[i]["DepID"].ToString()));
            //}
        }
        else
        {

            cmbdept.Items.Add(new Ext.Net.ListItem(DepName, DepID.ToString()));
            cmbdept.Disable(true);
        }

    }
    protected void btnsend_DirectClick(object sender, DirectEventArgs e)
    {
       GetGridData();
        //string rp = string.Format("BudgetMonEdit.apsx?Year={0}&cmbdeptid={1}", cmbyear.SelectedItem.Text, cmbdept.SelectedItem.Text);
        //Response.Redirect(rp, true);
    }




    protected void exbtn_Click(object sender, EventArgs e)
    {
        //string json = GridData.Value.ToString();
        //StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        //XmlNode xml = eSubmit.Xml;

        //this.Response.Clear();
        //this.Response.ContentType = "application/vnd.ms-excel";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=汇总查询.xls");
        //XslCompiledTransform xtExcel = new XslCompiledTransform();
        //xtExcel.Load(Server.MapPath("Excel.xsl"));
        //xtExcel.Transform(xml, null, this.Response.OutputStream); 
        //this.Response.End();
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
        List<Ext.Net.ListItem> listvalue = cmbdept.SelectedItems.ToList();
        int k = 0, p = 0;
        int listcout = 0;
        if (listvalue.Count == 1)
        {
            listcout = 1;
        }
        else { listcout = listvalue.Count + 1; }
        IRow headrow = sheet.CreateRow(0);
        for (int i = 0; i < listcout; i++)
        {
            if (i == 0)
            {
                ICell cell = headrow.CreateCell(k);
                cell.SetCellValue(cjname.Text);
                cell.CellStyle = style;
            }
            string depnametmp = "";
            if (listcout > 1 && i == 0)
            {
                depnametmp = "预算支出";
            }
            else if (listcout == 1 && i == 0)
            {
                depnametmp = listvalue[i].Text;
            }
            else { depnametmp = listvalue[i - 1].Text; }
            k++;
            ICell cell1 = headrow.CreateCell(k);
            cell1.SetCellValue(depnametmp);
            cell1.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, k, i == 0 ? k + 5 : k + 4));
            k = i == 0 ? k + 5 : k + 4;
        }
        IRow headrow1 = sheet.CreateRow(1);
        for (int i = 0; i < listcout; i++)
        {
            string leaftext = "";
            string depnametmp = "";
            if (listcout > 1 && i == 0)
            {
                depnametmp = "预算支出";
            }
            else if (listcout == 1 && i == 0)
            {
                depnametmp = listvalue[i].Text;
            }
            else { depnametmp = listvalue[i - 1].Text; }
            for (int j = 0; j < columndepname.Columns.Count; j++)
            {
                leaftext = columndepname.Columns[j].Text.Contains("（元）") ? columndepname.Columns[j].Text.Replace("（元）", "（万元）") : columndepname.Columns[j].Text;
                p++;
                ICell cell = headrow1.CreateCell(p);
                cell.SetCellValue(leaftext);
                cell.CellStyle = style;
                if (i == 0 && j == 4 && depnametmp == "预算支出")
                {
                    p++;
                    ICell cell1 = headrow1.CreateCell(p);
                    cell1.SetCellValue("总计（万元）");
                    cell1.CellStyle = style;
                }
            }
        }
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));

        #region 第二步：将DataTable导出到Excel
        if (dt != null && dt.Rows.Count > 0)
        {
            MemoryStream ms = ExcelRender.RenderToExcel(dt, workbook);
            Response.ContentType = "application/xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("汇总查询.xls", System.Text.Encoding.UTF8));
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