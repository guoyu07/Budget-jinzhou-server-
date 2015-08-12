using Budget.DataAccess.AutoCreate.BLL;
using Budget.DataAccess.AutoCreate.Model;
using Expense.BLL;
using Expense.DAL;
using Expense.Methods;
using Expense.Model;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cashier_keshiyewufei : System.Web.UI.Page, IPostBackEventHandler
{
    protected string lieming;
    protected string zhuijia;

    protected string zjje;

    protected string name;

   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RpDataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string sqlstr = "select BQJE,SQJE,ZJJE,Depname,LSJL from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
        DataTable dt = DBUnity.AdapterToTab(sqlstr);


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string aa = dt.Rows[i]["LSJL"].ToString();
            decimal zjje = 0;
            if (aa.Trim('@').Split('#').Length > 1)
            {
                for (int k = 0; k < aa.Trim('@').Split('@').Length; k++)
                {
                    zjje += ParseUtil.ToDecimal(aa.Trim('@').Split('@')[k].Split('#')[0], 0);
                }
            }
            dt.Rows[i]["ZJJE"] = 0;
        }
        dt.Columns.Remove("LSJL");
        MemoryStream ms = ExcelRender.RenderToExcelSetHead(dt, "本期金额#上年余额#追加金额#部门名");
        Response.ContentType = "application/xls";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("部门.xls", System.Text.Encoding.UTF8));
        Response.BinaryWrite(ms.ToArray());
        Response.End();
    }
    private void RpDataBind()
    {
        string sqlstr = "select * from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
        DataTable dt = DBUnity.AdapterToTab(sqlstr);
        dt.Columns.Add("Total");
        name = "<th>部门名</th>   <th>上年余额(元)</th> <th>本期金额(元)</th> ";

        dt.Columns.Add("aaaa");
        int sum = 0;
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string JL = dt.Rows[i]["LSJL"].ToString();
                if (JL != null)
                {
                    int sum1 = JL.TrimEnd('@').Split('@').Length;
                    if (sum1 > sum)
                    {
                        sum = sum1;
                    }
                }
            }
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            decimal total = 0;
            string JL = dt.Rows[i]["LSJL"].ToString();
            if (JL != "" && JL != null)
            {
                int su = JL.TrimEnd('@').Split('@').Length;
                for (int a = 0; a < sum - su; a++)
                {
                    JL += "#@";
                }

                for (int j = 0; j < JL.TrimEnd('@').Split('@').Length; j++)
                {
                    string content = JL.TrimEnd('@').Split('@')[j].ToString();
                    string[] str = content.Split('#');
                    if (str.Length != 1)
                    {
                        dt.Rows[i]["aaaa"] += "<td style=\"color:red;\">" + str[0] + "<br/>" + str[1] + "</td>";

                        total += ParseUtil.ToDecimal(str[0], 0);
                    }
                    else
                    {
                        for (int k = 0; k < sum; k++)
                        {
                            dt.Rows[i]["aaaa"] += "<td>   </td>";
                        }
                    }
                }
            }

            else
            {
                for (int x = 0; x < sum; x++)
                {
                    dt.Rows[i]["aaaa"] += "<td>   </td>";
                }
            }

            total += ParseUtil.ToDecimal(dt.Rows[i]["SQJE"].ToString(), 0) + ParseUtil.ToDecimal(dt.Rows[i]["BQJE"].ToString(), 0);
            dt.Rows[i]["Total"] = total;

            dt.Rows[i]["KYJE"] = total;

        }
        for (int i = 0; i < sum; i++)
        {
            name = name + " <th>" + "追加金额" + (i + 1) + "</th>";
        }
        name = name + "<th>合计</th>  ";
        repBudMonYear.DataSource = dt;
        repBudMonYear.DataBind();
    }


    public void RaisePostBackEvent(string eventArgument)
    {
        DataTable dt = new DataTable();

        dt = ImportExcelFile(hidpath.Value);

        switch (eventArgument)
        {
            case "是":
                decimal je = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    je += ParseUtil.ToDecimal(dt.Rows[i]["追加金额"].ToString(), 0);
                }
                string sqlstr11 = "select * from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
                DataTable dtOld = DBUnity.AdapterToTab(sqlstr11);
                DataTable dtNew = new DataTable();
                dt.Columns.Add("aaaa");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string depname = dt.Rows[i]["部门名"].ToString();
                    for (int j = 0; j < dtOld.Rows.Count; j++)
                    {
                        if (dtOld.Rows[j]["Depname"].ToString() == depname)
                        {
                            string zj = dtOld.Rows[j]["LSJL"].ToString();

                            if (je > 0)
                            {
                                zj += dt.Rows[i]["追加金额"].ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "@";
                            }


                            dt.Rows[i]["aaaa"] = zj;
                        }
                        else
                        {
                            RM_keshiyewufei rk = new RM_keshiyewufei();
                            rk.Depname = dt.Rows[i]["部门名"].ToString();
                            rk.SQJE = ParseUtil.ToDecimal(dt.Rows[i]["上年余额"].ToString(), 0);
                            rk.BQJE = ParseUtil.ToDecimal(dt.Rows[i]["本期金额"].ToString(), 0);
                            rk.ZJJE = ParseUtil.ToDecimal(dt.Rows[i]["追加金额"].ToString(), 0);

                            string aa = dt.Rows[i]["aaaa"].ToString();

                            rk.LSJL = rk.ZJJE + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "@";


                            rk.KYJE = rk.ZJJE + rk.BQJE + rk.SQJE;

                            rk.Remark = 0;
                            rk.Remark1 = DateTime.Now.Year.ToString();
                            RM_keshiyewufeiManager.AddRM_keshiyewufei(rk);
                        }
                    }
                }
                string strsql = "delete from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
                DBUnity.ExecuteNonQuery(CommandType.Text, strsql, null);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RM_keshiyewufei rk = new RM_keshiyewufei();
                    rk.Depname = dt.Rows[i]["部门名"].ToString();
                    rk.SQJE = ParseUtil.ToDecimal(dt.Rows[i]["上年余额"].ToString(), 0);
                    rk.BQJE = ParseUtil.ToDecimal(dt.Rows[i]["本期金额"].ToString(), 0);
                    rk.ZJJE = ParseUtil.ToDecimal(dt.Rows[i]["追加金额"].ToString(), 0);

                    rk.LSJL = dt.Rows[i]["aaaa"].ToString();

                    string aa = dt.Rows[i]["aaaa"].ToString();

                    decimal zj = 0;
                    for (int j = 0; j < aa.Trim('@').Split('@').Length; j++)
                    {
                        zj += ParseUtil.ToDecimal(aa.Trim('@').Split('@')[j].Split('#')[0], 0);
                    }

                    rk.KYJE = zj + rk.BQJE + rk.SQJE;

                    rk.Remark = 0;
                    rk.Remark1 = DateTime.Now.Year.ToString();
                    RM_keshiyewufeiManager.AddRM_keshiyewufei(rk);
                }
                MessageBox.Show(this, "导入成功");

                RpDataBind();

                break;
            case "否":

                string sqlstr = "select * from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
                DataTable dtUpd = DBUnity.AdapterToTab(sqlstr);

                dt.Columns.Add("newje");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string depname = dt.Rows[i]["部门名"].ToString();
                    for (int j = 0; j < dtUpd.Rows.Count; j++)
                    {
                        if (dtUpd.Rows[j]["Depname"].ToString() == depname)
                        {
                            string zj = dtUpd.Rows[j]["LSJL"].ToString().TrimEnd('@');
                            if (zj.Contains("@"))
                            {
                                zj = zj.Substring(0, zj.LastIndexOf('@'));
                                zj += dt.Rows[i]["追加金额"].ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "@";
                            }
                            else
                            {
                                zj = dt.Rows[i]["追加金额"].ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "@";
                            }
                             

                            dt.Rows[i]["newje"] = zj;

                        }
                    }
                }

                string strsqlde = "delete from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
                DBUnity.ExecuteNonQuery(CommandType.Text, strsqlde, null);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RM_keshiyewufei rk = new RM_keshiyewufei();
                    rk.Depname = dt.Rows[i]["部门名"].ToString();
                    rk.SQJE = ParseUtil.ToDecimal(dt.Rows[i]["上年余额"].ToString(), 0);
                    rk.BQJE = ParseUtil.ToDecimal(dt.Rows[i]["本期金额"].ToString(), 0);
                    rk.ZJJE = ParseUtil.ToDecimal(dt.Rows[i]["追加金额"].ToString(), 0);

                    rk.LSJL = dt.Rows[i]["newje"].ToString();

                    string aa = dt.Rows[i]["newje"].ToString();

                    decimal zj = 0;
                    for (int j = 0; j < aa.Trim('@').Split('@').Length; j++)
                    {
                        zj += ParseUtil.ToDecimal(aa.Trim('@').Split('@')[j].Split('#')[0], 0);
                    }

                    rk.KYJE = zj + rk.BQJE + rk.SQJE;

                    rk.Remark = 0;
                    rk.Remark1 = DateTime.Now.Year.ToString();
                    RM_keshiyewufeiManager.AddRM_keshiyewufei(rk);
                }
                MessageBox.Show(this, "导入成功");

                RpDataBind();

                break;
        }
    }

    public void BackNo(DataTable dt)
    {

    }
    protected void btnIn_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName1 = this.fileSelect.PostedFile.FileName;
            string name = this.GetFileName(fileName1);
            string path = base.Server.MapPath("~/files") + @"\" + name;
            this.fileSelect.SaveAs(path);
            hidpath.Value = path;

            string sqlstr = "select * from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
            DataTable dtsql = DBUnity.AdapterToTab(sqlstr);
            if (dtsql.Rows.Count > 0)
            {
                var s = "if(confirm('点击确定追加金额！点击取消覆盖上一次的金额')){0};else {1};";
                ScriptManager.RegisterStartupScript(this, typeof(cashier_keshiyewufei), "",
                    string.Format(s, this.ClientScript.GetPostBackEventReference(this, "是"), this.ClientScript.GetPostBackEventReference(this, "否")),
                    true);
            }
            else
            {
                DataTable dt = ImportExcelFile(path);

                string sqlstr11 = "select * from RM_keshiyewufei where Remark1='" + DateTime.Now.Year + "' and Remark=0";
                DataTable dtOld = DBUnity.AdapterToTab(sqlstr11);
                DataTable dtNew = new DataTable();
                dt.Columns.Add("aaaa");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RM_keshiyewufei rk = new RM_keshiyewufei();
                    rk.Depname = dt.Rows[i]["部门名"].ToString();
                    rk.SQJE = ParseUtil.ToDecimal(dt.Rows[i]["上年余额"].ToString(), 0);
                    rk.BQJE = ParseUtil.ToDecimal(dt.Rows[i]["本期金额"].ToString(), 0);
                    rk.ZJJE = ParseUtil.ToDecimal(dt.Rows[i]["追加金额"].ToString(), 0);

                    rk.LSJL = rk.ZJJE.ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "@";

                    string aa = rk.LSJL.ToString();

                    decimal zj = 0;
                    for (int j = 0; j < aa.Trim('@').Split('@').Length; j++)
                    {
                        zj += ParseUtil.ToDecimal(aa.Trim('@').Split('@')[j].Split('#')[0], 0);
                    }

                    rk.KYJE = zj + rk.BQJE + rk.SQJE;

                    rk.Remark = 0;
                    rk.Remark1 = DateTime.Now.Year.ToString();
                    RM_keshiyewufeiManager.AddRM_keshiyewufei(rk);
                }
                MessageBox.Show(this, "导入成功");

                RpDataBind();

            }


        }
        catch
        {

        }
    }

    private string GetFileName(string file)
    {
        return (DateTime.Now.Ticks + "." + file.Substring(file.LastIndexOf(".") + 1));
    }

    ///<summary>
    ///读取xls\xlsx格式的Excel文件的方法 
    ///</ummary>
    ///<param name="path">待读取Excel的全路径</param>
    ///<returns></returns>
    private DataTable ReadExcelToTable(string path)
    {
        //连接字符串
        //   string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";  // Office 07及以上版本不能出现多余的空格 而且分号注意

        string connstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

        //Office 07以下版本 因为本人用Office2010 所以没有用到这个连接字符串  可根据自己的情况选择 或者程序判断要用哪一//个连接字符串
        using (OleDbConnection conn = new OleDbConnection(connstring))
        {

            conn.Open();
            DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });  //得到所有sheet的名字    
            string firstSheetName = sheetsName.Rows[0][2].ToString();   //得到第一个sheet的名字    

            string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);
            OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);

            DataSet set = new DataSet();
            ada.Fill(set);
            return set.Tables[0];
        }
    }





    /// <summary>  
    /// 读取Excel文件到table中  
    /// </summary>  
    /// <param name="filePath">excel文件路径</param>  
    /// <returns></returns>  
    public static DataTable ReadExcel(string fileName)
    {
        DataTable dt = new DataTable();
        //string filePath = "";
        //if (basePath != null)
        //{
        //    filePath = HostingEnvironment.MapPath((basePath.ToString() + fileName));
        //    dt = ImportExcelFile(filePath);
        //}
        ////文件是否存在  
        //if (System.IO.File.Exists(filePath))
        //{

        //}
        return dt;
    }
    public static DataTable ImportExcelFile(string filePath)
    {
        HSSFWorkbook hssfworkbook;
        #region//初始化信息
        try
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        #endregion

        NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
        DataTable dt = new DataTable();
        rows.MoveNext();
        HSSFRow row = (HSSFRow)rows.Current;
        for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
        {
            //dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());  
            //将第一列作为列表头  
            dt.Columns.Add(row.GetCell(j).ToString());
        }
        while (rows.MoveNext())
        {
            row = (HSSFRow)rows.Current;
            DataRow dr = dt.NewRow();
            for (int i = 0; i < row.LastCellNum; i++)
            {
                NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    dr[i] = cell.ToString();
                }
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }

}
