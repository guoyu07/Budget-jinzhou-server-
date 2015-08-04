using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetWeb.DAL;
using Ext.Net;
using BudgetWeb.Model;
using System.Text;

public partial class WebPage_Setting_DataBak : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetbakBind();
    }

    private void GetbakBind()
    {
        DataTable dt = BG_Unit_DbbakService.GetAllBG_Unit_Dbbak();
        gridplstore.DataSource = dt;
        gridplstore.DataBind();
    }


    #region 原备份数据绑定
    //private void GetbakBind()
    //{
    //    string path = Server.MapPath("~\\DataFile");
    //    List<FileInfo> listfList = GetAllFilesInDirectory(path);
    //    DataTable dtTable = new DataTable();
    //    dtTable.Columns.Add("Name");
    //    dtTable.Columns.Add("CreationTime");
    //    dtTable.Columns.Add("DirectoryName");
    //    for (int i = 0; i < listfList.Count; i++)
    //    {
    //        DataRow dr = dtTable.NewRow();
    //        dr["Name"] = listfList[i].Name;
    //        dr["CreationTime"] = listfList[i].CreationTime;
    //        dr["DirectoryName"] = listfList[i].DirectoryName;
    //        dtTable.Rows.Add(dr);
    //    }
    //    if (listfList != null && listfList.Count > 0)
    //    {
    //        gridplstore.DataSource = dtTable;
    //        gridplstore.DataBind();
    //    }

    //}



    ///// <summary>
    ///// 返回指定目录下的所有文件信息
    ///// </summary>
    ///// <param name="strDirectory"></param>
    ///// <returns></returns>
    //public List<FileInfo> GetAllFilesInDirectory(string strDirectory)
    //{
    //    List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息  
    //    try
    //    {
    //        DirectoryInfo directory = new DirectoryInfo(strDirectory);
    //        DirectoryInfo[] directoryArray = directory.GetDirectories();
    //        FileInfo[] fileInfoArray = directory.GetFiles();
    //        if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
    //        foreach (DirectoryInfo _directoryInfo in directoryArray)
    //        {
    //            DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
    //            DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
    //            FileInfo[] fileInfoArrayA = directoryA.GetFiles("*.bak");
    //            if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
    //            GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
    //        }
    //    }
    //    catch
    //    {
    //        ExtNet.Msg.Alert("系统提示", "还是这里报错了").Show();
    //    }

    //    return listFiles;
    //} 
    #endregion


    protected void btnbak_DirectClick(object sender, Ext.Net.DirectEventArgs e)
    {
        Winadd.Show();
    }
    [DirectMethod]
    #region 原还原
    //public void btnrestore(string name)
    //{
    //    string tmp = AdName.Text;
    //    tmp = name;
    //    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
    //    string dbFileName = "";
    //    dbFileName = tmp;
    //    SqlCommand command = new SqlCommand("use master ;ALTER DATABASE [BudgetNetDB] SET OFFLINE WITH ROLLBACK IMMEDIATE;restore database @name from disk=@path  WITH REPLACE;", connection);
    //    connection.Open();
    //    string path = "";
    //    path = Server.MapPath("~\\DataFile");
    //    path = path + "\\" + @dbFileName;
    //    command.Parameters.AddWithValue("@name", "BudgetNetDB");
    //    command.Parameters.AddWithValue("@path", path);
    //    command.ExecuteNonQuery();
    //    connection.Close();
    //    X.Msg.Alert("系统提示", "还原成功！").Show();
    //} 
    public   void  btnrestore(string dbid)
    {
        string dbname = DBUnity.connectionString.Split(';')[1].Split('=')[1];
        string tmp = BG_Unit_DbbakService.GetBG_Unit_DbbakByDbID(Common.common.IntSafeConvert(dbid)).DbName;
        string  connectionStringserver = ConfigurationManager.ConnectionStrings["ConnectionStringserver"].ToString();
        string sql = " DECLARE  @sql  NVARCHAR (MAX)"
   + " DECLARE  @sql1  NVARCHAR (MAX)"
   + " DECLARE  @sql2  NVARCHAR (MAX)"
   + " DECLARE @i int"
   + " DECLARE @tabname  NVARCHAR (MAX)"
   + " set @i=1"
   + " while @i<=(select count(tab.name) from  {1}.dbo.SysObjects as tab where xtype='u'  and name not like '%_Unit_%' )"
   + " begin"
   + " set  @tabname=(select top(1) * from (select top(@i) tab.name from {1}.dbo.SysObjects as tab where xtype='u' and name not like"
   + "       '%_Unit_%'  order by name asc) as a  order by name desc)"
   + "   SET @sql1 =  'select * into {1}.dbo.'+@tabname+' from ' +'{0}.[dbo].'+@tabname "
   + "   SET @sql2 =  'drop table {1}.[dbo].'+@tabname   +' '  +  @sql1 "
   + " SET @sql =  ''if object_id(N'{1}.[dbo].'+@tabname+'',N'U') is not null   EXEC(@sql2)    else  EXEC(@sql1) "
   + " EXEC(@sql); "
   + " set @i= @i+1 "
   + " end ";
        sql = string.Format(sql, tmp, dbname);
        int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, null);
        if (t > 0)
        {
            FileStream aFile = new FileStream(Server.MapPath("~\\DataFile") + "\\" + "存储过程.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(aFile, Encoding.UTF8);
            string sqlccgc = sr.ReadToEnd();
            sr.Close();
            aFile.Close();
            int c = 0;
            try
            {
                string serversql = string.Format("EXEC  sp_dropserver 'JZDBServer' , 'droplogins'    EXEC sp_addlinkedserver 'JZDBServer','','SQLNCLI','{0}'      EXEC sp_addlinkedsrvlogin 'JZDBServer','false',NULL,'sa','{1}'", connectionStringserver.Split(';')[0].Split('=')[1], connectionStringserver.Split(';')[3].Split('=')[1]);
                DBUnity.ExecuteNonQuery(CommandType.Text, serversql, null);
                string[] arr = System.Text.RegularExpressions.Regex.Split(sqlccgc.Replace("\r\n"," "), "GO");
                try
                {
                    for (int n = 0; n < arr.Length; n++)
                    {
                        string strsql = arr[n];
                        if (strsql.Trim().Length > 1)
                        {
                            DBUnity.ExecuteNonQuery(CommandType.Text, strsql, null);
                        }

                    }
                    c++;
                }
                catch 
                {
                    c = 0; 
                }
               
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("已存在"))
                {
                    c++;
                }
            }
            if (c > 0)
            {
                X.Msg.Alert("系统提示", "还原成功！").Show();
            }
            else
            {
                X.Msg.Alert("系统提示", "还原失败！").Show();
            }
        }
    }

   
    #endregion
    [DirectMethod]
    public void btndownload(string name)
    {
        //string tmp = AdName.Text;
        //tmp = name;
        //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
        //string dbFileName = "";
        //dbFileName = tmp;
        //string path = "";
        //path = Server.MapPath("~\\DataFile");
        //path = path + "\\" + dbFileName;


        //    FileInfo info = new FileInfo(path);
        //           long fileSize = info.Length;
        //           HttpContext.Current.Response.Clear();                    

        //           //指定Http Mime格式为压缩包
        //           HttpContext.Current.Response.ContentType = "application/unknown";

        //            // Http 协议中有专门的指令来告知浏览器, 本次响应的是一个需要下载的文件. 格式如下:
        //           // Content-Disposition: attachment;filename=filename.txt
        //           HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(info.FullName));
        //           //不指明Content-Length用Flush的话不会显示下载进度   
        //           HttpContext.Current.Response.AddHeader("Content-Length", fileSize.ToString());
        //           HttpContext.Current.Response.TransmitFile(path, 0, fileSize);
        //           HttpContext.Current.Response.Flush();

        //FileInfo fi = new FileInfo(path);
        //Response.ClearHeaders();
        //Response.AppendHeader("Content-Disposition", "attachment;filename="
        //    //将文件名改成Guid  
        //    + string.Format("{0:n}{1}", System.Guid.NewGuid(), fi.Extension));
        ////文件的大小  
        //Response.AddHeader("Content-Length", fi.Length.ToString());
        //Response.AppendHeader("Last-Modified", fi.LastWriteTime.ToFileTime().ToString());
        //Response.AppendHeader("Location", Request.Url.AbsoluteUri);
        ////文件的类型。如：pdf文件为："application/pdf",  
        ////此处为"application/unknown" 未知类型(浏览器会根据文件类型自动判断)  
        //Response.ContentType = "application/unknown";
        //Response.Clear();
        //Response.ContentType = "application/octet-stream";
        //Response.AddHeader("Content-Disposition", "attachment; filename=预算系统数据备份"); 
        //Response.WriteFile(path);
        //Response.End();
    }

    //protected void btngetbak_DirectClick(object sender, Ext.Net.DirectEventArgs e)
    //{
    //    string tmp = AdName.Text;
    //    RowSelectionModel sm = this.gridpl.GetSelectionModel() as RowSelectionModel;
    //    if (sm.SelectedRows.Count != 1)
    //    {
    //        X.Msg.Alert("系统提示", "请选择一条记录还原！").Show();
    //        return;
    //    }
    //    else
    //    {
    //        string json = e.ExtraParams["Values"];
    //        Dictionary<string, string>[] companies = JSON.Deserialize<Dictionary<string, string>[]>(json);
    //        tmp = companies[0]["Name"];
    //    }
    //    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
    //    string dbFileName = "";
    //    dbFileName = tmp;
    //    SqlCommand command = new SqlCommand("use master ;ALTER DATABASE [BudgetNetDB] SET OFFLINE WITH ROLLBACK IMMEDIATE;restore database @name from disk=@path  WITH REPLACE;", connection);
    //    connection.Open();
    //    string path = "";
    //    path = Server.MapPath("~\\DataFile");
    //    path = path + "\\" + @dbFileName;
    //    command.Parameters.AddWithValue("@name", "BudgetNetDB");
    //    command.Parameters.AddWithValue("@path", path);
    //    command.ExecuteNonQuery();
    //    connection.Close();
    //    X.Msg.Alert("系统提示", "还原成功！").Show();
    //}

    #region 原备份
    //protected void btnWinAdd_DirectClick(object sender, DirectEventArgs e)
    //{
    //    SqlConnection connection =
    //        new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
    //    string dbFileName = "";

    //    dbFileName = AdName.Text;
    //    if (!dbFileName.EndsWith(".bak"))
    //    {
    //        dbFileName += ".bak";
    //    }
    //    SqlCommand command = new SqlCommand("use master;backup database @name to disk=@path;", connection);
    //    connection.Open();
    //    string path = Server.MapPath("~\\DataFile");
    //    if (!Directory.Exists(path))
    //    {
    //        Directory.CreateDirectory(path);
    //    }
    //    path = path + "\\" + dbFileName;
    //    if (!File.Exists(path))
    //    {
    //        command.Parameters.AddWithValue("@name", "BudgetNetDB");
    //        command.Parameters.AddWithValue("@path", path);
    //        int t = command.ExecuteNonQuery();
    //        connection.Close();
    //        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>window.location.href = window.location.href;</script>");
    //        //GetbakBind();
    //        //Winadd.Hide();
    //        X.Msg.Confirm("系统提示", "备份成功！", new MessageBoxButtonsConfig
    //        {
    //            Yes = new MessageBoxButtonConfig
    //            {
    //                Handler = "SS.DoYesSSS()",
    //                Text = "确定"
    //            }
    //        }).Show();
    //    }
    //    else
    //    {
    //        X.Msg.Alert("系统提示", "已存在相同的文件名！").Show();
    //    }
    //} 
    #endregion
    protected void btnWinAdd_DirectClick(object sender, DirectEventArgs e)
    {
        string dbFileName = "";

        dbFileName = AdName.Text;
        string sql = "  CREATE DATABASE  {0}"
 + " DECLARE  @sql  NVARCHAR (MAX)"
 + " DECLARE  @sql1  NVARCHAR (MAX)"
 + " DECLARE  @sql2  NVARCHAR (MAX)"
 + " DECLARE @i int"
 + " DECLARE @tabname  NVARCHAR (MAX)"
 + " set @i=1"
 + " while @i<=(select count(tab.name) from SysObjects as tab where xtype='u'  and name not like '%_Unit_%' )"
 + " begin"
 + " set  @tabname=(select top(1) * from (select top(@i) tab.name from SysObjects as tab where xtype='u' and name not like"
 + "       '%_Unit_%'  order by name asc) as a  order by name desc)"
 + "   SET @sql1 =  'select * into {0}.[dbo].'+@tabname+' from ' +@tabname "
 + "   SET @sql2 =  'drop table {0}.[dbo].'+@tabname    +  @sql1 "
 + " SET @sql =  ''if object_id(N'{0}.[dbo].'+@tabname+'',N'U') is not null   EXEC(@sql2)    else  EXEC(@sql1) "
 + " EXEC(@sql); "
 + " set @i= @i+1 "
 + " end ";
        sql = string.Format(sql, dbFileName);
        int t = DBUnity.ExecuteNonQuery(CommandType.Text, sql, null);
        if (t > 0)
        {
            BG_Unit_Dbbak dbbak = new BG_Unit_Dbbak();
            dbbak.DbName = dbFileName;
            dbbak.DbCreationTime = DateTime.Now;
            if (BG_Unit_DbbakService.AddBG_Unit_Dbbak(dbbak).DbID > 0)
            {
                X.Msg.Confirm("系统提示", "备份成功！", new MessageBoxButtonsConfig
           {
               Yes = new MessageBoxButtonConfig
               {
                   Handler = "SS.DoYesSSS()",
                   Text = "确定"
               }
           }).Show();
            }

        }
    }
    [DirectMethod(Namespace = "SS")]
    public void DoYesSSS()
    {
        Winadd.Hide();
    }

    protected void btnWinCancel_DirectClick(object sender, DirectEventArgs e)
    {
        Winadd.Hide();
    }

}


public static class ExtMethods
{
    /// <summary>  
    /// 执行带GO的SQL，返回最后一条SQL的受影响行数  
    /// </summary>  
    /// <param name="sql"></param>  
    /// <returns>返回最后一条SQL的受影响行数</returns>  
    public static int ExecuteNonQueryWithGo(this SqlCommand oldCmd)
    {
        int result = 0;
        string[] arr = System.Text.RegularExpressions.Regex.Split(oldCmd.CommandText, "GO");
        using (SqlConnection conn = new SqlConnection(oldCmd.Connection.ConnectionString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < arr.Length; n++)
                {
                    string strsql = arr[n];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        result = cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                //return -1;  
                throw new Exception(E.Message);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        return result;
    }
}//end of class  