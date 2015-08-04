<%@ WebHandler Language="C#" Class="QueryHandler" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class QueryHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        string filter = context.Request["filter"].ToString();
        if (filter == "")
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("0"); ;
        }
        else
        {
            System.Data.DataTable dt = BudgetWeb.BLL.IncomeContrastpayLogic.GetUnitByName(filter);
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
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
 