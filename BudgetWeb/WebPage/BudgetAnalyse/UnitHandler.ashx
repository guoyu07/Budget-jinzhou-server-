<%@ WebHandler Language="C#" Class="UnitHandler" %>

using System;
using System.Web;
using System.Web;
using BudgetWeb.BLL; 

public class UnitHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        string unitCode = context.Request["UnitCode"];
        IncomeContrastpayLogic.UnitCode = unitCode;
        context.Response.Write("0");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}