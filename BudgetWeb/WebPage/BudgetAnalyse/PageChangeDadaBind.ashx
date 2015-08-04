<%@ WebHandler Language="C#" Class="PageChangeDadaBind" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class PageChangeDadaBind : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        int pageindex = Common.common.IntSafeConvert(context.Request["pageindex"]);
        int Pagesize = 14;
        System.Data.DataTable dt = BudgetWeb.BLL.IncomeContrastpayLogic.GetUnit(pageindex, Pagesize);
        System.Collections.Generic.List<JsonDateTmp> list = new List<JsonDateTmp>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            JsonDateTmp jdt = new JsonDateTmp();
            jdt.UnitCode = dt.Rows[i]["UnitCode"].ToString();
            jdt.UnitName = dt.Rows[i]["UnitName"].ToString();
            list.Add(jdt);    
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        
       // string jsonData = DataTableToJson("srcTable", dt);
        string jsonData = js.Serialize(list);
        context.Response.Write(jsonData);
    }

    public static string DataTable2Json(DataTable dt)
    {
        StringBuilder jsonBuilder = new StringBuilder();
        jsonBuilder.Append("{\"");
        jsonBuilder.Append(dt.TableName);
        jsonBuilder.Append("\":[");
        jsonBuilder.Append("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[i][j].ToString());
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");
        }
        jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
        jsonBuilder.Append("]");
        jsonBuilder.Append("}");
        return jsonBuilder.ToString();
    }

    /// <summary>
    /// 将DataTable转换成Json格式 
    /// </summary>
    /// <param name="jsonName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string DataTableToJson(string jsonName, System.Data.DataTable dt)
    {
        System.Text.StringBuilder Json = new System.Text.StringBuilder();
        Json.Append("[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("'" + dt.Columns[j].ColumnName.ToString() + "':'" + dt.Rows[i][j].ToString() + "'");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]");
        return Json.ToString();
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}

 